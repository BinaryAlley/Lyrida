#region ========================================================================= USING =====================================================================================
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Lyrida.Infrastructure.Common.Security;
using Lyrida.Infrastructure.Common.Configuration;
#endregion

namespace Lyrida.Infrastructure.Common.Notification;

/// <summary>
/// Service for sending e-mails
/// </summary>
/// <remarks>
/// Creation Date: 30th of March, 2023
/// </remarks>
public class EmailService : IEmailService
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ISecurity security;
    private readonly IAppConfig appConfig;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="appConfig">Application configuration service</param>
    /// <param name="security">Service for security related tasks</param>
    public EmailService(IAppConfig appConfig, ISecurity security)
    {
        this.security = security;
        this.appConfig = appConfig;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Sends an e-mail message
    /// </summary>
    /// <param name="subject">Optional e-mail subject</param>
    /// <param name="body">The e-mail message</param>
    /// <param name="from">The e-mail address from which the e-mail is sent</param>
    /// <param name="to">The e-mail address to which the e-mail is sent</param>
    /// <param name="bcc">Optional extra reciptients of the e-mail</param>
    public async Task SendEmailAsync(string? subject, string body, string from, string to, string[]? bcc = null)
    {
        SmtpClient smtpServer = new("mail.thefibremanager.com"); 
        smtpServer.Port = 587;
        smtpServer.Credentials = new NetworkCredential(security.CryptographyService.Decrypt(appConfig.EmailServer!["username"]),
            security.CryptographyService.Decrypt(appConfig.EmailServer["password"]));
        MailMessage mail = new();
        mail.From = new MailAddress(from, "The Fibre Manager");
        mail.To.Add(to);
        if (bcc?.Length > 0)
            foreach (var recipient in bcc)
                mail.Bcc.Add(recipient);
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;
        // SSL is disabled because the smtp client is connecting to the email server internally, through the docker network;
        // the email server doesn't use TLS certificates because Nginx Proxy Manager handles SSL certificates
        smtpServer.EnableSsl = false;
        smtpServer.Timeout = 10000;
        await smtpServer.SendMailAsync(mail);
    }
    #endregion
}