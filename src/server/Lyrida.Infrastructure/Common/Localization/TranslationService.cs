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
            (Terms.GetUserEnvironmentsError, "Eroare la preluarea platformelor utilizatorului din mediul de stocare!", "Error getting the user's platforms from the storage medium!", "Fehler beim Abrufen der Plattformen des Benutzers aus dem Speichermedium!"),
            (Terms.UninitializedDatabaseError, "Database not initialized", "Database not initialized", "Database not initialized"), // this MUST be the same for any language!
            (Terms.InvalidTotpCode, "Codul TOTP este invalid!", "TOTP code is invalid!", "Der TOTP-Code ist ungültig!"), 
            (Terms.InsertUserPreferencesError, "Eroare la inserarea preferințelor de utilizator în mediul de stocare!", "Error inserting the user preferences in the storage medium!", "Fehler beim Einfügen der Benutzereinstellungen im Speichermedium!"), 
            (Terms.InsertUserPageError, "Eroare la inserarea paginii utilizatorului în mediul de stocare!", "Error inserting the user page in the storage medium!", "Fehler beim Einfügen der Benutzerseite im Speichermedium!"), 
            (Terms.InsertUserError, "Eroare la inserarea contului în mediul de stocare!", "Error inserting the account in the storage medium!", "Fehler beim Einfügen des Kontos in das Speichermedium!"),
            (Terms.InsertUserEnvironmentError, "Eroare la inserarea platformei utilizatorului în mediul de stocare!", "Error inserting the user's platform in the storage medium!", "Fehler beim Einfügen der Plattform des Benutzers in das Speichermedium!"),
            (Terms.UpdateUserPreferencesError, "Eroare la actualizarea preferințelor de utilizator în mediul de stocare!", "Error updating the user preferences in the storage medium!", "Fehler beim Aktualisieren der Benutzereinstellungen im Speichermedium!"), 
            (Terms.UpdateUserEnvironmentError, "Eroare la actualizarea platformei utilizatorului în mediul de stocare!", "Error updating the user's platform in the storage medium!", "Fehler beim Aktualisieren der Plattform des Benutzers im Speichermedium!"), 
            (Terms.UpdateUserPageError, "Eroare la actualizarea paginilor utilizatorului în mediul de stocare!", "Error updating the user pages in the storage medium!", "Fehler beim Aktualisieren der Benutzerseiten im Speichermedium!"), 
            (Terms.GetUserPreferencesError, "Eroare la preluarea preferințelor de utilizator din mediul de stocare!", "Error getting the user preferences from the storage medium!", "Fehler beim Abrufen der Benutzereinstellungen aus dem Speichermedium!"), 
            (Terms.GetUserPagesError, "Eroare la preluarea paginilor utilizatorului din mediul de stocare!", "Error getting the user pages from the storage medium!", "Fehler beim Abrufen der Benutzerseiten aus dem Speichermedium!"),
            (Terms.GetUserError, "Eroare la preluarea contului din mediul de stocare!", "Error getting the user from the storage medium!", "Fehler beim Abrufen des Benutzers vom Speichermedium!"),
            (Terms.NoThumbnail, "Fișierul nu conține o pictogramă!", "The file does not contain a thumbnail!", "Die Datei enthält kein Vorschaubild!"), 
            (Terms.DuplicateUsernameError, "Numele de utilizator este folosit deja!", "The username is already used!", "Der Benutzername wird bereits verwendet!"),
            (Terms.TotpCodeCannotBeEmpty, "Codul TOTP nu poate fi gol!", "TOTP code cannot be empty!", "Der TOTP-Code darf nicht leer sein!"),
            (Terms.CannotNavigateUpError, "Nu se poate naviga un nivel mai sus!", "Cannot navigate one level up!", "Kann nicht eine Ebene nach oben navigieren!"),
            (Terms.DatabaseSetupError, "Eroare la inițializarea bazei de date!", "Error initializing the database!", "Fehler bei der Initialisierung der Datenbank!"),
            (Terms.InvalidPathError, "Calea nu este validă!", "The path is invalid!", "Der Pfad ist ungültig!"),
            (Terms.DeleteUserError, "Eroare la ștergerea utilizatorului din mediul de stocare!", "Error deleting the user from the storage medium!", "Fehler beim Löschen des Benutzers aus dem Speichermedium!"),
            (Terms.DeleteUserPageError, "Eroare la ștergerea paginii utilizatorului din mediul de stocare!", "Error deleting the user page from the storage medium!", "Fehler beim Löschen der Benutzerseite aus dem Speichermedium!"),
            (Terms.ValueBetweenZeroAndOneHundred, "Valoarea trebuie să fie în intervalul 1-100!", "The value must be between 1 and 100!", "Der Wert muss zwischen 1 und 100 liegen!"),
            (Terms.ValueGreaterThanZero, "Valoarea trebuie să fie mai mare ca 0!", "The value must be greater than 0!", "Der Wert muss größer als 0 sein!"),
            (Terms.NameCannotBeEmptyError, "Numele nu poate fi gol!", "The name cannot be empty!", "Name darf nicht leer sein!"),
            (Terms.PathCannotBeEmpty, "Calea nu poate fi goală!", "The path cannot be empty!", "Der Pfad darf nicht leer sein!"),
            (Terms.DirectoryAlreadyExistsError, "Directorul există deja!", "The directory already exists!", "Der Ordner existiert bereits!"),
            (Terms.FileAlreadyExistsError, "Fișierul există deja!", "The file already exists!", "Die Datei existiert bereits!"),
            (Terms.DirectoryNotFoundError, "Directorul nu există!", "The directory does not exist!", "Der Ordner existiert nicht!"),
            (Terms.FileNotFoundError, "Fișierul nu există!", "The file does not exist!", "Die Datei existiert nicht!"),
            (Terms.DirectoryCopyError, "Eroare la copierea directorului!", "Error copying the directory!", "Fehler beim Kopieren des Ordners!"),
            (Terms.DirectoryMoveError, "Eroare la mutarea directorului!", "Error moving the directory!", "Fehler beim Verschieben des Ordners!"),
            (Terms.FileCopyError, "Eroare la copierea fișierului!", "Error copying the file!", "Fehler beim Kopieren der Datei!"),
            (Terms.FileMoveError, "Eroare la mutarea fișierului!", "Error moving the file!", "Fehler beim Verschieben der Datei!"),
            (Terms.UnauthorizedAccess, "Nu aveți autorizația necesară pentru efectuarea aceastei acțiuni!", "You are not authorized to perform this action!", "Sie sind nicht berechtigt, diese Aktion auszuführen!"),
           
            (Terms.Conflict, "A apărut un conflict!", "You have a conflict!", "Ein Konflikt ist aufgetreten!"),
            (Terms.NotFound, "Resursa nu a putut fi gasită!", "The resource was not found!", "Die Ressource wurde nicht gefunden!"),
            (Terms.BadRequest, "Cerere invalidă!", "Invalid request!", "Ungültige Anfrage!"),
            (Terms.Forbidden, "Accesul la această resursă este interzis!", "Access to this resource is forbidden!", "Der Zugriff auf diese Ressource ist verboten!"),
            (Terms.InternalServerError, "Eroare internă de server!", "Internal server error!", "Interner Serverfehler!"),
            (Terms.ValidationError, "Au survenit una sau mai multe erori de validare!", "One or more validation errors occurred!", "Es ist ein oder mehrere Validierungsfehler aufgetreten!"),
            (Terms.IdMustBePositive, "Id-ul trebuie să fie pozitiv!", "The id must be positive!", "Die ID muss positiv sein!"),
            (Terms.MaximumLength200, "Lungimea nu poate depasi 200 caractere!", "The length cannot exceed 200 characters!", "Länge darf nicht mehr als 200 Zeichen betragen!"),
            (Terms.MaximumLength1000, "Lungimea nu poate depasi 1000 caractere!", "The length cannot exceed 1000 characters!", "Länge darf nicht mehr als 1000 Zeichen betragen!"),
            (Terms.FirstNameCannotBeEmpty, "Prenumele nu poate fi gol!", "First name cannot be empty!", "Vorname darf nicht leer sein!"),
            (Terms.LastNameCannotBeEmpty, "Numele nu poate fi gol!", "Last name cannot be empty!", "Nachname darf nicht leer sein!"),
            (Terms.UsernameCannotBeEmpty, "Numele de utilizator nu poate fi gol!", "Username cannot be empty!", "Der Benutzername darf nicht leer sein!"),
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
            (Terms.CannotDeleteAdminAccount, "Contul de administrator nu poate fi șters!", "The administrator account cannot be deleted!", "Das Administrator-Konto kann nicht gelöscht werden!"),
            (Terms.ErrorInitializingDatabase, "Eroare la inițializarea bazei de date!", "Error initializing the database!", "Fehler bei der Initialisierung der Datenbank!"),
            (Terms.NoFirstError, "Prima eroare nu poate fi preluată dintr-un ErrorOr reușit!", "First error cannot be retrieved from a successful ErrorOr!", "Der erste Fehler kann nicht von einem erfolgreichen ErrorOr abgerufen werden!"),
            (Terms.NoErrors, "Lista de erori nu poate fi preluată dintr-un ErrorOr reușit!", "Error list cannot be retrieved from a successful ErrorOr!", "Fehlerliste kann nicht von einem erfolgreichen ErrorOr abgerufen werden!"),
            (Terms.AccountAlreadyVerified, "Contul dumneavoastră este deja verificat.", "Your account has already been verified.", "Ihr Konto wurde bereits verifiziert."),
            (Terms.TokenCannotBeEmpty, "Token-ul nu poate fi gol!", "The token cannot be empty!", "Das Token darf nicht leer sein!"),
            (Terms.TokenIsExpired, "Token-ul de verificare este expirat!", "The verification token is expired!", "Der Bestätigungstoken ist abgelaufen!"),
            (Terms.InvalidPermission, "Nu aveți permisiunile necesare pentru a efectua această acțiune!", "You do not have the necessary permissions to perform this action!", "Sie haben nicht die erforderlichen Berechtigungen, um diese Aktion durchzuführen!"),
            (Terms.ErrorValidatingAccount, "Eroare la activarea contului!", "Error activating the account!", "Fehler bei der Aktivierung des Kontos!"),
            (Terms.ErrorValidatingPasswordReset, "Eroare la verificarea token-ului de resetare a parolei!", "Error verifying the password reset token!", "Fehler bei der Überprüfung des Zurücksetzungs-Token für das Passwort!"),
            (Terms.ErrorUpdatingUser, "Eroare la actualizarea contului în mediul de stocare!", "Error updating the user in the storage medium!", "Fehler bei der Aktualisierung des Benutzers im Speichermedium!"),
            (Terms.ClickHereToChangePassword, "Click aici pentru schimbarea parolei", "Click here to change password", "Klicken Sie hier, um Ihr Passwort zu ändern"),
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