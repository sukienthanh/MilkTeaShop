using MimeKit;

namespace WebClient.Models
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string To { get; set; }
        public string SmtpServer { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    //public class EmailMessage
    //{
    //    public MailboxAddress To { get; set; }
    //    public string Subject { get; set; }
    //    public string Content { get; set; }
    //    public EmailMessage(string to, string subject, string content)
    //    {
    //        To = new MailboxAddress("Bụi Tea",to);            
    //        Subject = subject;
    //        Content = content;
    //    }
    //}
}
