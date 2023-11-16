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
            (Terms.Back, "Înapoi", "Back", "Zurück"),
            (Terms.Forward, "Înainte", "Forward", "Vorwärts"),
            (Terms.UpOneLevel, "Un nivel mai sus", "Up one level", "Eine Ebene höher"),
            (Terms.Deleted, "Șters", "Deleted", "Gelöscht"),
            (Terms.Copied, "Copiat", "Copied", "Kopiert"),
            (Terms.Moved, "Mutat", "Moved", "Verschoben"),
            (Terms.Cut, "Decupează", "Cut", "Ausschneiden"),
            (Terms.Copy, "Copiază", "Copy", "Kopieren"),
            (Terms.Paste, "Lipește", "Paste", "Einfügen"),
            (Terms.Delete, "Șterge", "Delete", "Löschen"),
            (Terms.Rename, "Redenumește", "Rename", "Umbenennen"),
            (Terms.NewDirectory, "Director nou", "New directory", "Neuer Ordner"),
            (Terms.NewTestFile, "Fișier text nou", "New text file", "Neue Textdatei"),
            (Terms.CopyTitle, "Text copiat!", "Text copied!", "Text kopiert!"),
            (Terms.Search, "Caută", "Search", "Suchen"),
            (Terms.SortAscending, ": activează pentru a sorta coloana în ordine crescătoare", ": activate to sort column ascending", ": aktivieren, um die Spalte aufsteigend zu sortieren"),
            (Terms.SortDescending, ": activează pentru a sorta coloana în ordine descrescătoare", ": activate to sort column descending", ": aktivieren, um die Spalte absteigend zu sortieren"),
            (Terms.RegisterAdminAccount, "Înregistrare cont administrator", "Admin account registration", "Administrator-Konto registrieren"),
            (Terms.RoleDeleted, "Rolul a fost șters!", "The role has been deleted!", "Die Rolle wurde gelöscht!"),
            (Terms.RoleCreated, "Rolul a fost creat!", "The role has been created!", "Die Rolle wurde erstellt!"),
            (Terms.RoleUpdated, "Rolul a fost actualizat!", "The role has been updated!", "Die Rolle wurde aktualisiert!"),
            (Terms.PermissionsUpdated, "Permisiunile au fost actualizate!", "Permissions have been updated!", "Die Berechtigungen wurden aktualisiert!"),
            (Terms.NotificationSuccess, "Succes", "Success", "Erfolg"),
            (Terms.NotificationError, "Eroare", "Error", "Fehler"),
            (Terms.NotificationWarning, "Avertizare", "Warning", "Warnung"),
            (Terms.NotificationHelp, "Ajutor", "Help", "Hilfe"),
            (Terms.NotificationConfirmation, "Confirmare", "Confirmation", "Bestätigung"),
            (Terms.CanViewUsers, "Poate vedea utilizatori", "Can view users", "Kann Benutzer anzeigen"),
            (Terms.CanViewPermissions, "Poate vedea roluri și permisiuni", "Can view roles and permissions", "Kann Rollen und Berechtigungen anzeigen"),
            (Terms.CanViewSettings, "Poate vedea setări", "Can view settings", "Kann Einstellungen anzeigen"),
            (Terms.CanEditUsers, "Poate edita utilizatori", "Can edit users", "Kann Benutzer bearbeiten"),
            (Terms.Roles, "Roluri", "Roles", "Rollen"),
            (Terms.RoleName, "Nume rol", "Role name", "Rollenname"),
            (Terms.SelectARole, "Selectați un rol", "Select a role", "Wählen Sie eine Rolle aus"),
            (Terms.SelectAUser, "Selectați un utilizator", "Select a user", "Wählen Sie einen Benutzer aus"),
            (Terms.Confirm, "Confirmare", "Confirm", "Bestätigung"),
            (Terms.CreateRole, "Creare Rol", "Create Role", "Rolle Erstellen"),
            (Terms.UpdateRole, "Actualizare Rol", "Update Role", "Rolle Aktualisieren"),
            (Terms.Update, "Actualizare", "Update", "Aktualisierung"),
            (Terms.Cancel, "Anulare", "Cancel", "Abbrechen"),
            (Terms.Confirmation, "Confirmare", "Confirmation", "Bestätigung"),
            (Terms.ConfirmationMessage, "Sunteți sigur?", "Are you sure?", "Sind Sie sicher?"),
            (Terms.No, "Nu", "No", "Nein"),
            (Terms.Yes, "Da", "Yes", "Ja"),
            (Terms.Edit, "Editează", "Edit", "Bearbeiten"),
            (Terms.Actions, "Acțiuni", "Actions", "Aktionen"),
            (Terms.Selection, "Selectare", "Selection", "Auswahl"),
            (Terms.Created, "Creat", "Created", "Erstellt"),
            (Terms.NotUpdated, "Neactualizat", "Not updated", "Nicht aktualisiert"),
            (Terms.Updated, "Actualizat", "Updated", "Aktualisiert"),
            (Terms.PasswordTemporarilyReset, "Parola a fost setată temporar la Abcd123$. Schimbați-o cu una mai securizată, cât mai curând posibil!", "The password has been temporarily set to Abcd123$. Change it to a more secured one, as soon as possible!", "Das Passwort wurde vorübergehend auf Abcd123$ festgelegt. Ändern Sie es so bald wie möglich in ein sichereres Passwort!"),
            (Terms.EmptyUsername, "Numele de utilizator nu poate fi gol!", "The username cannot be empty!", "Der Benutzername darf nicht leer sein!"),
            (Terms.EmptyCurrentPassword, "Parola curentă nu poate fi goală!", "The current password cannot be empty!", "Das aktuelle Passwort darf nicht leer sein!"),
            (Terms.EmptyNewPassword, "Parola nouă nu poate fi goală!", "The new password cannot be empty!", "Das neue Passwort darf nicht leer sein!"),
            (Terms.EmptyNewPasswordConfirm, "Confirmarea parolei noi nu poate fi goală!", "The new password confirmation cannot be empty!", "Die Bestätigung des neuen Passworts darf nicht leer sein!"),
            (Terms.EmptyPassword, "Parola nu poate fi goală!", "The password cannot be empty!", "Das Passwort darf nicht leer sein!"),
            (Terms.EmptyEmail, "Email-ul nu poate fi gol!", "The email cannot be empty!", "Die E-Mail darf nicht leer sein!"),
            (Terms.EmptyTotp, "Codul TOTP nu poate fi gol!", "The TOTP code cannot be empty!", "Der TOTP-Code darf nicht leer sein!"),
            (Terms.EmptyLastName, "Numele nu poate fi gol!", "The name cannot be empty!", "Der Nachname darf nicht leer sein!"),
            (Terms.EmptyFirstName, "Prenumele nu poate fi gol!", "The surname cannot be empty!", "Der Vorname darf nicht leer sein!"),
            (Terms.TheTokenIsEmpty, "Token-ul de verificare nu poate fi gol!", "The verification token cannot be empty!", "Der Bestätigungstoken darf nicht leer sein!"),
            (Terms.TokenIsExpired, "Token-ul de verificare este expirat!", "The verification token is expired!", "Der Bestätigungstoken ist abgelaufen!"),
            (Terms.TokenAlreadyIssued, "Un e-mail de verificare a fost deja trimis către adresa dumneavoastră. Verificați-vă contul de e-mail.", "A verification e-mail has already been sent to your address. Verify your e-mail account.", "Eine Bestätigungs-E-Mail wurde bereits an Ihre E-Mail-Adresse gesendet. Bitte überprüfen Sie Ihr E-Mail-Konto."),
            (Terms.ErrorValidatingAccount, "Eroare la activarea contului!", "Error activating the account!", "Fehler bei der Aktivierung des Kontos!"),
            (Terms.UnverifiedAccount, "Contul dumneavoastră nu a fost încă verificat. Verificați-vă adresa de email pentru a găsi emailul de verificare.", "Your account has not yet been verified. Check your email address to find the verification email.", "Ihr Konto wurde noch nicht verifiziert. Überprüfen Sie Ihre E-Mail-Adresse, um die Bestätigungs-E-Mail zu finden."),
            (Terms.EmailVerified, "Contul dumneavoastră a fost verificat. Bun venit!", "Your account has been verified. Welcome!", "Ihr Konto wurde verifiziert. Herzlich willkommen!"),
            (Terms.EnterEmail, "Introduceți adresa de e-mail: ", "Enter your e-mail address: ", "Geben Sie Ihre E-Mail-Adresse ein:"),
            (Terms.FirstName, "Prenume", "Surname", "Vorname"),
            (Terms.LastName, "Nume", "Name", "Nachname"),
            (Terms.Login, "Autentificare", "Login", "Anmelden"),
            (Terms.Account, "Cont", "Account", "Konto"),
            (Terms.MyAccount, "Contul meu", "My account", "Mein Konto"),
            (Terms.Logout, "Deconectare", "Logout", "Abmelden"),
            (Terms.Registration, "Înregistrare", "Register", "Registrieren"),
            (Terms.Username, "Nume utilizator", "Username", "Benutzername"),
            (Terms.Permissions, "Permisiuni", "Permissions", "Berechtigungen"),
            (Terms.ManagePermissions, "Administrare Permisiuni", "Manage Permissions", "Berechtigungen verwalten"),
            (Terms.History, "Istoric", "History", "Geschichte"),
            (Terms.AssignedBy, "Atribuit De", "Assigned By", "Zugewiesen Von"),
            (Terms.Settings, "Setări", "Settings", "Einstellungen"),
            (Terms.Users, "Utilizatori", "Users", "Benutzer"),
            (Terms.Password, "Parolă", "Password", "Passwort"),
            (Terms.RecoverPassword, "Recuperare Parolă", "Recover Password", "Passwort Wiederherstellen"),
            (Terms.PasswordConfirm, "Confirmare Parolă", "Password Confirm", "Passwort Bestätigen"),
            (Terms.ReadIn, "Citește în:", "Read in:", "Einlesen in:"),
            (Terms.Welcome, "Bun venit, ", "Welcome, ", "Willkommen, "),
            (Terms.Conflict, "A apărut un conflict!", "You have a conflict!", "Ein Konflikt ist aufgetreten!"),
            (Terms.NotFound, "Resursa nu a putut fi gasită!", "The resource was not found!", "Die Ressource wurde nicht gefunden!"),
            (Terms.BadRequest, "Cerere invalidă!", "Invalid request!", "Ungültige Anfrage!"),
            (Terms.Unauthorized, "Nu aveți autorizația necesară pentru efectuarea aceastei acțiuni!", "You are not authorized to perform this action!", "Sie sind nicht berechtigt, diese Aktion auszuführen!"),
            (Terms.Forbidden, "Accesul la această resursă este interzis!", "Access to this resource is forbidden!", "Der Zugriff auf diese Ressource ist verboten!"),
            (Terms.InternalServerError, "Eroare internă de server!", "Internal server error!", "Interner Serverfehler!"),
            (Terms.ValidationError, "Au survenit una sau mai multe erori de validare!", "One or more validation errors occurred!", "Es ist ein oder mehrere Validierungsfehler aufgetreten!"),
            (Terms.NameCannotBeEmpty, "Numele nu poate fi gol!", "The name cannot be empty!", "Name darf nicht leer sein!"),
            (Terms.MaximumLength200, "Lungimea nu poate depasi 200 caractere!", "The length cannot exceed 200 characters!", "Länge darf nicht mehr als 200 Zeichen betragen!"),
            (Terms.MaximumLength1000, "Lungimea nu poate depasi 1000 caractere!", "The length cannot exceed 1000 characters!", "Länge darf nicht mehr als 1000 Zeichen betragen!"),
            (Terms.FirstNameCannotBeEmpty, "Prenumele nu poate fi gol!", "First name cannot be empty!", "Vorname darf nicht leer sein!"),
            (Terms.LastNameCannotBeEmpty, "Numele nu poate fi gol!", "Last name cannot be empty!", "Nachname darf nicht leer sein!"),
            (Terms.EmailCannotBeEmpty, "Adresa de e-mail nu poate fi goală!", "E-mail cannot be empty!", "E-Mail darf nicht leer sein!"),
            (Terms.PasswordCannotBeEmpty, "Parola nu poate fi goală!", "Password cannot be empty!", "Passwort darf nicht leer sein!"),
            (Terms.RoleNameCannotBeEmpty, "Numele rolului nu poate fi gol!", "Role name cannot be empty!", "Der Rollenname darf nicht leer sein!"),
            (Terms.RoleMustHavePermissions, "Rolul trebuie să aibă cel puțin o permisiune!", "The role must have at least one permission!", "Die Rolle muss mindestens eine Berechtigung haben!"),
            (Terms.EmptyPasswordConfirm, "Confirmarea parolei nu poate fi goală!", "The password confirm cannot be empty!", "Die Passwortbestätigung darf nicht leer sein!"),
            (Terms.PasswordsNotMatch, "Parola și confirmarea parolei nu sunt identice!", "Password and password confirm are not identical!", "Passwort und Passwortbestätigung stimmen nicht überein!"),
            (Terms.InvalidUsername, "Nume de utilizator sau parola invalidă!", "Invalid username or password!", "Ungültiger Benutzername oder Passwort!"),
            (Terms.ReplaceTheFile, "Înlocuiește fișierul destinație", "Replace the file in the destination", "Die Datei im Ziel ersetzen"),
            (Terms.ReplaceAllFiles, "Înlocuiește toate fișierele", "Replace all files", "Alle Dateien ersetzen"),
            (Terms.SkipThisFile, "Sări peste acest fișier", "Skip this file", "Diese Datei überspringen"),
            (Terms.KeepBothFiles, "Păstrează ambele fișiere", "Keep both files", "Beide Dateien behalten"),
            (Terms.InvalidPath, "Calea este invalidă!", "The path is invalid!", "Der Pfad ist ungültig!"),
            (Terms.DirectoryAlreadyExistsError, "Directorul există deja!", "The directory already exists!", "Der Ordner existiert bereits!"),
            (Terms.FileAlreadyExistsError, "Fișierul există deja!", "The file already exists!", "Die Datei existiert bereits!"),
            (Terms.ChooseAnAction, "Alegeți o acțiune:", "Chose an action:", "Wählen Sie eine Aktion:"),
            (Terms.ErrorGettingUser, "Eroare la preluarea contului din mediul de stocare!", "Error getting the user from the storage medium!", "Fehler beim Abrufen des Benutzers vom Speichermedium!"),
            (Terms.ErrorInsertingUser, "Eroare la inserarea contului în mediul de stocare!", "Error inserting the account in the storage medium!", "Fehler beim Einfügen des Kontos in das Speichermedium!"),
            (Terms.NoFirstError, "Prima eroare nu poate fi preluată dintr-un ErrorOr reușit!", "First error cannot be retrieved from a successful ErrorOr!", "Der erste Fehler kann nicht von einem erfolgreichen ErrorOr abgerufen werden!"),
            (Terms.NoErrors, "Lista de erori nu poate fi preluată dintr-un ErrorOr reușit!", "Error list cannot be retrieved from a successful ErrorOr!", "Fehlerliste kann nicht von einem erfolgreichen ErrorOr abgerufen werden!"),
            (Terms.AccountAlreadyRegistered, "Adresa de e-mail este folosită deja!", "The e-mail address is already used!", "Die E-Mail-Adresse wird bereits verwendet!"),
            (Terms.AccountCreated, "Contul a fost creat.", "The account has been created.", "Das Konto wurde erstellt."),
            (Terms.EmailVerification, "Verificare E-mail", "E-mail Verification", "E-Mail-Bestätigung"),
            (Terms.SessionExpired, "Sesiunea a expirat.", "Session expired.", "Die Sitzung ist abgelaufen."),
            (Terms.SelectARoleOrProvideName, "Selectati un rol existent, sau introduceti numele unui rol nou!", "Select an existing role, or enter the name of a new role!", "Wählen Sie eine bestehende Rolle aus oder geben Sie den Namen einer neuen Rolle ein!"),
            (Terms.BackToLogin, "Înapoi La Autentificare", "Back To Login", "Zurück Zur Anmeldung"),
            (Terms.PasswordReset, "Resetare parolă", "Password reset", "Passwort zurücksetzen"),
            (Terms.PasswordHasBeenReset, "Parola a fost resetată!", "The password has been reset!", "Ihr Passwort wurde zurückgesetzt!"),
            (Terms.PasswordChanged, "Parola a fost schimbată!", "The password has been changed!", "Das Passwort wurde geändert!"),
            (Terms.ChangePassword, "Schimbare parolă", "Change password", "Passwort ändern"),
            (Terms.CurrentPassword, "Parolă curentă", "Current password", "Aktuelles Passwort"),
            (Terms.NewPassword, "Parolă nouă", "New password", "Neues Passwort"),
            (Terms.NewPasswordConfirm, "Confirmare parolă nouă", "New password confirm", "Neues Passwort bestätigen"),
            (Terms.AuthenticationRequired, "Pentru a avea acces la această secțiune, trebuie să fiți autentificați.", "In order to have access to this section, you must be authenticated.", "Um Zugang zu diesem Bereich zu haben, müssen Sie authentifiziert sein."),
            (Terms.UsersList, "Listă Utilizatori", "Users List", "Benutzerliste"),
            (Terms.AddUser, "Adăugare Utilizator", "Add User", "Benutzer Hinzufügen"),
            (Terms.ManageRoles, "Administrare Roluri", "Manage Roles", "Rollen Verwalten"),
            (Terms.CreateAdminAccount, "Creare cont administrator", "Create administrator account", "Administrator-Konto erstellen"),
            (Terms.ConfirmUserDelete, "Sunteți sigur ca doriți ștergerea utilizatorului?", "Are you sure you want to delete the user?", "Sind Sie sicher, dass Sie den Benutzer löschen möchten?"),
            (Terms.ConfirmRoleDelete, "Sunteți sigur ca doriți ștergerea rolului?", "Are you sure you want to delete the role?", "Sind Sie sicher, dass Sie die Rolle löschen möchten?"),
            (Terms.UserDeleted, "Utilizator șters!", "User deleted!", "Benutzer gelöscht!"),
            (Terms.Name, "Nume", "Name", "Name"),
            (Terms.Description, "Descriere", "Description", "Beschreibung"),
            (Terms.DeleteRole, "Ștergere Rol", "Delete Role", "Rolle Löschen"),
            (Terms.DirectoryType, "Director", "File folder", "Aktenordner"),
            (Terms.Enable2Fa, "Activează autentificarea în doi pași", "Enable two factor authentication", "Aktiviere die Zwei-Faktor-Authentifizierung"),
            (Terms.Disabled2Fa, "Recuperarea parolelor este permisă doar dacă autentificarea în doi pași este activată!", "Password recovery is allowed only if two factor authentication is enabled!", "Die Wiederherstellung von Passwörtern ist nur möglich, wenn die Zwei-Faktor-Authentifizierung aktiviert ist!"),
            (Terms.ScanTotpCode, "Scanați codul QR cu o aplicație TOTP (ex: Authy, Google Authenticator, Aegis Authenticator, etc)", "Scan the QR code with a TOTP application (ie: Authy, Google Authenticator, Aegis Authenticator, etc)", "Scanne den QR-Code mit einer TOTP-Anwendung (z. B. Authy, Google Authenticator, Aegis Authenticator, usw.)"),
            (Terms.EnterTotpCode, "Cod TOTP", "TOTP code", "TOTP-Code"),
            (Terms.ValidateCode, "Validare cod", "Validate code", "Code validiert"),
            (Terms.ShowImagePreviews, "Afișează previzualizări imagini", "Show image previews", "Zeige Bildvorschauen"),
            (Terms.InspectFileForThumbnails, "Verifică tipul de fișier pentru previzualizări", "Check file type for thumbnails", "Überprüfe den Dateityp für Miniaturansichten"),
            (Terms.ImagePreviewsQuality, "Calitate previzualizare imagini", "Image previews quality", "Qualität der Bildvorschauen"),
            (Terms.FullImageQuality, "Calitate imagini întreagă", "Full image quality", "Volle Bildqualität"),
            (Terms.ScrollThumbnailRetrievalTimeout, "Timp pentru preluarea previzualizărilor la derulare", "Scroll thumbnails retrieval timeout", "Zeitüberschreitung bei der Abrufung der Miniaturansichten beim Scrollen."),
            (Terms.ThumbnailsRetrievalBatchSize, "Numărul de previzualizări de preluat concomitent", "Number of thumbnails to get concurrently", "Anzahl der gleichzeitig abzurufenden Miniaturansichten"),
            (Terms.RememberOpenTabs, "Memorează pagini deschise", "Remember open tabs", "Merke offene Registerkarten"),
            (Terms.EnableConsoleDebugMessages, "Activează afișarea mesajelor de depanare în consola de dezvoltator", "Enable debug messages in developer console", "Aktiviere Debug-Nachrichten in der Entwicklerkonsole"),
            (Terms.DescriptionEnable2Fa, "Activează autentificarea în doi pași folosind o aplicație TOTP, precum Authy, Aegis Authenticator, Google Authenticator, etc. Singurul mod de a recupera parola, în cazul în care o pierzi.", "Enable two-factor authentication using a TOTP app, such as Authy, Aegis Authenticator, Google Authenticator, etc. The only way to recover your password, if lost.", "Zwei-Faktor-Authentifizierung mit einer TOTP-App, wie Authy, Aegis Authenticator, Google Authenticator, etc., aktivieren. Der einzige Weg, Ihr Passwort, bei Verlust, wiederherzustellen."),
            (Terms.DescriptionRememberOpenTabs, "Salvează și restaurează filele deschise când repornești aplicația.", "Save and restore your open tabs when relaunching the application.", "Speichern und Wiederherstellen Ihrer geöffneten Tabs beim erneuten Starten der Anwendung."),
            (Terms.DescriptionShowImagePreviews, "Afișează previzualizări ale imaginilor, în loc de pictogramele implicite pentru fișiere.", "Display image thumbnails, instead of default icons for files.", "Bildvorschauen anstelle von Standard-Symbolen für Dateien anzeigen."),
            (Terms.DescriptionFullImageQuality, "Calitatea imaginii când este văzută la dimensiune completă. Ajustează ca procentaj.", "Image quality when viewing at full size. Adjust as a percentage.", "Bildqualität beim Betrachten in voller Größe. Als Prozentsatz anpassen."),
            (Terms.DescriptionScrollThumbnailRetrievalTimeout, "Intervalul de timp trecut de la ultima derulare, înainte de a prelua previzualizări.", "The time span since the last scroll, before getting thumbnails.", "Die Zeitspanne seit dem letzten Scrollen, bevor Miniaturansichten abgerufen werden."),
            (Terms.DescriptionThumbnailsRetrievalBatchSize, "Numărul de previzualizări ce va fi cerut la un moment dat de la server.", "The number of thumbnails to ask from the server at a given time.", "Die Anzahl der Miniaturansichten, die zu einem bestimmten Zeitpunkt vom Server angefordert werden sollen."),
            (Terms.DescriptionImagePreviewsQuality, "Calitatea imaginii pentru miniaturi. Ajustează ca procentaj.", "Image quality for thumbnails. Adjust as a percentage.", "Bildqualität für Vorschaubilder. Als Prozentsatz anpassen."),
            (Terms.DescriptionInspectFileForThumbnails, "Încearcă să afișezi previzualizări pentru toate tipurile de fișiere, verificând biții fișierului, nu extensia acestuia (mai încet, dar mai precis).", "Attempt to display thumbnails for all file types, by inspecting the file's bytes, not its extension (slower, but more accurate).", "Versuche, Miniaturansichten für alle Dateitypen anzuzeigen, indem die Bytes der Datei überprüft werden, nicht die Dateiendung (langsamer, aber genauer)."),
            (Terms.DescriptionEnableConsoleDebugMessages, "Afișează mesaje de depanare în consola de dezvoltator ce apare la apăsarea tastei F12, în browser.", "Display debug messages in the developer console that appears when pressing the F12 key in browser.", "Zeige Debug-Nachrichten in der Entwicklerkonsole an, die erscheint, wenn die F12-Taste im Browser gedrückt wird."),
            (Terms.ValueBetweenZeroAndOneHundred, "Valoarea trebuie să fie in intervalul 1-100!", "The value must be between 1 and 100!", "Der Wert muss zwischen 1 und 100 liegen!"),
            (Terms.ValueGreaterThanZero, "Valoarea trebuie să fie mai mare ca 0!", "The value must be greater than 0!", "Der Wert muss größer als 0 sein!"),
            (Terms.SelectionMode, "Mod selecție", "Selection mode", "Auswahlmodus"),
            (Terms.YouNeedToWriteSomething, "Trebuie să scrieți ceva!", "You need to write something!", "Du musst etwas schreiben!"),
            (Terms.YouMustTypeDelete, "Trebuie să tastați \"Delete\"!", "You must type \"Delete\"!", "Du musst \"Delete\" eingeben!"),
            (Terms.PleaseTypeDelete, "Tastați \"Delete\" în caseta de dialog de mai jos, pentru confirmare:", "Type \"Delete\" in the input dialog below, for confirmation:", "Tasten Sie \"Delete\" in das unten stehende Dialogfeld zur Bestätigung ein:"),
            (Terms.EnterNewDirectoryName, "Introduceți numele noului director:", "Enter the new directory name:", "Geben Sie den neuen Ordnernamen ein:"),
            (Terms.NavigateToALocationFirst, "Navigați la o locație mai întâi!", "Navigate to a location first!", "Zuerst zu einem Speicherort navigieren!"),
            (Terms.NoFilesSelected, "Nici un fișier sau director selectat!", "No file or directory selected!", "Keine Datei oder Ordner ausgewählt!"),
            (Terms.NoFilesCopied, "Nici un fișier sau director decupat sau copiat!", "No file or directory cut or copied!", "Keine Datei oder Ordner ausgeschnitten oder kopiert!"),
            (Terms.NormalMode, "Mod normal", "Normal mode", "Normalmodus"),
            (Terms.SelectAll, "Selectează tot", "Select all", "Alles auswählen"),
            (Terms.SelectNone, "Deselectează tot", "Select none", "Nichts auswählen"),
            (Terms.SelectInverse, "Inversează selecție", "Invert selection", "Auswahl umkehren"),
            (Terms.SelectAPlatform, "Selectați o platformă", "Select a platform", "Plattform auswählen"),
            (Terms.DirectoryCreated, "Directorul a fost creat!", "Directory has been created!", "Der Ordner wurde erstellt!"),
            (Terms.FileCreated, "Fișierul a fost creat!", "File has been created!", "Die Datei wurde erstellt!"),
            (Terms.FileRenamed, "Fișierul a fost redenumit!", "File has been renamed!", "Die Datei wurde umbenannt!"),
            (Terms.DirectoryRenamed, "Directorul a fost redenumit!", "Directory has been renamed!", "Der Ordner wurde umbenannt!"),
            (Terms.CopyHere, "Copiază aici", "Copy here", "Hierhin kopieren"),
            (Terms.MoveHere, "Mută aici", "Move here", "Hierhin verschieben"),
            (Terms.LinkHere, "Leagă aici", "Link here", "Hier verlinken"),
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