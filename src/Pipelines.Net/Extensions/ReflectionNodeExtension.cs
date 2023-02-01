using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pipelines.Net.Nodes;

namespace Pipelines.Net.Extensions
{
    public static class ReflectionNodeExtension
    {

        public static PipelineFactory PreviousNode(this PipelineFactory factory, out INode result)
        {
            if (factory.Parent is null)
            {
                throw new InvalidOperationException("Something has to be added to the pipeline, before it can be retrieved");
            }
            else
            {
                result = factory.Parent;
            }
            return factory;
        }

        public static PipelineFactory Anchor(this PipelineFactory factory, out INode result)
        {
            var node = new PipelineNode();
            result = node;
            factory.Add(node);
            return factory;
        }
    }
}
