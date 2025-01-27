using OllamaSharp;

namespace OllamaClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // set up the client
            var uri = new Uri("http://localhost:11434");
            OllamaApiClient ollama = new(uri);

            // select a model which should be used for further operations
            ollama.SelectedModel = "deepseek-r1:8b";

            var models = await ollama.ListLocalModelsAsync();

            //foreach (var model in models)
            //{
            //    Console.WriteLine(model.Name);
            //}

            var chat = new Chat(ollama);
            while (true)
            {
                var message = Console.ReadLine();
                if (string.IsNullOrEmpty(message))
                {
                    Console.WriteLine("Message cannot be null or empty. Please enter a valid message.");
                    continue;
                }
                await foreach (var answerToken in chat.SendAsync(message))
                {
                    Console.Write(answerToken);
                }
            }
            // messages including their roles and tool calls will automatically be tracked within the chat object
            // and are accessible via the Messages property
        }
    }
}
