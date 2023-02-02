namespace Pipes.Net.Nodes
{
    public class GotoNode : PipelineNode
    {
        public INode Target { get; set; }

        public GotoNode(INode target)
        {
            Target = target;
        }

        public override Task<object?> Run(object? input)
        {
            return Target.Run(input);
        }
    }




}