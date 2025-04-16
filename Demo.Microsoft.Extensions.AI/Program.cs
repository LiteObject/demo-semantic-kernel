using Microsoft.Extensions.AI;

namespace Demo.Microsoft.Extensions.AI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using IChatClient client =
                new OllamaChatClient(new Uri("http://localhost:11434/"), "llama3.2:latest");

            var response = await client.GetResponseAsync("What is AI? Give me a short answer.");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(response.Text);
            Console.ResetColor();

            // await GeneratedEmbeddings();
        }

        // install package Microsoft.Extensions.AI.Abstractions

        //private static IChatClient CreateChatClient(Arguments arguments)
        //{
        //    if (arguments.Provider.Equals("ollama", StringComparison.OrdinalIgnoreCase))
        //        return new OllamaApiClient(arguments.Uri, arguments.Model);
        //    else
        //        return new OpenAIChatClient(new OpenAI.OpenAIClient(arguments.ApiKey), arguments.Model);
        //}

        private static async Task GeneratedEmbeddings()
        {
            IEmbeddingGenerator<string, Embedding<float>> generator =
                new OllamaEmbeddingGenerator(new Uri("http://localhost:11434/"), "nomic-embed-text:latest");

            var embedding = await generator.GenerateAsync(["What is AI?"]);

            Console.WriteLine(string.Join(", ", embedding[0].Vector.ToArray()));
        }
    }
}
    
