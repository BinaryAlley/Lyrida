#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
#endregion

namespace Lyrida.Infrastructure.Common.Notification;

/// <summary>
/// Interface for sending e-mails
/// </summary>
/// <remarks>
/// Creation Date: 30th of March, 2023
/// </remarks>
public interface IEmailService
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Sends an e-mail message
    /// </summary>
    /// <param name="subject">Optional e-mail subject</param>
    /// <param name="body">The e-mail message</param>
    /// <param name="from">The e-mail address from which the e-mail is sent</param>
    /// <param name="to">The e-mail address to which the e-mail is sent</param>
    /// <param name="bcc">Optional extra reciptients of the e-mail</param>
    Task SendEmailAsync(string? subject, string body, string from, string to, string[]? bcc = null);
    #endregion
}