namespace Pipelines.Net
{
    public class Pipeline
    {
        public INode? StartElement { get; set; }        
        public bool SplitExecutionTreeRoot { get; set; }

        public Pipeline()
        {
            this.SplitExecutionTreeRoot = true;
        }

        public Task<object?> Run()
        {
            return this.Run(null);
        }

        public Task<object?> Run(object? input)
        {
            if (this.StartElement is not null)
            {
                return this.StartElement.Run(input);
            }
            else
            {
                return Task.FromResult<object?>(null);
            }
        }
    }
}