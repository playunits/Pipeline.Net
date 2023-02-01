namespace Pipelines.Net
{
    public class GotoNode : PipelineNode
    {
        public INode Target { get; set; }

        public GotoNode(INode target)
        {
            this.Target = target;
        }

        public override Task<object?> Run(object? input)
        {
            return Target.Run(input);
        }
    }

    


}