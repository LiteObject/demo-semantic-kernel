using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Net;

namespace DemoSemanticKernel
{
    public class ChatApp
    {
        private readonly Kernel _kernel;

        public ChatApp(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task StartAsync()
        {         
            var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

            // Add a plugin (the LightsPlugin class is defined below)
            // _kernel.Plugins.AddFromType<LightsPlugin>("Lights");
            _kernel.ImportPluginFromType<ContactInfo>();

            // Enable planning
            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            // Create a history store the conversation
            var history = new ChatHistory();

            // Initiate a back-and-forth chat
            string? userInput;
            do
            {
                // Collect user input
                Console.Write("User > ");
                userInput = Console.ReadLine();

                // Add user input
                history.AddUserMessage(userInput);

                // Get the response from the AI
                var result = await chatCompletionService.GetChatMessageContentAsync(
                    history,
                    executionSettings: openAIPromptExecutionSettings,
                    kernel: _kernel);

                // Print the results
                Console.WriteLine("Assistant > " + result);

                // Add the message from the agent to the chat history
                history.AddMessage(result.Role, result.Content ?? string.Empty);
            } while (userInput is not null);
        }

        class ContactInfo
        {
            [KernelFunction]
            public string GetEmailAddress(string name)
            {
                return name.ToLower() switch
                {
                    "mohammed" => "mohammed@email.com",
                    _ => "Email address is not available"
                };
            }
        }
    }
}
