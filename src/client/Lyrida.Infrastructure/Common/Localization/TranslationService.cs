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
            (Terms.CopyButton, "Copiază", "Copy", "Kopieren"),
            (Terms.CopyTitle, "Text copiat!", "Text copied!", "Text kopiert!"),
            (Terms.CopyKeys, "Apasă <i>ctrl</i> sau <i>\u2318</i> + <i>C</i> pentru a copia datele din tabel în clipboard. <br><br>Pentru a anula, faceți clic pe acest mesaj sau apăsați Esc.", "Press <i>ctrl</i> or <i>\u2318</i> + <i>C</i> to copy the table data to your clipboard. <br><br>To cancel, click this message or press Esc.", "Drücken Sie <i>Strg</i> oder <i>\u2318</i> + <i>C</i>, um die Tabellendaten in die Zwischenablage zu kopieren. <br><br>Um abzubrechen, klicken Sie auf diese Nachricht oder drücken Sie Esc."),
            (Terms.CopySuccessMultiple, "%d linii copiate", "%d lines copied", "%d Zeilen kopiert"),
            (Terms.CopySuccessSingle, "1 linie copiată", "1 line copied", "1 Zeile kopiert"),
            (Terms.CustomSearchBuilder, "Constructor Personalizat de Căutare", "Custom Search Builder", "Benutzerdefinierter Such-Builder"),
            (Terms.AddCondition, "Adaugă Condiție", "Add Condition", "Bedingung Hinzufügen"),
            (Terms.FiltersActive, "Filtre Active", "Filters Active", "Aktive Filter"),
            (Terms.CollapseAll, "Restrânge Tot", "Collapse All", "Alle Einklappen"),
            (Terms.ClearAll, "Șterge Tot", "Clear All", "Alles Löschen"),
            (Terms.ShowAll, "Arată Tot", "Show All", "Zeige Alles"),
            (Terms.Active, "Activ", "Active", "Aktiv"),
            (Terms.Inactive, "Inactiv", "Inactive", "Inaktiv"),
            (Terms.IsVerified, "Verificat", "Is Verified", "Ist Verifiziert"),
            (Terms.VerifiedStatus, "Status Verificat", "Verified Status", "Verifizierungsstatus"),
            (Terms.Show, "Arată", "Show", "Zeigen"),
            (Terms.Entries, "Intrări", "Entries", "Einträge"),
            (Terms.Previous, "Anterior", "Previous", "Vorherige"),
            (Terms.Next, "Următor", "Next", "Nächste"),
            (Terms.NoDataAvailable, "Nicio dată disponibilă", "No data available", "Keine Daten verfügbar"),
            (Terms.ShowingStartToEnd, "Arată de la _START_ la _END_ din _TOTAL_ intrări", "Showing _START_ to _END_ of _TOTAL_ entries", "Zeige _START_ bis _END_ von _TOTAL_ Einträgen"),
            (Terms.ShowingZeroEntries, "Arată 0 până la 0 din 0 intrări", "Showing 0 to 0 of 0 entries", "Zeige 0 bis 0 von 0 Einträgen"),
            (Terms.FilteredFromMax, "(filtrat din _MAX_ intrări totale)", "(filtered from _MAX_ total entries)", "(gefiltert von _MAX_ Gesamteinträgen)"),
            (Terms.Search, "Caută", "Search", "Suchen"),
            (Terms.First, "Primul", "First", "Erste"),
            (Terms.Last, "Ultimul", "Last", "Letzte"),
            (Terms.Processing, "Procesare...", "Processing...", "Verarbeitung..."),
            (Terms.EmptyTable, "Nicio dată disponibilă în tabel", "No data available in table", "Keine Daten im Tisch verfügbar"),
            (Terms.LengthMenu, "Arată _MENU_ intrări", "Show _MENU_ entries", "Zeige _MENU_ Einträge"),
            (Terms.LoadingRecords, "Încărcare înregistrări...", "Loading records...", "Ladeaufzeichnungen..."),
            (Terms.ZeroRecords, "Nicio înregistrare potrivită găsită", "No matching records found", "Keine passenden Aufzeichnungen gefunden"),
            (Terms.SortAscending, ": activează pentru a sorta coloana în ordine crescătoare", ": activate to sort column ascending", ": aktivieren, um die Spalte aufsteigend zu sortieren"),
            (Terms.SortDescending, ": activează pentru a sorta coloana în ordine descrescătoare", ": activate to sort column descending", ": aktivieren, um die Spalte absteigend zu sortieren"),
            (Terms.InfoFiltered, "- filtrat din _MAX_ intrări totale", "- filtered from _MAX_ total entries", "- gefiltert von _MAX_ Gesamteinträgen"),
            (Terms.InfoEmpty, "Arată 0 până la 0 din 0 intrări", "Showing 0 to 0 of 0 entries", "Zeige 0 bis 0 von 0 Einträgen"),
            (Terms.Info, "Arată _START_ până la _END_ din _TOTAL_ intrări", "Showing _START_ to _END_ of _TOTAL_ entries", "Zeige _START_ bis _END_ von _TOTAL_ Einträgen"),
            (Terms.ClearAllSearchBuilder, "Șterge tot", "Clear All", "Alles löschen"),
            (Terms.Condition, "Condiție", "Condition", "Bedingung"),
            (Terms.LogicAnd, "Și", "And", "Und"),
            (Terms.LogicOr, "Sau", "Or", "Oder"),
            (Terms.Value, "Valoare", "Value", "Wert"),
            (Terms.Equals, "Egal cu", "Equals", "Gleich"),
            (Terms.Not, "Nu este", "Not", "Nicht"),
            (Terms.StartsWith, "Începe cu", "Starts with", "Beginnt mit"),
            (Terms.Contains, "Conține", "Contains", "Enthält"),
            (Terms.EndsWith, "Se termină cu", "Ends with", "Endet mit"),
            (Terms.LessThan, "Mai mic decât", "Less than", "Weniger als"),
            (Terms.GreaterThan, "Mai mare decât", "Greater than", "Größer als"),
            (Terms.DoesNotStartWith, "Nu începe cu", "Does not start with", "Beginnt nicht mit"),
            (Terms.DoesNotContain, "Nu conține", "Does not contain", "Enthält nicht"),
            (Terms.DoesNotEndWith, "Nu se termină cu", "Does not end with", "Endet nicht mit"),
            (Terms.Empty, "Gol", "Empty", "Leer"),
            (Terms.NotEmpty, "Nu este gol", "Not empty", "Nicht leer"),
            (Terms.After, "După", "After", "Nach"),
            (Terms.Before, "Înainte", "Before", "Vor"),
            (Terms.Between, "Între", "Between", "Zwischen"),
            (Terms.Empty, "Gol", "Empty", "Leer"),
            (Terms.Equals, "Egal", "Equals", "Gleich"),
            (Terms.Not, "Nu", "Not", "Nicht"),
            (Terms.NotBetween, "Nu între", "Not between", "Nicht zwischen"),
            (Terms.GreaterThan, "Mai mare decât", "Greater Than", "Größer als"),
            (Terms.GreaterThanOrEqualTo, "Mai mare sau egal cu", "Greater than or equal to", "Größer oder gleich"),
            (Terms.LessThan, "Mai mic decât", "Less Than", "Weniger als"),
            (Terms.LessThanOrEqualTo, "Mai mic sau egal cu", "Less Than Equal To", "Weniger oder gleich"),
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
            (Terms.Delete, "Șterge", "Delete", "Löschen"),
            (Terms.Edit, "Editează", "Edit", "Bearbeiten"),
            (Terms.Actions, "Acțiuni", "Actions", "Aktionen"),
            (Terms.Selection, "Selectare", "Selection", "Auswahl"),
            (Terms.Created, "Creat", "Created", "Erstellt"),
            (Terms.NotUpdated, "Neactualizat", "Not updated", "Nicht aktualisiert"),
            (Terms.Updated, "Actualizat", "Updated", "Aktualisiert"),
            (Terms.PasswordResetEmailSent, "E-mail-ul de resetare a parolei a fost trimis.", "The password reset e-mail has been sent.", "Die E-Mail zum Zurücksetzen des Passworts wurde gesendet."),
            (Terms.EmptyUsername, "Numele de utilizator nu poate fi gol!", "The username cannot be empty!", "Der Benutzername darf nicht leer sein!"),
            (Terms.EmptyCurrentPassword, "Parola curentă nu poate fi goală!", "The current password cannot be empty!", "Das aktuelle Passwort darf nicht leer sein!"),
            (Terms.EmptyNewPassword, "Parola nouă nu poate fi goală!", "The new password cannot be empty!", "Das neue Passwort darf nicht leer sein!"),
            (Terms.EmptyNewPasswordConfirm, "Confirmarea parolei noi nu poate fi goală!", "The new password confirmation cannot be empty!", "Die Bestätigung des neuen Passworts darf nicht leer sein!"),
            (Terms.EmptyPassword, "Parola nu poate fi goală!", "The password cannot be empty!", "Das Passwort darf nicht leer sein!"),
            (Terms.EmptyEmail, "Email-ul nu poate fi gol!", "The email cannot be empty!", "Die E-Mail darf nicht leer sein!"),
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
            (Terms.ErrorGettingUser, "Eroare la preluarea contului din mediul de stocare!", "Error getting the user from the storage medium!", "Fehler beim Abrufen des Benutzers vom Speichermedium!"),
            (Terms.ErrorInsertingUser, "Eroare la inserarea contului în mediul de stocare!", "Error inserting the account in the storage medium!", "Fehler beim Einfügen des Kontos in das Speichermedium!"),
            (Terms.NoFirstError, "Prima eroare nu poate fi preluată dintr-un ErrorOr reușit!", "First error cannot be retrieved from a successful ErrorOr!", "Der erste Fehler kann nicht von einem erfolgreichen ErrorOr abgerufen werden!"),
            (Terms.NoErrors, "Lista de erori nu poate fi preluată dintr-un ErrorOr reușit!", "Error list cannot be retrieved from a successful ErrorOr!", "Fehlerliste kann nicht von einem erfolgreichen ErrorOr abgerufen werden!"),
            (Terms.AccountAlreadyRegistered, "Adresa de e-mail este folosită deja!", "The e-mail address is already used!", "Die E-Mail-Adresse wird bereits verwendet!"),
            (Terms.RegistrationEmailSent, "E-mail-ul de confirmare a contului a fost trimis.", "The account verification e-mail has been sent.", "Die Bestätigungs-E-Mail für Ihr Konto wurde gesendet."),
            (Terms.AccountCreated, "Contul a fost creat.", "The account has been created.", "Das Konto wurde erstellt."),
            (Terms.EmailVerification, "Verificare E-mail", "E-mail Verification", "E-Mail-Bestätigung"),
            (Terms.SessionExpired, "Sesiunea a expirat.", "Session expired.", "Die Sitzung ist abgelaufen."),
            (Terms.SelectARoleOrProvideName, "Selectati un rol existent, sau introduceti numele unui rol nou!", "Select an existing role, or enter the name of a new role!", "Wählen Sie eine bestehende Rolle aus oder geben Sie den Namen einer neuen Rolle ein!"),
            (Terms.BackToLogin, "Înapoi La Autentificare", "Back To Login", "Zurück Zur Anmeldung"),
            (Terms.ResetPassword, "Resetare Parolă", "Reset Password", "Passwort Zurücksetzen"),
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