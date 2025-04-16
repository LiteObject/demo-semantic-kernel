using OllamaSharp;

namespace OllamaClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // set up the client
            var uri = new Uri("http://localhost:11434");
            OllamaApiClient ollama = new(uri)
            {
                // select a model which should be used for further operations
                SelectedModel = "llama3.2:latest"
            };

            IEnumerable<OllamaSharp.Models.Model> models = await ollama.ListLocalModelsAsync();

            //foreach (var model in models)
            //{
            //    Console.WriteLine(model.Name);
            //}

            var chat = new Chat(ollama);
            while (true)
            {
                // prompt the user for input
                Console.Write("User: ");

                var message = Console.ReadLine();
                
                if (string.IsNullOrEmpty(message))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Message cannot be null or empty. Please enter a valid message.");
                    Console.ResetColor();
                    continue;
                }

                // check for exit command
                if (message.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                    message.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                Console.Write("AI: ");
                await foreach (var answerToken in chat.SendAsync(message))
                {
                    Console.Write(answerToken);
                }

                Console.WriteLine();
            }
            // messages including their roles and tool calls will automatically be tracked within the chat object
            // and are accessible via the Messages property
        }
    }
}
