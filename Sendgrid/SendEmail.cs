//using SendGrid;
//using SendGrid.Helpers.Mail;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;

//namespace SabiAsp.Sendgrid
//{
//    public class SendEmail
//    {
        
//        public async Task<string> Email(/*string emailFrom, string emailSubject, string emailBody*/) 
//        {
//            string apiKey = Environment.GetEnvironmentVariable("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");
//            var client = new SendGridClient(apiKey);
//            var from = new EmailAddress("waleedkhan9645@gmail.com", "Example User");
//            var subject = "Sending with Twilio SendGrid is Fun";
//            var to = new EmailAddress("wkhan9645@gmail.com", "Example User");
//            var plainTextContent = "and easy to do anywhere, even with C#";
//            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
//            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
//            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
//            return "Send Email";
//        }

        

//    }
//}