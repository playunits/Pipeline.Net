namespace Pipelines.Net
{    
    public class ResumeNode : PipelineNode
    {
        public int ResumationLevel { get; set; } = 1;
        public ResumeNode()
        {
            this.SplitExecutionTreeRoot = false;
        }

        public ResumeNode(int resumationLevel) : this()
        {
            this.ResumationLevel = resumationLevel;
        }

        public override Task<object?> Run(object? input)
        {
            INode? node = null;
            for (int i = 0; i<this.ResumationLevel; i++)
            {
                var searchNode = node;

                if (searchNode is null)
                {
                    searchNode = this;
                }

                var result = searchNode.SearchUp(x => x.SplitExecutionTreeRoot);
                if (result is not null)
                {
                    node = result;
                }
                else
                {
                    throw new InvalidOperationException($"No Split Execution Trees could be found on Level {i+1}");
                }                
            }
            //var node = this.SearchUp(x => x.SplitExecutionTreeRoot);

            if (node is not null)
            {
                if (node.Parent is not null)
                {
                    if (node.Parent is SplitNode)
                    {
                        throw new InvalidOperationException("Resume may not exit SplitNode Context");
                    }

                    if (node.Parent.Child is not null)
                    {
                        return node.Parent.Child.Run(input);
                    }
                }
            }
            throw new InvalidOperationException("Trying to resume Tree that does not exist");            
        }
    }

    


}