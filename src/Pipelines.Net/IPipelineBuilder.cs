namespace Pipelines.Net
{
    public interface IPipelineFactory<T> where T : IPipeline
    {
        public INode? Parent { get; set; }

        T Build();

        public IPipelineFactory<T> Add(INode node);
        public IPipelineFactory<T> AddSplit(MergeConditions mergeCondition, params Action<IPipelineFactory<T>>[] builders);
        public IPipelineFactory<T> AddSplit(MergeConditions mergeCondition, params T[] pipelines);
        public IPipelineFactory<T> AddSplit(MergeConditions mergeCondition, params INode[] nodes);        
        public IPipelineFactory<T> AddAction(Action action);                
        public IPipelineFactory<T> AddAction(Action<INode> action);        
        public IPipelineFactory<T> AddAction<TInput>(Action<TInput> action);        
        public IPipelineFactory<T> AddAction<TInput>(Action<INode, TInput> action);        
        public IPipelineFactory<T> AddAction(Func<object?> action);        
        public IPipelineFactory<T> AddAction(Func<INode, object?> action);        
        public IPipelineFactory<T> AddAction<TInput>(Func<TInput, object?> action);        
        public IPipelineFactory<T> AddAction<TInput>(Func<INode, TInput, object?> action);
        public IPipelineFactory<T> AddDecision<TInput>(Func<TInput, bool?> determination, Action<IPipelineFactory<T>> success, Action<IPipelineFactory<T>> failure);
        public IPipelineFactory<T> AddDecision<TInput>(Func<TInput, bool?> determination, T success, T failure);
        public IPipelineFactory<T> AddDecision<TInput>(Func<TInput, bool?> determination, INode success, INode failure);
        public IPipelineFactory<T> Input(object? value);        
        public IPipelineFactory<T> Resume();
        public IPipelineFactory<T> Resume(int resumationLevel);
        public IPipelineFactory<T> PreviousNode(out INode result);
        public IPipelineFactory<T> Anchor(out INode result);
        public IPipelineFactory<T> Goto(INode node);

        public INode? GetStartElement();
    }
}