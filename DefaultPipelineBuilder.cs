using System;

namespace Pipelines.Net
{

    public class DefaultPipelineBuilder : IPipelineBuilder<Pipeline>
    {
        public INode? Parent { get; set; }

        private bool isFirst = true;

        private INode? StartElement;

        private INode? tmpStorage;

        private void Append(INode element)
        {
            if (isFirst)
            {
                this.StartElement = element;
                this.isFirst = false;
            }

            if (Parent is not null)
            {
                Parent.Append(element);
            }

            if (tmpStorage is not null)
            {
                this.tmpStorage = element;
            }

            Parent = element;
        }

        public IPipelineBuilder<Pipeline> Add(INode node)
        {
            this.Append(node);
            return this;
        }
        public IPipelineBuilder<Pipeline> AddAction(Action action, out INode result)
        {
            result = new ActionNode(action);            
            this.Append(result);
            return this;
        }

        public IPipelineBuilder<Pipeline> AddAction(Action action)
        {
            return this.AddAction(action, out INode _);
        }

        public IPipelineBuilder<Pipeline> AddAction(Action<INode> action)
        {
            var node = new ActionNode(action);
            this.Append(node);
            return this;
        }

        public IPipelineBuilder<Pipeline> AddSplit(MergeConditions mergeCondition, params Action<IPipelineBuilder<Pipeline>>[] builders)
        {
            return this.AddSplit(mergeCondition, builders.Select(x =>
            {
                DefaultPipelineBuilder builder = new DefaultPipelineBuilder();
                x(builder);

                return builder.Build();
            }).ToArray());
        }

        public IPipelineBuilder<Pipeline> AddSplit(MergeConditions mergeCondition, params Pipeline[] pipelines)
        {
            return this.AddSplit(mergeCondition, pipelines.Select(x => x.Child).Where(x => x is not null).Cast<INode>().ToArray());
        }

        public IPipelineBuilder<Pipeline> AddSplit(MergeConditions mergeCondition, params INode[] nodes)
        {            
            var splitNode = new SplitNode(mergeCondition);            
            splitNode.AddSubProcesses(nodes.ToList());
            this.Append(splitNode);
            return this;
        }

        public IPipelineBuilder<Pipeline> AddDecision(Func<object?, bool?> determination, Action<IPipelineBuilder<Pipeline>> success, Action<IPipelineBuilder<Pipeline>> failure)
        {
            var successBuilder = new DefaultPipelineBuilder();
            success(successBuilder);

            var failureBuilder = new DefaultPipelineBuilder();
            failure(failureBuilder);

            return this.AddDecision(determination, successBuilder.Build(), failureBuilder.Build());
        }

        public IPipelineBuilder<Pipeline> AddDecision(Func<object?, bool?> determination, Pipeline success, Pipeline failure)
        {
            return this.AddDecision(determination, success.Child, failure.Child);
        }

        public IPipelineBuilder<Pipeline> AddDecision(Func<object?, bool?> determination, INode? success, INode? failure)
        {
            var decisionNode = new DecisionNode(determination, success, failure);
            this.Append(decisionNode);
            return this;
        }

        public IPipelineBuilder<Pipeline> Resume()
        {
            var resumeNode = new ResumeNode();
            this.Append(resumeNode);
            return this;
        }

        public IPipelineBuilder<Pipeline> Resume(int resumationLevel)
        {
            var resumeNode = new ResumeNode(resumationLevel);
            this.Append(resumeNode);
            return this;
        }

        public Pipeline Build()
        {
            var pipeline = new Pipeline();
            pipeline.Child = this.GetStartElement();

            return pipeline;
        }

        public INode? GetStartElement() => this.StartElement;

        public IPipelineBuilder<Pipeline> AddAction(Action<object?> action)
        {
            var node = new ActionNode(action);
            this.Append(node);
            return this;
        }

        public IPipelineBuilder<Pipeline> AddAction(Func<object?> action)
        {
            var node = new ActionNode(action);
            this.Append(node);
            return this;
        }

        public IPipelineBuilder<Pipeline> AddAction(Func<object?, object?> action)
        {
            var node = new ActionNode(action);
            this.Append(node);
            return this;
        }

        public IPipelineBuilder<Pipeline> AddAction(Func<INode, object?> action)
        {
            var node = new ActionNode(action);
            this.Append(node);
            return this;
        }

        public IPipelineBuilder<Pipeline> AddAction(Func<INode, object?, object?> action)
        {
            var node = new ActionNode(action);
            this.Append(node);
            return this;
        }

        public IPipelineBuilder<Pipeline> Input(object? value)
        {
            var node = new ActionNode((INode x, object? y) =>
            {
                return value;
            });
            this.Append(node);
            return this;
        }

        public IPipelineBuilder<Pipeline> GetPreviousNode(out INode result)
        {
            if (this.Parent is null)
            {
                throw new InvalidOperationException("Something has to be added to the pipeline, before it can be retrieved");
            }
            else
            {
                result = this.Parent;
                return this;
            }            
        }

        public IPipelineBuilder<Pipeline> Anchor(out INode result)
        {
            var node = new PipelineNode();
            result = node;
            this.Append(node);
            return this;
        }

        public IPipelineBuilder<Pipeline> Wait(int ms)
        {
            return this.Wait(TimeSpan.FromMilliseconds(ms));
        }

        public IPipelineBuilder<Pipeline> Wait(TimeSpan timespan)
        {
            var node = new ActionNode(() =>
            {
                Thread.Sleep(timespan);
            });
            this.Append(node);
            return this;
        }

        public IPipelineBuilder<Pipeline> Goto(INode node)
        {
            var gotoNode = new GotoNode(node);
            this.Append(gotoNode);
            return this;
        }
    }
}