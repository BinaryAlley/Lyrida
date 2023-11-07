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
using Lyrida.UI.Common.DTO.Configuration;
using Microsoft.AspNetCore.Authentication;
using Lyrida.UI.Common.DTO.Authentication;
using Lyrida.Infrastructure.Common.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    /// Logs out an user and redirects to the login page.
    /// </summary>
    [HttpGet("Logout")]
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
    /// Displays the view for changing the password.
    /// </summary>
    [HttpGet("ChangePassword")]
    [Authorize]
    public IActionResult ChangePassword()
    {
        return View(new ChangePasswordRequestDto());
    }

    /// <summary>
    /// Displays the view for profile preferences.
    /// </summary>
    [HttpGet("Profile")]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        string response = await apiHttpClient.GetAsync($"account/getPreferences", HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
        ProfilePreferencesDto? profilePreferences = JsonConvert.DeserializeObject<ProfilePreferencesDto>(response);
        return View(profilePreferences);
    }

    /// <summary>
    /// Displays the view for registering a new account.
    /// </summary>
    [HttpGet("Register")]
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
        return View(new RegisterRequestDto());
    }

    /// <summary>
    /// Displays the view for logging in.
    /// </summary>
    /// <param name="returnUrl">The url to return to, after login (if any).</param>
    [HttpGet("Login/{returnUrl?}")]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        // if the user is already logged in, redirect them to the home page
        if (User?.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");
        return View(new LoginDto());
    }

    /// <summary>
    /// Displays the view for recovering the password of an account.
    /// </summary>
    [HttpGet("RecoverPassword")]
    public IActionResult RecoverPassword()
    {
        // if the user is already logged in, redirect them to the home page
        if (User?.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");
        return View();
    }

    /// <summary>
    /// Registers a new account.
    /// </summary>
    /// <param name="data">User credentials used for registration.</param>
    [HttpPost("Register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterRequestDto data)
    {
        // handle cases when mandatory info is not provided
        if (string.IsNullOrEmpty(data.FirstName) || string.IsNullOrEmpty(data.LastName) || string.IsNullOrEmpty(data.Email) 
            || string.IsNullOrEmpty(data.Password) ||string.IsNullOrEmpty(data.PasswordConfirm))            
        {
            var errors = new
            {
                FirstNameError = string.IsNullOrEmpty(data.FirstName) ? translationService.Translate(Terms.EmptyFirstName) : null,
                LastNameError = string.IsNullOrEmpty(data.LastName) ? translationService.Translate(Terms.EmptyLastName) : null,
                EmailError = string.IsNullOrEmpty(data.Email) ? translationService.Translate(Terms.EmptyEmail) : null,
                PasswordError = string.IsNullOrEmpty(data.Password) ? translationService.Translate(Terms.EmptyPassword) : null,
                PasswordConfirmError = string.IsNullOrEmpty(data.PasswordConfirm) ? translationService.Translate(Terms.EmptyPasswordConfirm) : null,
            };
            return Json(errors);
        }
        else
        {
            try
            {
                // call different endpoints based on the view hidden field - registration for normal users, or initial application admin account setup
                string endpoint = data.RegistrationType == "Admin" ? "initialization" : "authentication/register";
                // attempt API registration
                var response = await apiHttpClient.PostAsync(endpoint, data, language: translationService.Language);
                RegisterResponseDto? responseDto = JsonConvert.DeserializeObject<RegisterResponseDto>(response);
                return Json(new { success = true, registrationData = responseDto }); // return success status 
            }
            catch (ApiException ex)
            {
                string errorMessage = ex.ToString();
                // treat validation errors differently - they come as a list of key-value pairs, where the key is the invalid field, and the value is the validation message
                if (ex.Error != null && ex.Error.ValidationErrors?.Count > 0)
                    errorMessage = ex.Message + " " + string.Join(Environment.NewLine, ex.Error.ValidationErrors.Select(e => e.Value.First()));
                else if (ex.Error != null && ex.Error.Errors?.Count > 0) // other errors are just a simple list
                    errorMessage = ex.Message + " " + string.Join(Environment.NewLine, ex.Error.Errors);
                return Json(new { success = false, errorMessage });
            }
            catch (Exception ex) 
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }
    }

    /// <summary>
    /// Authenticates the user.
    /// </summary>
    /// <param name="data">The credentials of the user to authenticate.</param>
    [HttpPost("Login")]
    [ValidateAntiForgeryToken] 
    public async Task<IActionResult> Login(LoginDto data, string? returnUrl = null)
    {
        // handle cases when mandatory info is not provided
        if (string.IsNullOrEmpty(data.Email) || string.IsNullOrEmpty(data.Password))
        {
            var errors = new
            {
                EmailError = string.IsNullOrEmpty(data.Email) ? translationService.Translate(Terms.EmptyUsername) : null,
                PasswordError = string.IsNullOrEmpty(data.Password) ? translationService.Translate(Terms.EmptyPassword) : null
            };
            return Json(errors);
        }
        else
        {
            try
            {
                // attempt API login
                string response = await apiHttpClient.PostAsync("authentication/login/", data, language: translationService.Language);
                LoginResponseDto? responseDto = JsonConvert.DeserializeObject<LoginResponseDto>(response);
                if (responseDto!.UsesTotp)
                    if (string.IsNullOrEmpty(data.TotpCode))
                        return Json(new { success = false, TotpError = translationService.Translate(Terms.EmptyTotp) });
                // store the received token in a secure cookie            
                Response.Cookies.Delete("Token");
                Response.Cookies.Append("Token", cryptographyService.Encrypt(responseDto?.Token!), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMonths(1),
                    Path = "/",
                    HttpOnly = true,
                    Secure = true
                });
                // tell asp.net we are logged in
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.Name, responseDto?.Email!),
                    new Claim(ClaimTypes.NameIdentifier, responseDto?.Id.ToString()!),
                    // You can add other claims as needed, for example, you might add a claim for the JWT token
                    new Claim("Token", responseDto?.Token!),
                };
                ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                // return success status and redirect URL (also, handle cases when a return url was specified)
                return Json(new { success = true, redirectUrl = string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl) ? Url.Content("~/") : Url.Content(returnUrl) });
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
                return Json(new { success = false, errorMessage });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }
    }

    /// <summary>
    /// Recovers the password of a user identified by <paramref name="email"/>.
    /// </summary>
    /// <param name="email">The email account of the user for who to recover the password.</param>
    [HttpPost("RecoverPassword")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RecoverPassword(string email, string totpCode)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(totpCode))
        {
            var errors = new
            {
                EmailError = string.IsNullOrEmpty(email) ? translationService.Translate(Terms.EmptyEmail) : null,
                TotpError = string.IsNullOrEmpty(totpCode) ? translationService.Translate(Terms.EmptyTotp) : null
            };
            return Json(errors);
        }
        else
        {
            await apiHttpClient.PostAsync("authentication/recoverPassword/", new { email, totpCode }, language: translationService.Language);
            return Json(new { success = true });
        }
    }

    /// <summary>
    /// Changes the password of a user.
    /// </summary>
    /// <param name="data">The data representing the user and the password to change.</param>
    [HttpPost("ChangePassword")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto data)
    {
        // handle cases when mandatory info is not provided...
        if (string.IsNullOrEmpty(data.CurrentPassword) || string.IsNullOrEmpty(data.NewPassword) || string.IsNullOrEmpty(data.NewPasswordConfirm))
        {
            var errors = new
            {
                CurrentPasswordError = string.IsNullOrEmpty(data.CurrentPassword) ? translationService.Translate(Terms.EmptyCurrentPassword) : null,
                NewPasswordError = string.IsNullOrEmpty(data.NewPassword) ? translationService.Translate(Terms.EmptyNewPassword) : null,
                NewPasswordConfirmError = string.IsNullOrEmpty(data.NewPasswordConfirm) ? translationService.Translate(Terms.EmptyNewPasswordConfirm) : null
            };
            return Json(errors);
        }
        else
        {
            try
            {
                ChangePasswordRequestDto newDto = new() { Email = User?.Identity?.Name, CurrentPassword = data.CurrentPassword, NewPassword = data.NewPassword, NewPasswordConfirm = data.NewPasswordConfirm };
                // attempt backend password change
                var response = await apiHttpClient.PostAsync("authentication/changePassword/", newDto, HttpContext.Items["UserToken"]?.ToString(), translationService.Language);
                var responseDto = JsonConvert.DeserializeObject<LoginResponseDto>(response);
                return Json(new { success = true });
            }
            catch (ApiException ex)
            {
                string errorMessage = ex.ToString();
                // treat validation errors differently - they come as a list of key-value pairs, where the key is the invalid field, and the value is the validation message
                if (ex.Error != null && ex.Error.ValidationErrors?.Count > 0)
                    errorMessage = ex.Message + " " + string.Join(Environment.NewLine, ex.Error.ValidationErrors.Select(e => e.Value.First()));
                else if (ex.Error != null && ex.Error.Errors?.Count > 0) // other errors are just a simple list
                    errorMessage = ex.Message + " " + string.Join(Environment.NewLine, ex.Error.Errors);
                return Json(new { success = false, errorMessage });
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }
    }

    /// <summary>
    /// Updates the profile preferences of a the currently authenticated user.
    /// </summary>
    /// <param name="data">The profile preferences to be updated.</param>
    [HttpPost("Profile")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(ProfilePreferencesDto data)
    {
        if (data.ImagePreviewsQuality <= 0 || data.ImagePreviewsQuality > 100)
            return Json(new { success = false, imagePreviewQualityError = translationService.Translate(Terms.ValueBetweenZeroAndOneHundred) });
        if (data.FullImageQuality <= 0 || data.FullImageQuality > 100)
            return Json(new { success = false, fullImageQualityError = translationService.Translate(Terms.ValueBetweenZeroAndOneHundred) });
        if (data.ThumbnailsRetrievalBatchSize <= 0)
            return Json(new { success = false, thumbnailsRetrievalBatchSizeError = translationService.Translate(Terms.ValueGreaterThanZero) });
        if (data.ScrollThumbnailRetrievalTimeout <= 0)
            return Json(new { success = false, scrollThumbnailRetrievalTimeoutError = translationService.Translate(Terms.ValueGreaterThanZero) });
        var response = await apiHttpClient.PutAsync("account/updatePreferences/", data, HttpContext.Items["UserToken"]?.ToString(), language: translationService.Language);
        var responseDto = JsonConvert.DeserializeObject<ProfilePreferencesDto>(response);
        return Json(new { success = true, preferences = responseDto });
    }

    /// <summary>
    /// Enables 2FA for the currently authenticated user.
    /// </summary>
    [HttpGet("EnableTotp")]
    public async Task<IActionResult> EnableTotp()
    {
        string response = await apiHttpClient.GetAsync("authentication/generateTOTPQR/", HttpContext.Items["UserToken"]?.ToString(), language: translationService.Language);
        QrCodeResultDto? responseDto = JsonConvert.DeserializeObject<QrCodeResultDto>(response);
        return Json(new { success = true, totpSecret = responseDto });
    }
    #endregion
}