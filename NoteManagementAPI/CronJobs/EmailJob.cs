using Castle.Core.Smtp;
using EmailService;
using IEmailSender = EmailService.IEmailSender;

namespace NoteManagementAPI.EmailSender
{
    public class EmailJob
    {
        private readonly ILogger<EmailJob> _logger;
        private readonly EmailService.IEmailSender _emailSender;

        public EmailJob(
            ILogger<EmailJob> logger,
            IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        public async Task SendEmailAsync()
        {
            var message = new Message(
                to:new string[] { 
                    "datlt.mdc@gmail.com",
                    "DOIXUIHEN@gmail.com"
                }, 
                subject: "Test email", 
                content: "This is the content from our email.");
            _emailSender.SendEmail(message);
        }

        public void SendEmail()
        {
            var message = new Message(
                to: new string[] {
                    "datlt.mdc@gmail.com",
                    "DOIXUIHEN@gmail.com"
                },
                subject: "Test email",
                content: "This is the content from our email.");
            _emailSender.SendEmail(message);
        }
    }
}
