using MimeKit;

namespace EmailScanner
{
    internal static class Extensions
    {
        public static MyEmail ToMyEmail(this MimeMessage message)
        {
            return new MyEmail
            {
                Subject = message.Subject,
                From = message.From.Mailboxes.FirstOrDefault()?.ToString(),
                Date = message.Date.DateTime,
                Body = message.TextBody,
                Note = null
            };
        }
    }
}
