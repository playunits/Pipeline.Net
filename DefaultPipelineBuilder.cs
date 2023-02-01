using System;

namespace Pipelines.Net
{

    public class DefaultPipelineBuilder : IPipelineBuilder<Pipeline>
    {
        public INode? Parent { get; set; }        

        private INode? StartElement;        

        private void Append(INode element)
        {
            if (this.StartElement is null)
            {
                this.StartElement = element;                
            }

            if (Parent is not null)
            {
                Parent.Append(element);
            }
            Parent = element;
        }

        public IPipelineBuilder<Pipeline> Add(INode node)
        {
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

        public IPipelineBuilder<Pipeline> AddDecision<T>(Func<T, bool?> determination, Action<IPipelineBuilder<Pipeline>> success, Action<IPipelineBuilder<Pipeline>> failure)
        {
            var successBuilder = new DefaultPipelineBuilder();
            success(successBuilder);

            var failureBuilder = new DefaultPipelineBuilder();
            failure(failureBuilder);

            return this.AddDecision(determination, successBuilder.Build(), failureBuilder.Build());
        }

        public IPipelineBuilder<Pipeline> AddDecision<T>(Func<T, bool?> determination, Pipeline success, Pipeline failure)
        {
            return this.AddDecision(determination, success.Child, failure.Child);
        }

        public IPipelineBuilder<Pipeline> AddDecision<T>(Func<T, bool?> determination, INode? success, INode? failure)
        {
            this.Append(DecisionNode.Create(determination, success, failure));
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

            this.Reset();

            return pipeline;
        }

        public INode? GetStartElement() => this.StartElement;

        public IPipelineBuilder<Pipeline> AddAction(Action action)
        {
            this.Append(ActionNode.Create(action));
            return this;
        }

        public IPipelineBuilder<Pipeline> AddAction(Action<INode> action)
        {
            this.Append(ActionNode.Create(action));
            return this;
        }

        public IPipelineBuilder<Pipeline> AddAction<T>(Action<T> action)
        {
            this.Append(ActionNode.Create(action));
            return this;
        }

        public IPipelineBuilder<Pipeline> AddAction<T>(Action<INode, T> action)
        {
            this.Append(ActionNode.Create(action));
            return this;
        }

        public IPipelineBuilder<Pipeline> AddAction(Func<object?> action)
        {
            this.Append(ActionNode.Create(action));
            return this;
        }
        public IPipelineBuilder<Pipeline> AddAction(Func<INode, object?> action)
        {
            this.Append(ActionNode.Create(action));
            return this;
        }

        public IPipelineBuilder<Pipeline> AddAction<T>(Func<T, object?> action)
        {
            this.Append(ActionNode.Create(action));
            return this;
        }


        public IPipelineBuilder<Pipeline> AddAction<T>(Func<INode, T, object?> action)
        {
            this.Append(ActionNode.Create(action));
            return this;
        }

        public IPipelineBuilder<Pipeline> Input(object? value)
        {
            this.Append(ActionNode.Create(() => value));
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
            this.Append(ActionNode.Create(() => Thread.Sleep(timespan)));            
            return this;
        }

        public IPipelineBuilder<Pipeline> Goto(INode node)
        {
            var gotoNode = new GotoNode(node);
            this.Append(gotoNode);
            return this;
        }

        public IPipelineBuilder<Pipeline> Output(string output)
        {
            this.Append(ActionNode.Create(() => Console.WriteLine(output)));
            return this;
        }

        private void Reset()
        {
            this.Parent = null;
            this.StartElement = null;
        }

        public DefaultPipelineBuilder()
        {
            this.Reset();
        }
    }
}