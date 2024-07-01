using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Threading.Tasks;

namespace DemoSemanticKernel
{
    public class Summarizer
    {
        private readonly Kernel _kernel;

        public Summarizer(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<FunctionResult> RunAsync(string textToSummarize, int maxTokens = 100)
        {
            var prompt = @"{{$input}}

                        One line TLDR with the fewest words.";

            var summarize = _kernel.CreateFunctionFromPrompt(prompt, executionSettings: new OpenAIPromptExecutionSettings { MaxTokens = maxTokens });

            return await _kernel.InvokeAsync(summarize, new() { ["input"] = textToSummarize });            
        }
    }
}


