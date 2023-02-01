namespace Pipelines.Net
{
    public class Pipeline : IPipeline
    {
        public INode? Child { get; set; }
        public INode? Parent { get; set; }
        public bool SplitExecutionTreeRoot { get; set; }

        public Pipeline()
        {
            this.SplitExecutionTreeRoot = true;
        }

        public void Append(INode node)
        {
            throw new NotImplementedException();
        }

        public Task Run()
        {
            return this.Run(null);
        }

        public Task<object?> Run(object? input)
        {
            if (this.Child is not null)
            {
                return this.Child.Run(input);
            }
            else
            {
                return Task.FromResult<object?>(null);
            }
        }

        public INode? SearchUp(Func<INode, bool> search, bool searchOnSelf = false)
        {
            throw new NotImplementedException();
        }
    }
}