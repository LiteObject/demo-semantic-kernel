using MailKit;
using MailKit.Net.Imap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.AI;

namespace EmailScanner
{
    internal class Program
    {
        private const string GmailSettingsAppPasswordKey = "GmailSettings:AppPassword";
        private const string GmailSettingsUsernameKey = "GmailSettings:Username";
        private const string ImapHost = "imap.gmail.com";
        private const int ImapPort = 993;
        private const bool UseSsl = true;
        private const int FetchCount = 5;

        static async Task Main(string[] args)
        {
            var configuration = LoadConfiguration();
            var (username, apppassword) = GetGmailCredentials(configuration);

            using var ollamaClient = new OllamaChatClient(new Uri("http://localhost:11434/"), "llama3.2:latest");
            using var client = new ImapClient();

            await FetchAndProcessEmails(client, ollamaClient, username, apppassword);
        }

        private static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Program>()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        private static (string username, string apppassword) GetGmailCredentials(IConfiguration configuration)
        {
            string? apppassword = configuration[GmailSettingsAppPasswordKey];
            string? username = configuration[GmailSettingsUsernameKey];

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(apppassword))
            {
                throw new InvalidOperationException("The Gmail username or app password is not configured.");
            }

            return (username, apppassword);
        }

        private static async Task FetchAndProcessEmails(ImapClient client, IChatClient ollamaClient, string username, string apppassword)
        {
            // Connect to the IMAP server
            client.Connect(ImapHost, ImapPort, UseSsl);

            // Authenticate
            client.Authenticate(username, apppassword);

            // Select the inbox folder
            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);

            // Fetch the most recent emails
            for (int i = inbox.Count - 1; i >= inbox.Count - FetchCount && i >= 0; i--)
            {
                var myEmail = inbox.GetMessage(i).ToMyEmail();
                //Console.WriteLine($"Subject: {myEmail.Subject}");
                //Console.WriteLine($"From: {myEmail.From}");
                //Console.WriteLine($"Date: {myEmail.Date}");
                //Console.WriteLine("Body: " + myEmail.Body);

                Console.WriteLine($"Processing -> Subject: {myEmail.Subject}\n");

                ChatOptions options = new() { Temperature = 0.5f };
                string chatMessage = $"Review the following email. Is it a spam? If not, then provide a very short summary.\n\n---\nFrom:{myEmail.From}\nSubject:{myEmail.Subject}\nMessage Body:\n{myEmail.Body}";
                var response = await ollamaClient.CompleteAsync(chatMessage, options);
                myEmail.Note = response.Message.Text;
                Console.WriteLine("Note: " + myEmail.Note);

                Console.WriteLine("----------------------------------------");
            }

            // Disconnect
            client.Disconnect(true);
        }
    }
}
