namespace Pipelines.Net
{
    public interface IPipelineBuilder<T> where T : IPipeline
    {
        public INode? Parent { get; set; }

        T Build();

        public IPipelineBuilder<T> Add(INode node);
        public IPipelineBuilder<T> AddSplit(MergeConditions mergeCondition, params Action<IPipelineBuilder<T>>[] builders);
        public IPipelineBuilder<T> AddSplit(MergeConditions mergeCondition, params T[] pipelines);
        public IPipelineBuilder<T> AddSplit(MergeConditions mergeCondition, params INode[] nodes);        
        public IPipelineBuilder<T> AddAction(Action action);                
        public IPipelineBuilder<T> AddAction(Action<INode> action);        
        public IPipelineBuilder<T> AddAction<TInput>(Action<TInput> action);        
        public IPipelineBuilder<T> AddAction<TInput>(Action<INode, TInput> action);        
        public IPipelineBuilder<T> AddAction(Func<object?> action);        
        public IPipelineBuilder<T> AddAction(Func<INode, object?> action);        
        public IPipelineBuilder<T> AddAction<TInput>(Func<TInput, object?> action);        
        public IPipelineBuilder<T> AddAction<TInput>(Func<INode, TInput, object?> action);
        public IPipelineBuilder<T> AddDecision<TInput>(Func<TInput, bool?> determination, Action<IPipelineBuilder<T>> success, Action<IPipelineBuilder<T>> failure);
        public IPipelineBuilder<T> AddDecision<TInput>(Func<TInput, bool?> determination, T success, T failure);
        public IPipelineBuilder<T> AddDecision<TInput>(Func<TInput, bool?> determination, INode success, INode failure);
        public IPipelineBuilder<T> Input(object? value);        
        public IPipelineBuilder<T> Resume();
        public IPipelineBuilder<T> Resume(int resumationLevel);
        public IPipelineBuilder<T> GetPreviousNode(out INode result);
        public IPipelineBuilder<T> Anchor(out INode result);
        public IPipelineBuilder<T> Output(string output);

        public IPipelineBuilder<T> Wait(int ms);

        public IPipelineBuilder<T> Wait(TimeSpan timespan);

        public IPipelineBuilder<T> Goto(INode node);


        // .Wait(ms) .Wait(Timespan)
        // .Goto(INode)
        // out INode option for every Add

        public INode? GetStartElement();
    }
}