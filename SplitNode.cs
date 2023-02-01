﻿using System.Threading.Tasks;

namespace Pipelines.Net
{

    public class SplitNode : PipelineNode
    {        
        private List<Task> Tasks { get; set; } = new List<Task>();

        public List<INode> SubProcesses { get; private set; } = new List<INode>();
        public MergeConditions Conditions { get; set; }
        public SplitNode(MergeConditions conditions)
        {            
            this.Conditions = conditions;
        }

        public SplitNode(MergeConditions conditions, IEnumerable<INode> subprocesses)
        {
            this.Conditions = conditions;
            this.AddSubProcesses(subprocesses);
        }

        public SplitNode(MergeConditions conditions, INode subprocess)
        {
            this.Conditions = conditions;
            this.AddSubProcess(subprocess);
        }

        public override async Task<object?> Run(object? input)
        {
            foreach (var pipeline in this.SubProcesses)
            {
                var task = pipeline.Run(input);                
                this.Tasks.Add(task);
            }

            Task bundleTask = Task.CompletedTask;
            switch (this.Conditions)
            {
                default:
                case MergeConditions.AllFinished:
                    bundleTask = Task.WhenAll(this.Tasks);
                    break;
                case MergeConditions.AnyFinished:
                    bundleTask = Task.WhenAny(this.Tasks);
                    break;
                case MergeConditions.FireAndForget:
                    bundleTask = Task.CompletedTask;
                    break;
            }

            await bundleTask;
            return await base.Run(input);
        }

        public override void Append(INode node)
        {
            if (node.Parent is not null)
            {
                throw new InvalidOperationException("This node already has a parent");
            }
            else
            {
                this.Child = node;
                node.Parent = this;                
            }
        }

        public void AddSubProcess(INode node)
        {
            node.Parent = this;
            node.SplitExecutionTreeRoot = true;
            this.SubProcesses.Add(node);
        }

        public void AddSubProcesses(IEnumerable<INode> nodes)
        {
            foreach (var node in nodes)
            {
                this.AddSubProcess(node);
            }
        }
    }

    


}