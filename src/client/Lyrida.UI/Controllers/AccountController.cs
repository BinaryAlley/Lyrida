#region ========================================================================= USING =====================================================================================
using System;
using System.Linq;
using Newtonsoft.Json;
using Lyrida.UI.Common.Api;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Lyrida.UI.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Lyrida.Infrastructure.Localization;
using Lyrida.Infrastructure.Common.Enums;
using Microsoft.AspNetCore.Authentication;
using Lyrida.Infrastructure.Common.Security;
using Lyrida.UI.Common.Entities.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http;
#endregion

namespace Lyrida.UI.Controllers;

/// <summary>
/// Controller for authentication related operations
/// </summary>
/// <remarks>
/// Creation Date: 25th of July, 2023
/// </remarks>
[Route("[controller]")]
public class AccountController : Controller
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IApiHttpClient apiHttpClient;
    private readonly ICryptography cryptographyService;
    private readonly ITranslationService translationService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    /// <param name="httpClient">Injected service for interactions with the API</param>
    /// <param name="translationService">Injected service for translations</param>
    /// <param name="cryptographyService">Injected service for cryptographic operations</param>
    public AccountController(IApiHttpClient httpClient, ITranslationService translationService, ICryptography cryptographyService)
    {
        this.apiHttpClient = httpClient;
        this.translationService = translationService;
        this.cryptographyService = cryptographyService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Logs out an user and redirects to the login page
    /// </summary>
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        // if the user is not logged in, redirect them to the home page
        if (User?.Identity?.IsAuthenticated == false)
            return RedirectToAction("Index", "Home");
        Response.Cookies.Delete("Token");
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }

    /// <summary>
    /// Logs out an user and redirects to the login page
    /// </summary>
    [HttpGet("changePassword")]
    [Authorize]
    public IActionResult ChangePassword()
    {
        return View(new ChangePasswordRequestEntity(null, null, null, null));
    }

    /// <summary>
    /// Logs out an user and redirects to the login page
    /// </summary>
    [HttpGet("profile")]
    [Authorize]
    public IActionResult Profile()
    {
        return View();
    }

    /// <summary>
    /// Returns the Register view with an empty RegisterEntity.
    /// </summary>
    [HttpGet("register")]
    public async Task<IActionResult> Register()
    {
        try
        {
            // check if the application was initialized
            var response = await apiHttpClient.GetAsync("initialization/", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
            // if the user is already logged in, redirect them to the home page
            if (response == "true" && User?.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            else
                // if the application was initialized, return a normal registration page, otherwise, registration for admin account
                ViewData["RegistrationType"] = response == "true" ? "User" : "Admin";
        }
        catch (ApiException ex)
        {
            ViewData["RegistrationType"] = ex.Error?.Errors?[0] == "Database not initialized" ? "Admin" : "User";
            Response.Cookies.Delete("Token");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        catch (Exception ex)
        {
            TempData["generalException"] = ex.Message;
        }
        return View(new RegisterRequestEntity(null, null, null, null, null, null));
    }

    /// <summary>
    /// Returns the Login view with an empty LoginEntity
    /// </summary>
    /// <param name="returnUrl">The url to return to after login</param>
    [HttpGet("login/{returnUrl?}")]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        // if the user is already logged in, redirect them to the home page
        if (User?.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");
        return View(new LoginEntity(null, null));
    }

    /// <summary>
    /// Returns the LoginPartial view, for in-app session expiration handling
    /// </summary>
    /// <param name="returnUrl">The url to return to after login</param>
    [HttpGet("loginPartial/{returnUrl}")]
    public IActionResult LoginPartial(string returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return PartialView("_LoginPartial");
    }

    /// <summary>
    /// Returns the RecoverPassword view.
    /// </summary>
    [HttpGet("recoverPassword")]
    public IActionResult RecoverPassword()
    {
        // if the user is already logged in, redirect them to the home page
        if (User?.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");
        return View();
    }

    /// <summary>
    /// </summary>
    /// <param name="token">The token to verify</param>
    /// <param name="lang">The language that was used to initiate the password reset request</param>
    [HttpGet("resetPassword")]
    public async Task<IActionResult> ResetPassword([FromQuery] string token, [FromQuery] string lang)
    {
        // if the user is already logged in, redirect them to the home page
        if (User?.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");
        SetLanguage(lang);
        try
        {
            var response = await apiHttpClient.PostAsync("authentication/verifyReset/", new ValidateTokenRequestEntity(token), language: translationService.Language);
            LoginResponseEntity? responseEntity = JsonConvert.DeserializeObject<LoginResponseEntity>(response);
            return View(new RecoverPasswordEntity(responseEntity?.Email, responseEntity?.Token, null, null));
        }
        catch (ApiException ex)
        {
            string errorMessage = ex.ToString();
            // treat validation errors differently - they come as a list of key-value pairs, where the key is the invalid field, and the value is the validation message
            if (ex.Error != null && ex.Error.ValidationErrors?.Count > 0)
                errorMessage = ex.Message + " " + string.Join(Environment.NewLine, ex.Error.ValidationErrors.Select(e => e.Value.First()));
            else if (ex.Error != null && ex.Error.Errors?.Count > 0) // other errors are just a simple list
                errorMessage = ex.Message + " " + string.Join(Environment.NewLine, ex.Error.Errors);
            ModelState.AddModelError("resetPasswordTokenError", errorMessage);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = false, errorMessage });
            else
                return View(new RecoverPasswordEntity(null, null, null, null));
        }
        catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
        {
            ModelState.AddModelError("resetPasswordTokenError", ex.Message);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = false, errorMessage = ex.Message });
            else
                return View(new RecoverPasswordEntity(null, null, null, null));
        }        
    }

    /// <summary>
    /// </summary>
    /// <param name="token">The token to verify</param>
    /// <param name="lang">The language that was used to register the account</param>
    [HttpGet("verify")]
    public async Task<IActionResult> Verify([FromQuery] string token, [FromQuery] string lang)
    {
        // if the user is already logged in, redirect them to the home page
        if (User?.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");
        SetLanguage(lang);
        try
        {
            //await authentication.VerifyTokenAsync(token);
            var response = await apiHttpClient.PostAsync("authentication/verifyRegister/", new ValidateTokenRequestEntity(token), language: translationService.Language);
            LoginResponseEntity? responseEntity = JsonConvert.DeserializeObject<LoginResponseEntity>(response);
            TempData["ValidateAccountSuccess"] = translationService.Translate(Terms.EmailVerified);
            // store the received token in a cookie            
            Response.Cookies.Delete("Token");
            Response.Cookies.Append("Token", cryptographyService.Encrypt(responseEntity?.Token!), new CookieOptions 
            { 
                Expires = DateTimeOffset.UtcNow.AddYears(1), 
                Path = "/", 
                HttpOnly = true, 
                Secure = true 
            });
        }
        catch (ApiException ex)
        {
            string errorMessage = ex.ToString();
            // treat validation errors differently - they come as a list of key-value pairs, where the key is the invalid field, and the value is the validation message
            if (ex.Error != null && ex.Error.ValidationErrors?.Count > 0)
                errorMessage = ex.Message + " " + string.Join(Environment.NewLine, ex.Error.ValidationErrors.Select(e => e.Value.First()));
            else if (ex.Error != null && ex.Error.Errors?.Count > 0) // other errors are just a simple list
                errorMessage = ex.Message + " " + string.Join(Environment.NewLine, ex.Error.Errors);
            else if (ex.Error != null && ex.Error.Status == 401)
            {
                TempData["loginError"] = translationService.Translate(Terms.SessionExpired);
                return RedirectToAction("Login", "Account");
            }
            TempData["ValidateAccountError"] = errorMessage;
        }
        catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
        {
            TempData["ValidateAccountError"] = ex.Message;
        }
        return View();
    }

    /// <summary>
    /// Sets the language used by the user
    /// </summary>
    /// <param name="language">The identifier of the language to set</param>
    private void SetLanguage(string language)
    {
        if (!string.IsNullOrEmpty(language) && (language == "ro" || language == "en" || language == "de"))
        {
            Response.Cookies.Delete("Language");
            Response.Cookies.Append("Language", language, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), Path = "/", HttpOnly = true, Secure = true });
            translationService.Language = language == "en" ? Language.English : language == "ro" ? Language.Romanian : Language.German;
        }
    }

    /// <summary>
    /// Registers a new account.
    /// </summary>
    /// <param name="entity">User credentials used for registration</param>
    [HttpPost("register")]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterRequestEntity entity)
    {
        ModelState.Clear(); // clear all existing errors, use translated messages instead
        if (string.IsNullOrEmpty(entity.FirstName))
            ModelState.AddModelError("firstNameError", translationService.Translate(Terms.EmptyFirstName));
        if (string.IsNullOrEmpty(entity.LastName))
            ModelState.AddModelError("lastNameError", translationService.Translate(Terms.EmptyLastName));
        if (string.IsNullOrEmpty(entity.Email))
            ModelState.AddModelError("emailError", translationService.Translate(Terms.EmptyEmail));
        if (string.IsNullOrEmpty(entity.Password))
            ModelState.AddModelError("passwordError", translationService.Translate(Terms.EmptyPassword));
        if (string.IsNullOrEmpty(entity.PasswordConfirm))
            ModelState.AddModelError("passwordConfirmError", translationService.Translate(Terms.EmptyPasswordConfirm));
        // handle cases when mandatory info is not provided...
        if (!ModelState.IsValid)
        {
            // ...in AJAX way
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var errors = new
                {
                    FirstNameError = ModelState.ContainsKey("firstNameError") ? ModelState["firstNameError"]!.Errors[0].ErrorMessage : null,
                    LastNameError = ModelState.ContainsKey("lastNameError") ? ModelState["lastNameError"]!.Errors[0].ErrorMessage : null,
                    EmailError = ModelState.ContainsKey("emailError") ? ModelState["emailError"]!.Errors[0].ErrorMessage : null,
                    PasswordError = ModelState.ContainsKey("passwordError") ? ModelState["passwordError"]!.Errors[0].ErrorMessage : null,
                    PasswordConfirmError = ModelState.ContainsKey("passwordConfirmError") ? ModelState["passwordConfirmError"]!.Errors[0].ErrorMessage : null,
                };
                return Json(errors);
            }
            else // or FORM way
                return View(entity);
        }
        else
        {
            try
            {
                // call different endpoints based on the view hidden field - registration for normal users, or initial application admin account setup
                string endpoint = entity.RegistrationType == "Admin" ? "initialization" : "authentication/register";
                // attempt API registration
                await apiHttpClient.PostAsync(endpoint, entity, language: translationService.Language);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = true }); // return success status 
                else
                {
                    TempData["registerSuccess"] = translationService.Translate(entity.RegistrationType == "Admin" ? Terms.AccountCreated : Terms.RegistrationEmailSent);
                    return View();
                }
            }
            catch (ApiException ex)
            {
                string errorMessage = ex.ToString();
                // treat validation errors differently - they come as a list of key-value pairs, where the key is the invalid field, and the value is the validation message
                if (ex.Error != null && ex.Error.ValidationErrors?.Count > 0)
                    errorMessage = ex.Message + " " + string.Join(Environment.NewLine, ex.Error.ValidationErrors.Select(e => e.Value.First()));
                else if (ex.Error != null && ex.Error.Errors?.Count > 0) // other errors are just a simple list
                    errorMessage = ex.Message + " " + string.Join(Environment.NewLine, ex.Error.Errors);
                ModelState.AddModelError("registerError", errorMessage);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, errorMessage });
                else
                    return View(entity);
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("registerError", ex.Message);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, errorMessage = ex.Message });
                else
                    return View(entity);
            }
        }
    }

    /// <summary>
    /// Authenticates the user and redirects to the home page upon successful authentication,
    /// or returns the Login view with error messages upon failed authentication.
    /// </summary>
    /// <param name="entity">User credentials</param>
    [HttpPost("login")]
    //[ValidateAntiForgeryToken] TODO: enable this!
    public async Task<IActionResult> Login(LoginEntity entity, string? returnUrl = null)
    {
        ModelState.Clear(); // clear all existing errors, use translated messages instead
        if (string.IsNullOrEmpty(entity.Email))
            ModelState.AddModelError("emailError", translationService.Translate(Terms.EmptyUsername));
        if (string.IsNullOrEmpty(entity.Password))
            ModelState.AddModelError("passwordError", translationService.Translate(Terms.EmptyPassword));
        // handle cases when mandatory info is not provided...
        if (!ModelState.IsValid)
        {
            // ...in AJAX way
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var errors = new
                {
                    EmailError = ModelState.ContainsKey("emailError") ? ModelState["emailError"]!.Errors[0].ErrorMessage : null,
                    PasswordError = ModelState.ContainsKey("passwordError") ? ModelState["passwordError"]!.Errors[0].ErrorMessage : null
                };
                return Json(errors);
            }
            else // or FORM way
                return View(entity);
        }
        else
        {
            try
            {
                // attempt API login
                var response = await apiHttpClient.PostAsync("authentication/login/", entity, language: translationService.Language);
                var responseEntity = JsonConvert.DeserializeObject<LoginResponseEntity>(response);

                //var responseUserPermissions = await apiHttpClient.GetAsync($"users/{responseEntity!.Id}/permissions", responseEntity?.Token!, translationService.Language);
                //var userPermissions = JsonConvert.DeserializeObject<UserPermissionEntity[]>(responseUserPermissions);
                //var responseUserRoles = await apiHttpClient.GetAsync($"users/{responseEntity!.Id}/roles", responseEntity?.Token!, translationService.Language);
                //var userRoles = JsonConvert.DeserializeObject<RoleEntity[]>(responseUserRoles);
                //var responseRolePermissions = await apiHttpClient.GetAsync($"roles/{userRoles![0].Id}/permissions", responseEntity?.Token!, translationService.Language);
                //var rolePermissions = JsonConvert.DeserializeObject<PermissionEntity[]>(responseRolePermissions);

                // store the received token in a secure cookie            
                Response.Cookies.Delete("Token");
                Response.Cookies.Append("Token", cryptographyService.Encrypt(responseEntity?.Token!), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMonths(1),
                    Path = "/",
                    HttpOnly = true,
                    Secure = true
                });
                // tell asp.net we are logged in
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, responseEntity?.Email!),
                    new Claim(ClaimTypes.NameIdentifier, responseEntity?.Id.ToString()!),
                    // You can add other claims as needed, for example, you might add a claim for the JWT token
                    new Claim("Token", responseEntity?.Token!),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                // return success status and redirect URL (also, handle cases when a return url was specified)
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = true, redirectUrl = string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl) ? Url.Content("~/") : Url.Content(returnUrl) });
                else
                    return Redirect(string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl) ? "~/" : returnUrl);
            }
            catch (ApiException ex)
            {
                string errorMessage = ex.ToString();
                // treat validation errors differently - they come as a list of key-value pairs, where the key is the invalid field, and the value is the validation message
                if (ex.Error != null && ex.Error.ValidationErrors?.Count > 0)
                    errorMessage = ex.Message + " " + string.Join(Environment.NewLine, ex.Error.ValidationErrors.Select(e => e.Value.First()));
                else if (ex.Error != null && ex.Error.Errors?.Count > 0)
                    errorMessage = ex.Message + " " + string.Join(Environment.NewLine, ex.Error.Errors);
                else
                    errorMessage = ex.Message;
                ModelState.AddModelError("registerError", errorMessage);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, errorMessage });
                else
                    return View(entity);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("loginError", ex.Message);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, errorMessage = ex.Message });
                else
                    return View(entity);
            }
        }
    }

    /// <summary>
    /// Recovers the password of a user identified by <paramref name="email"/>
    /// </summary>
    /// <param name="email">The email account of the user for who to recover the password</param>
    [HttpPost("recoverPassword")]
    public async Task<IActionResult> RecoverPassword(string email)
    {
        ModelState.Clear(); // clear all existing errors
        if (string.IsNullOrEmpty(email))
            ModelState.AddModelError("recoverPasswordError", translationService.Translate(Terms.EmptyEmail));
        if (!ModelState.IsValid)
        {
            // handle errors in AJAX way
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var errors = new
                {
                    EmailError = ModelState.ContainsKey("recoverPasswordError") ? ModelState["recoverPasswordError"]!.Errors[0].ErrorMessage : null
                };
                return Json(errors);
            }
            else // or FORM way
                return View("RecoverPassword", email);
        }
        else
        {
            try
            {
                var response = await apiHttpClient.PostAsync("authentication/recoverPassword/", new { email }, language: translationService.Language);
                var responseEntity = JsonConvert.DeserializeObject<LoginResponseEntity>(response);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = true });
                else
                {
                    TempData["recoverPasswordSuccess"] = translationService.Translate(Terms.PasswordResetEmailSent);
                    return View();
                }
            }
            catch (ApiException ex)
            {
                string errorMessage = ex.ToString();
                // treat validation errors differently - they come as a list of key-value pairs, where the key is the invalid field, and the value is the validation message
                if (ex.Error != null && ex.Error.ValidationErrors?.Count > 0)
                    errorMessage = ex.Message + " " + string.Join(Environment.NewLine, ex.Error.ValidationErrors.Select(e => e.Value.First()));
                else if (ex.Error != null && ex.Error.Errors?.Count > 0) // other errors are just a simple list
                    errorMessage = ex.Message + " " + string.Join(Environment.NewLine, ex.Error.Errors);
                ModelState.AddModelError("recoverPasswordError", errorMessage);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, errorMessage });
                else
                    return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("recoverPasswordError", ex.Message);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, emailError = ex.Message });
                else
                    return View("RecoverPassword", email);
            }
        }
    }

    /// <summary>
    /// Resets the password of a user 
    /// </summary>
    /// <param name="entity">The entity representing the user and the password to reset</param>
    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword(RecoverPasswordEntity entity)
    {
        ModelState.Clear(); // clear all existing errors, use translated messages instead
        if (string.IsNullOrEmpty(entity.Email))
            ModelState.AddModelError("email", translationService.Translate(Terms.EmptyUsername));
        if (string.IsNullOrEmpty(entity.Password))
            ModelState.AddModelError("password", translationService.Translate(Terms.EmptyPassword));
        if (string.IsNullOrEmpty(entity.PasswordConfirm))
            ModelState.AddModelError("passwordConfirm", translationService.Translate(Terms.EmptyPasswordConfirm));
        // handle cases when mandatory info is not provided...
        if (!ModelState.IsValid)
        {
            // ...in AJAX way
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var errors = new
                {
                    PasswordError = ModelState.ContainsKey("password") ? ModelState["password"]!.Errors[0].ErrorMessage : null,
                    PasswordConfirmError = ModelState.ContainsKey("passwordConfirm") ? ModelState["passwordConfirm"]!.Errors[0].ErrorMessage : null
                };
                return Json(errors);
            }
            else // or FORM way
                return View(entity);
        }
        else
        {
            try
            {
                // attempt backend password reset, also send the token that was offered when the verification link was clicked
                var response = await apiHttpClient.PostAsync("authentication/resetPassword/", entity, token: entity.Token, language: translationService.Language);
                var responseEntity = JsonConvert.DeserializeObject<LoginResponseEntity>(response);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = true });
                else
                {
                    TempData["resetPasswordSuccess"] = translationService.Translate(Terms.PasswordChanged);
                    return View();
                }
            }
            catch (ApiException ex)
            {
                string errorMessage = ex.ToString();
                // treat validation errors differently - they come as a list of key-value pairs, where the key is the invalid field, and the value is the validation message
                if (ex.Error != null && ex.Error.ValidationErrors?.Count > 0)
                    errorMessage = ex.Message + " " + string.Join(Environment.NewLine, ex.Error.ValidationErrors.Select(e => e.Value.First()));
                else if (ex.Error != null && ex.Error.Errors?.Count > 0) // other errors are just a simple list
                    errorMessage = ex.Message + " " + string.Join(Environment.NewLine, ex.Error.Errors);
                ModelState.AddModelError("resetPasswordError", errorMessage);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, errorMessage });
                else
                    return View();
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                ModelState.AddModelError("resetPasswordError", ex.Message);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, errorMessage = ex.Message });
                else
                    return View(entity);
            }
        }
    }

    /// <summary>
    /// Changes the password of a user
    /// </summary>
    /// <param name="entity">The entity representing the user and the password to change</param>
    [HttpPost("changePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequestEntity entity)
    {
        ModelState.Clear(); // clear all existing errors, use translated messages instead
        if (string.IsNullOrEmpty(entity.CurrentPassword))
            ModelState.AddModelError("currentPassword", translationService.Translate(Terms.EmptyCurrentPassword));
        if (string.IsNullOrEmpty(entity.NewPassword))
            ModelState.AddModelError("newPassword", translationService.Translate(Terms.EmptyNewPassword));
        if (string.IsNullOrEmpty(entity.NewPasswordConfirm))
            ModelState.AddModelError("newPasswordConfirm", translationService.Translate(Terms.EmptyNewPasswordConfirm));
        // handle cases when mandatory info is not provided...
        if (!ModelState.IsValid)
        {
            // ...in AJAX way
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var errors = new
                {
                    CurrentPasswordError = ModelState.ContainsKey("currentPassword") ? ModelState["currentPassword"]!.Errors[0].ErrorMessage : null,
                    NewPasswordError = ModelState.ContainsKey("newPassword") ? ModelState["newPassword"]!.Errors[0].ErrorMessage : null,
                    NewPasswordConfirmError = ModelState.ContainsKey("newPasswordConfirm") ? ModelState["newPasswordConfirm"]!.Errors[0].ErrorMessage : null
                };
                return Json(errors);
            }
            else // or FORM way
                return View(entity);
        }
        else
        {
            try
            {
                ChangePasswordRequestEntity newEntity = new(User?.Identity?.Name, entity.CurrentPassword, entity.NewPassword, entity.NewPasswordConfirm);
                // attempt backend password change
                var response = await apiHttpClient.PostAsync("authentication/changePassword/", newEntity, HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
                var responseEntity = JsonConvert.DeserializeObject<LoginResponseEntity>(response);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = true });
                else
                {
                    TempData["changePasswordSuccess"] = translationService.Translate(Terms.PasswordChanged);
                    return View();
                }
            }
            catch (ApiException ex)
            {
                string errorMessage = ex.ToString();
                // treat validation errors differently - they come as a list of key-value pairs, where the key is the invalid field, and the value is the validation message
                if (ex.Error != null && ex.Error.ValidationErrors?.Count > 0)
                    errorMessage = ex.Message + " " + string.Join(Environment.NewLine, ex.Error.ValidationErrors.Select(e => e.Value.First()));
                else if (ex.Error != null && ex.Error.Errors?.Count > 0) // other errors are just a simple list
                    errorMessage = ex.Message + " " + string.Join(Environment.NewLine, ex.Error.Errors);
                ModelState.AddModelError("changePasswordError", errorMessage);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, errorMessage });
                else
                    return View();
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                ModelState.AddModelError("changePasswordError", ex.Message);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, errorMessage = ex.Message });
                else
                    return View(entity);
            }
        }
    }
    #endregion
}