using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pipes.Net.Nodes;

namespace Pipes.Net.Extensions
{
    public static class SplitNodeExtension
    {
        public static PipelineFactory AddSplit(this PipelineFactory factory, MergeConditions mergeCondition, params Action<PipelineFactory>[] builders)
        {
            return factory.AddSplit(mergeCondition, builders.Select(x =>
            {
                PipelineFactory builder = new PipelineFactory();
                x(builder);

                return builder.Build();
            }).ToArray());
        }

        public static PipelineFactory AddSplit(this PipelineFactory factory, MergeConditions mergeCondition, params Pipeline[] pipelines)
        {
            return factory.AddSplit(mergeCondition, pipelines.Select(x => x.StartElement).Where(x => x is not null).Cast<INode>().ToArray());
        }

        public static PipelineFactory AddSplit(this PipelineFactory factory, MergeConditions mergeCondition, params INode[] nodes)
        {
            var splitNode = new SplitNode(mergeCondition);
            splitNode.AddSubProcesses(nodes.ToList());
            return factory.Add(splitNode);
        }
    }
}
