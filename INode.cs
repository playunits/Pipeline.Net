using System.Runtime.CompilerServices;

namespace Pipelines.Net
{
    public interface INode
    {
        INode? Parent { get; set; }
        INode? Child { get; set; }

        bool SplitExecutionTreeRoot { get; set; }

        Task<object?> Run(object? input);

        void Append(INode node);

        INode? SearchUp(Func<INode, bool> search, bool searchOnSelf = false);
    }
}