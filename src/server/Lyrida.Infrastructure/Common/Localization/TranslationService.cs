#region ========================================================================= USING =====================================================================================
using Lyrida.Infrastructure.Common.Enums;
using Lyrida.Infrastructure.Common.Configuration;
#endregion

namespace Lyrida.Infrastructure.Localization;

/// <summary>
/// Service for strings translation
/// </summary>
/// <remarks>
/// Creation Date: 18th of January, 2022
/// </remarks>
public class TranslationService : ITranslationService
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IAppConfig appConfig;
    private readonly TranslationsCollection translations;
    #endregion

    #region ==================================================================== PROPERTIES =================================================================================
    private Language language = Language.English;
    public Language Language
    {
        get { return language; } 
        set { language = value; }
    }
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="appConfig">Injected application configuration service</param>
    public TranslationService(IAppConfig appConfig)
    {
        this.appConfig = appConfig;
        translations = new TranslationsCollection
        {
            (Terms.GetUsersError, "Eroare la preluarea listei de utilizatori din mediul de stocare!", "Error getting the users list from the storage medium!", "Fehler beim Abrufen der Benutzerliste aus dem Speichermedium!"),
            (Terms.UninitializedDatabaseError, "Database not initialized", "Database not initialized", "Database not initialized"), // this MUST be the same for any language!
            (Terms.InvalidTotpCode, "Codul TOTP este invalid!", "TOTP code is invalid!", "Der TOTP-Code ist ungültig!"), 
            (Terms.InsertUserPreferencesError, "Eroare la inserarea preferințelor de utilizator în mediul de stocare!", "Error inserting the user preferences in the storage medium!", "Fehler beim Einfügen der Benutzereinstellungen im Speichermedium!"), 
            (Terms.InsertUserPageError, "Eroare la inserarea paginii utilizatorului în mediul de stocare!", "Error inserting the user page in the storage medium!", "Fehler beim Einfügen der Benutzerseite im Speichermedium!"), 
            (Terms.UpdateUserPreferencesError, "Eroare la actualizarea preferințelor de utilizator în mediul de stocare!", "Error updating the user preferences in the storage medium!", "Fehler beim Aktualisieren der Benutzereinstellungen im Speichermedium!"), 
            (Terms.GetUserPreferencesError, "Eroare la preluarea preferințelor de utilizator din mediul de stocare!", "Error getting the user preferences from the storage medium!", "Fehler beim Abrufen der Benutzereinstellungen aus dem Speichermedium!"), 
            (Terms.GetUserPagesError, "Eroare la preluarea paginilor utilizatorului din mediul de stocare!", "Error getting the user pages from the storage medium!", "Fehler beim Abrufen der Benutzerseiten aus dem Speichermedium!"), 
            (Terms.NoThumbnail, "Fișierul nu conține o pictogramă!", "The file does not contain a thumbnail!", "Die Datei enthält kein Vorschaubild!"), 
            (Terms.DuplicateEmailError, "Adresa de e-mail este folosită deja!", "The e-mail address is already used!", "Die E-Mail-Adresse wird bereits verwendet!"),
            (Terms.TotpCodeCannotBeEmpty, "Codul TOTP nu poate fi gol!", "TOTP code cannot be empty!", "Der TOTP-Code darf nicht leer sein!"),
            (Terms.CannotNavigateUpError, "Nu se poate naviga un nivel mai sus!", "Cannot navigate one level up!", "Kann nicht eine Ebene nach oben navigieren!"),
            (Terms.DatabaseSetupError, "Eroare la inițializarea bazei de date!", "Error initializing the database!", "Fehler bei der Initialisierung der Datenbank!"),
            (Terms.InvalidPathError, "Calea nu este validă!", "The path is invalid!", "Der Pfad ist ungültig!"),
            (Terms.DeleteUserError, "Eroare la ștergerea utilizatorului din mediul de stocare!", "Error deleting the user from the storage medium!", "Fehler beim Löschen des Benutzers aus dem Speichermedium!"),
            (Terms.DeleteUserPageError, "Eroare la ștergerea paginii utilizatorului din mediul de stocare!", "Error deleting the user page from the storage medium!", "Fehler beim Löschen der Benutzerseite aus dem Speichermedium!"),
            (Terms.UpdateRoleError, "Eroare la actualizarea rolului în mediul de stocare!", "Error updating the role in the storage medium!", "Fehler beim Aktualisieren der Rolle im Speichermedium!"),

            (Terms.Conflict, "A apărut un conflict!", "You have a conflict!", "Ein Konflikt ist aufgetreten!"),
            (Terms.NotFound, "Resursa nu a putut fi gasită!", "The resource was not found!", "Die Ressource wurde nicht gefunden!"),
            (Terms.BadRequest, "Cerere invalidă!", "Invalid request!", "Ungültige Anfrage!"),
            (Terms.Unauthorized, "Nu aveți autorizația necesară pentru efectuarea aceastei acțiuni!", "You are not authorized to perform this action!", "Sie sind nicht berechtigt, diese Aktion auszuführen!"),
            (Terms.Forbidden, "Accesul la această resursă este interzis!", "Access to this resource is forbidden!", "Der Zugriff auf diese Ressource ist verboten!"),
            (Terms.InternalServerError, "Eroare internă de server!", "Internal server error!", "Interner Serverfehler!"),
            (Terms.ValidationError, "Au survenit una sau mai multe erori de validare!", "One or more validation errors occurred!", "Es ist ein oder mehrere Validierungsfehler aufgetreten!"),
            (Terms.IdMustBePositive, "Id-ul trebuie să fie pozitiv!", "The id must be positive!", "Die ID muss positiv sein!"),
            (Terms.NameCannotBeEmpty, "Numele nu poate fi gol!", "The name cannot be empty!", "Name darf nicht leer sein!"),
            (Terms.MaximumLength200, "Lungimea nu poate depasi 200 caractere!", "The length cannot exceed 200 characters!", "Länge darf nicht mehr als 200 Zeichen betragen!"),
            (Terms.MaximumLength1000, "Lungimea nu poate depasi 1000 caractere!", "The length cannot exceed 1000 characters!", "Länge darf nicht mehr als 1000 Zeichen betragen!"),
            (Terms.FirstNameCannotBeEmpty, "Prenumele nu poate fi gol!", "First name cannot be empty!", "Vorname darf nicht leer sein!"),
            (Terms.LastNameCannotBeEmpty, "Numele nu poate fi gol!", "Last name cannot be empty!", "Nachname darf nicht leer sein!"),
            (Terms.EmailCannotBeEmpty, "Adresa de e-mail nu poate fi goală!", "E-mail cannot be empty!", "E-Mail darf nicht leer sein!"),
            (Terms.RoleMustHavePermissions, "Rolul trebuie să aibă cel puțin o permisiune!", "The role must have at least one permission!", "Die Rolle muss mindestens eine Berechtigung haben!"),
            (Terms.RoleNameCannotBeEmpty, "Numele rolului nu poate fi gol!", "The role name cannot be empty!", "Der Rollenname darf nicht leer sein!"),
            (Terms.ProfilePreferencesCannotBeEmpty, "Preferințele de cont nu pot fi goale!", "The profile preferences cannot be empty!", "Die Profilpräferenzen dürfen nicht leer sein!"),
            (Terms.InvalidPassword, "Parola trebuie să aibă minim 8 caractere și trebuie să conțină cel puțin o minusculă, o majusculă, un număr și un caracter special!", "The password must be at least 8 characters long and it must contain at least one lowercase character, one uppercase, a number and a special character!", "Das Passwort muss mindestens 8 Zeichen lang sein und mindestens einen Kleinbuchstaben, einen Großbuchstaben, eine Zahl und ein Sonderzeichen enthalten."),
            (Terms.PasswordCannotBeEmpty, "Parola nu poate fi goală!", "Password cannot be empty!", "Passwort darf nicht leer sein!"),
            (Terms.EmptyPasswordConfirm, "Confirmarea parolei nu poate fi goală!", "The password confirm cannot be empty!", "Die Passwortbestätigung darf nicht leer sein!"),
            (Terms.EmptyCurrentPassword, "Parola curentă nu poate fi goală!", "The current password cannot be empty!", "Das aktuelle Passwort darf nicht leer sein!"),
            (Terms.EmptyNewPassword, "Parola nouă nu poate fi goală!", "The new password cannot be empty!", "Das neue Passwort darf nicht leer sein!"),
            (Terms.EmptyNewPasswordConfirm, "Confirmarea parolei noi nu poate fi goală!", "The new password confirmation cannot be empty!", "Die Bestätigung des neuen Passworts darf nicht leer sein!"),
            (Terms.PasswordsNotMatch, "Parola și confirmarea parolei nu sunt identice!", "Password and password confirm are not identical!", "Passwort und Passwortbestätigung stimmen nicht überein!"),
            (Terms.InvalidUsername, "Nume de utilizator sau parola invalidă!", "Invalid username or password!", "Ungültiger Benutzername oder Passwort!"),
            (Terms.InvalidUserId, "Id de utilizator invalid!", "Invalid user id!", "Ungültige Benutzer-ID!"),
            (Terms.InvalidEmailAddress, "Adresa de E-Mail nu este validă!", "Invalid E-Mail address!", "Ungültige E-Mail-Adresse!"),
            (Terms.RoleDoesNotExist, "Rolul specificat nu există!", "The specified role does not exist!", "Die angegebene Rolle existiert nicht!"),
            (Terms.RoleAlreadyExists, "Rolul specificat există deja!", "The specified role already exists!", "Die angegebene Rolle existiert bereits!"),
            (Terms.CannotDeleteAdminAccount, "Contul de administrator nu poate fi șters!", "The administrator account cannot be deleted!", "Das Administrator-Konto kann nicht gelöscht werden!"),
            (Terms.CannotSetAdminRole, "Rolul de administrator nu poate fi atribuit!", "The administrator role cannot be assigned!", "Die Administrator-Rolle kann nicht zugewiesen werden!"),
            (Terms.CannotUpdateAdminRole, "Rolul de administrator nu poate fi modificat!", "The administrator role cannot be modified!", "Die Administrator-Rolle kann nicht geändert werden!"),
            (Terms.CannotDeleteAdminRole, "Rolul de administrator nu poate fi șters!", "The administrator role cannot be deleted!", "Die Administrator-Rolle kann nicht gelöscht werden!"),
            (Terms.ErrorGettingUser, "Eroare la preluarea contului din mediul de stocare!", "Error getting the user from the storage medium!", "Fehler beim Abrufen des Benutzers vom Speichermedium!"),
            (Terms.ErrorGettingRole, "Eroare la preluarea rolului din mediul de stocare!", "Error getting the role from the storage medium!", "Fehler beim Abrufen der Rolle aus dem Speichermedium!"),
            (Terms.ErrorGettingUserPermissions, "Eroare la preluarea permisiunilor contului din mediul de stocare!", "Error getting the user permissions from the storage medium!", "Fehler beim Abrufen der Benutzerberechtigungen aus dem Speichermedium!"),
            (Terms.ErrorGettingPermissions, "Eroare la preluarea permisiunilor din mediul de stocare!", "Error getting the permissions from the storage medium!", "Fehler beim Abrufen der Berechtigungen aus dem Speichermedium!"),
            (Terms.ErrorInsertingUser, "Eroare la inserarea contului în mediul de stocare!", "Error inserting the account in the storage medium!", "Fehler beim Einfügen des Kontos in das Speichermedium!"),
            (Terms.ErrorGettingRolePermissions, "Eroare la preluarea listei de permisiuni ale rolului din mediul de stocare!", "Error getting the role permissions list from the storage medium!", "Fehler beim Abrufen der Liste der Rollenberechtigungen aus dem Speichermedium!"),
            (Terms.ErrorGettingUserRoles, "Eroare la preluarea listei de roluri de utilizator din mediul de stocare!", "Error getting the user roles list from the storage medium!", "Fehler beim Abrufen der Liste der Benutzerrollen aus dem Speichermedium!"),
            (Terms.ErrorGettingRoles, "Eroare la preluarea listei de roluri din mediul de stocare!", "Error getting the roles list from the storage medium!", "Fehler beim Abrufen der Liste der Rollen aus dem Speichermedium!"),
            (Terms.ErrorInsertingPermission, "Eroare la inserarea permisiunii în mediul de stocare!", "Error inserting the permission in the storage medium!", "Fehler beim Einfügen der Berechtigung in das Speichermedium!"),
            (Terms.ErrorInsertingRole, "Eroare la inserarea rolului în mediul de stocare!", "Error inserting the role in the storage medium!", "Fehler beim Einfügen der Rolle in das Speichermedium!"),
            (Terms.ErrorDeletingRole, "Eroare la ștergerea rolului din mediul de stocare!", "Error deleting the role from the storage medium!", "Fehler beim Löschen der Rolle aus dem Speichermedium!"),
            (Terms.ErrorInitializingDatabase, "Eroare la inițializarea bazei de date!", "Error initializing the database!", "Fehler bei der Initialisierung der Datenbank!"),
            (Terms.NoFirstError, "Prima eroare nu poate fi preluată dintr-un ErrorOr reușit!", "First error cannot be retrieved from a successful ErrorOr!", "Der erste Fehler kann nicht von einem erfolgreichen ErrorOr abgerufen werden!"),
            (Terms.NoErrors, "Lista de erori nu poate fi preluată dintr-un ErrorOr reușit!", "Error list cannot be retrieved from a successful ErrorOr!", "Fehlerliste kann nicht von einem erfolgreichen ErrorOr abgerufen werden!"),
            (Terms.UnverifiedAccount, "Contul dumneavoastră nu a fost încă verificat. Verificați-vă adresa de email pentru a găsi emailul de verificare.", "Your account has not yet been verified. Check your email address to find the verification email.", "Ihr Konto wurde noch nicht verifiziert. Überprüfen Sie Ihre E-Mail-Adresse, um die Bestätigungs-E-Mail zu finden."),
            (Terms.AccountAlreadyVerified, "Contul dumneavoastră este deja verificat.", "Your account has already been verified.", "Ihr Konto wurde bereits verifiziert."),
            (Terms.TokenCannotBeEmpty, "Token-ul nu poate fi gol!", "The token cannot be empty!", "Das Token darf nicht leer sein!"),
            (Terms.TokenIsExpired, "Token-ul de verificare este expirat!", "The verification token is expired!", "Der Bestätigungstoken ist abgelaufen!"),
            (Terms.InvalidPermission, "Nu aveți permisiunile necesare pentru a efectua această acțiune!", "You do not have the necessary permissions to perform this action!", "Sie haben nicht die erforderlichen Berechtigungen, um diese Aktion durchzuführen!"),
            (Terms.ErrorValidatingAccount, "Eroare la activarea contului!", "Error activating the account!", "Fehler bei der Aktivierung des Kontos!"),
            (Terms.ErrorValidatingPasswordReset, "Eroare la verificarea token-ului de resetare a parolei!", "Error verifying the password reset token!", "Fehler bei der Überprüfung des Zurücksetzungs-Token für das Passwort!"),
            (Terms.TokenAlreadyIssued, "Un e-mail de verificare a fost deja trimis către adresa dumneavoastră. Verificați-vă contul de e-mail.", "A verification e-mail has already been sent to your address. Verify your e-mail account.", "Eine Bestätigungs-E-Mail wurde bereits an Ihre E-Mail-Adresse gesendet. Bitte überprüfen Sie Ihr E-Mail-Konto."),
            (Terms.ErrorUpdatingUser, "Eroare la actualizarea contului în mediul de stocare!", "Error updating the user in the storage medium!", "Fehler bei der Aktualisierung des Benutzers im Speichermedium!"),
            (Terms.ErrorUpdatingUserRole, "Eroare la actualizarea rolului utilizatorului în mediul de stocare!", "Error updating the user role in the storage medium!", "Fehler beim Aktualisieren der Benutzerrolle im Speichermedium!"),
            (Terms.ErrorUpdatingUserPermissions, "Eroare la actualizarea permisiunilor utilizatorului în mediul de stocare!", "Error updating the user permissions in the storage medium!", "Fehler beim Aktualisieren der Benutzerberechtigungen im Speichermedium!"),
            (Terms.PasswordReset, "Resetare parolă The Fibre Manager", "Password reset The Fibre Manager", "Passwort zurücksetzen The Fibre Manager"),
            (Terms.PasswordChangeEmail1, "Dragă client,<br/><br/>Am primit o cerere de resetare a parolei pentru contul tău The Fibre Manager. Pentru a continua cu resetarea, te rugăm să dai click pe linkul de mai jos:<br/><br/>", "Dear client,<br/><br/>We received a password reset request for your The Fibre Manager account. To proceed with the reset, please click on the link below:<br/><br/>", "Lieber Kunde,<br/><br/>Wir haben eine Anforderung zum Zurücksetzen des Passworts für Ihr The Fibre Manager Konto erhalten. Um mit dem Zurücksetzen fortzufahren, klicken Sie bitte auf den unten stehenden Link:<br/><br/>"),
            (Terms.PasswordChangeEmail2, "<br/><br/>Dacă nu tu ai inițiat această cerere, poți ignora acest e-mail în întregime. Totuși, dacă ai o bănuială că cineva încearcă să acceseze contul tău, poate ar fi o idee bună să îți schimbi parola cu una mai puternică.<br/><br/>O zi minunată în continuare,<br/>Echipa The Fibre Manager.", "<br/><br/>If you did not initiate this request, you can simply ignore this email. However, if you suspect someone might be trying to access your account, it might be a good idea to change your password to a stronger one.<br/><br/>Have a great day,<br/>The The Fibre Manager Team.", "<br/><br/>Wenn Sie diese Anforderung nicht gestellt haben, können Sie diese E-Mail einfach ignorieren. Sollten Sie jedoch den Verdacht haben, dass jemand versucht, auf Ihr Konto zuzugreifen, wäre es vielleicht eine gute Idee, Ihr Passwort durch ein stärkeres zu ersetzen.<br/><br/>Einen schönen Tag noch,<br/>Das The Fibre Manager Team."),
            (Terms.ClickHereToChangePassword, "Click aici pentru schimbarea parolei", "Click here to change password", "Klicken Sie hier, um Ihr Passwort zu ändern"),
            (Terms.ValueBetweenZeroAndOneHundred, "Valoarea trebuie să fie in intervalul 1-100!", "The value must be between 1 and 100!", "Der Wert muss zwischen 1 und 100 liegen!"),
        };
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Translates a string identified by <paramref name="messageId"/>
    /// </summary>
    /// <param name="messageId">The index of the string to be translated</param>
    /// <returns>The translation of the item identified by <paramref name="messageId"/></returns>
    public string Translate(Terms messageId)
    {
        return translations[messageId][Language];
    }
    #endregion
}