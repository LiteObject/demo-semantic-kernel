using Microsoft.Extensions.AI;
using OllamaSharp;
using System;

namespace Demo.Microsoft.Extensions.AI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IChatClient client =
                new OllamaChatClient(new Uri("http://localhost:11434/"), "llama3.2:latest");

            var response = await client.CompleteAsync("What is AI? Give me a short answer.");
            Console.WriteLine(response.Message);
        }

        // install package Microsoft.Extensions.AI.Abstractions

        private static IChatClient CreateChatClient(Arguments arguments)
        {
            if (arguments.Provider.Equals("ollama", StringComparison.OrdinalIgnoreCase))
                return new OllamaApiClient(arguments.Uri, arguments.Model);
            else
                return new OpenAIChatClient(new OpenAI.OpenAIClient(arguments.ApiKey), arguments.Model);
        }
    }
}
