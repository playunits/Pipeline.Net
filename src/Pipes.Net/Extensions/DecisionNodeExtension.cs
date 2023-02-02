using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pipes.Net.Nodes;

namespace Pipes.Net.Extensions
{
    public static class DecisionNodeExtension
    {
        public static PipelineFactory AddDecision<T>(this PipelineFactory factory, Func<T, bool?> determination, Action<PipelineFactory> success, Action<PipelineFactory> failure)
        {
            var successBuilder = new PipelineFactory();
            success(successBuilder);

            var failureBuilder = new PipelineFactory();
            failure(failureBuilder);

            return factory.AddDecision(determination, successBuilder.Build(), failureBuilder.Build());
        }

        public static PipelineFactory AddDecision<T>(this PipelineFactory factory, Func<T, bool?> determination, Pipeline success, Pipeline failure)
        {
            return factory.AddDecision(determination, success.StartElement, failure.StartElement);
        }

        public static PipelineFactory AddDecision<T>(this PipelineFactory factory, Func<T, bool?> determination, INode? success, INode? failure)
        {
            return factory.Add(DecisionNode.Create(determination, success, failure));
        }
    }
}
