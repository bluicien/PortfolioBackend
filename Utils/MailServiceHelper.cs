using Resend;
using Microsoft.AspNetCore.DataProtection;
using PortfolioBackend.Config;
using Microsoft.Extensions.Options;
using PortfolioBackend.Models;


namespace PortfolioBackend.Utils;
public class MailServiceHelper
{
    private readonly IDataProtector _protector;
    private readonly AppSettings _settings;

    public MailServiceHelper(IOptions<AppSettings> options, IDataProtectionProvider provider)
    {
        _protector = provider.CreateProtector("FeedbackApproval");
        _settings = options.Value;
    }

    public async Task SendApprovalEmailAsync(Feedback feedback)
    {
        string feedbackId = feedback.FeedbackId.ToString();

        string encryptedToken = _protector.Protect(feedbackId);

        UriBuilder uriBuilder = new(_settings.ServerUrl)
        {
            Path = "/api/feedback/approve",
            Query = $"token={Uri.EscapeDataString(encryptedToken)}"
        };
        string approvalLink = uriBuilder.ToString();

        IResend resend = ResendClient.Create(_settings.EmailServiceApiKey);

        var resp = await resend.EmailSendAsync(new EmailMessage()
        {
            From = _settings.DomainAddress,
            To = _settings.MyEmailAddress,
            Subject = $"Approve Feedback #{feedbackId}",
            HtmlBody = @$"
                <div style='font-family:Arial,sans-serif; max-width:600px; margin:auto; padding:20px; border:1px solid #ddd; border-radius:8px;'>
                    <h2 style='color:#333;'>Feedback Approval #{feedbackId}</h2>
                    <p style='margin-bottom:20px;'>Click the button below to approve this feedback:</p>
                    <a href='{approvalLink}' style='display:inline-block; padding:10px 20px; background-color:#4CAF50; color:white; text-decoration:none; border-radius:4px;'>Approve Feedback</a>
                    <hr style='margin:20px 0; border:none; border-top:1px solid #eee;' />
                    <h3 style='margin-bottom:5px; color:#555;'>{feedback.Username} <span style='font-size:0.9em; color:#999;'>({feedback.UserIpAddress})</span></h3>
                    <p style='white-space:pre-wrap; color:#444;'>{feedback.Message}</p>
                </div>"
        });
    }

    public string DecryptLink(string encryptedToken)
    {
        return _protector.Unprotect(encryptedToken);
    }
}