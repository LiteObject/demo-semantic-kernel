using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSemanticKernel
{
    internal class PermissionFilter : IFunctionInvocationFilter
    {
        public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
        {
            Console.WriteLine($"Allow {context.Function.Name}?");
            if (Console.ReadLine() == "y")
            {
                await next(context);
            }
            else
            {
                throw new Exception("Permission denied.");     
            }
        }
    }
}
