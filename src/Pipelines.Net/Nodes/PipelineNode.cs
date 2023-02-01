namespace Pipelines.Net.Nodes
{
    public class PipelineNode : INode
    {
        public INode? Parent { get; set; }
        public INode? Child { get; set; }
        public bool SplitExecutionTreeRoot { get; set; } = false;

        public virtual void Append(INode node)
        {
            if (Child is not null)
            {
                throw new InvalidOperationException("This node already has a child");
            }
            else if (node.Parent is not null)
            {
                throw new InvalidOperationException("This node already has a parent");
            }
            else
            {
                Child = node;
                node.Parent = this;
            }
        }

        public virtual Task<object?> Run(object? input)
        {
            if (Child is not null)
            {
                return Child.Run(input);
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

            if (Parent is not null)
            {
                if (search(Parent))
                {
                    return Parent;
                }
                else
                {
                    return Parent.SearchUp(search, searchOnSelf);
                }
            }
            else
            {
                return null;
            }
        }
    }




}