using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pipes.Net.Nodes;

namespace Pipes.Net.Extensions
{
    public static class ActionNodeExtension
    {
        public static PipelineFactory AddAction(this PipelineFactory factory, Action action)
        {
            return factory.Add(ActionNode.Create(action));
        }

        public static PipelineFactory AddAction(this PipelineFactory factory, Action<INode> action)
        {
            return factory.Add(ActionNode.Create(action));
        }

        public static PipelineFactory AddAction<T>(this PipelineFactory factory, Action<T> action)
        {
            return factory.Add(ActionNode.Create(action));
        }

        public static PipelineFactory AddAction<T>(this PipelineFactory factory, Action<INode, T> action)
        {
            return factory.Add(ActionNode.Create(action));
        }

        public static PipelineFactory AddAction(this PipelineFactory factory, Func<object?> action)
        {
            return factory.Add(ActionNode.Create(action));
        }

        public static PipelineFactory AddAction(this PipelineFactory factory, Func<INode, object?> action)
        {
            return factory.Add(ActionNode.Create(action));
        }

        public static PipelineFactory AddAction<T>(this PipelineFactory factory, Func<T, object?> action)
        {
            return factory.Add(ActionNode.Create(action));
        }

        public static PipelineFactory AddAction<T>(this PipelineFactory factory, Func<INode, T, object?> action)
        {
            return factory.Add(ActionNode.Create(action));
        }
    }
}
