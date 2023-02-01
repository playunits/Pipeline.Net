namespace Pipelines.Net
{
    public class PipelineNode : INode
    {
        public INode? Parent { get; set; }
        public INode? Child { get; set; }
        public bool SplitExecutionTreeRoot { get; set; } = false;

        public virtual void Append(INode node)
        {
            if (this.Child is not null)
            {
                throw new InvalidOperationException("This node already has a child");
            }
            else if (node.Parent is not null)
            {
                throw new InvalidOperationException("This node already has a parent");
            }
            else
            {
                this.Child = node;
                node.Parent = this;
            }
        }

        public virtual Task<object?> Run(object? input)
        {
            if (this.Child is not null)
            {
                return this.Child.Run(input);
            }
            return Task.FromResult(input);
        }

        public INode? SearchUp(Func<INode, bool> search, bool searchOnSelf = false)
        {
            if (searchOnSelf)
            {
                if (search(this))
                {
                    return this;
                }
            }

            if (this.Parent is not null)
            {
                if (search(this.Parent))
                {
                    return this.Parent;
                }
                else
                {
                    return this.Parent.SearchUp(search, searchOnSelf);
                }
            }
            else
            {
                return null;
            }
        }
    }




}