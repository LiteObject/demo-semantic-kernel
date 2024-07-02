using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;

namespace DemoSemanticKernel
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string openAIApiKey = configuration["OpenAIApiKey"] ??
                throw new InvalidOperationException($"OpenAI API key is missing in the configuration.");

            IKernelBuilder builder = Kernel.CreateBuilder();
            builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));

            builder
                .AddOpenAIChatCompletion("gpt-3.5-turbo", openAIApiKey);                

            Kernel kernel = builder.Build();

            ChatApp chatApp = new(kernel);
            await chatApp.StartAsync();

            //string text1 = @"
            //            1st Law of Thermodynamics - Energy cannot be created or destroyed.
            //            2nd Law of Thermodynamics - For a spontaneous process, the entropy of the universe increases.
            //            3rd Law of Thermodynamics - A perfect crystal at zero Kelvin has zero entropy.";

            //var summarizer = new Summarizer(kernel);
            //FunctionResult result = await summarizer.RunAsync(text1);
            //Console.WriteLine(result);
        }        
    }
}