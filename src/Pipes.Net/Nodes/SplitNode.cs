using System.Threading.Tasks;

namespace Pipes.Net.Nodes
{

    public class SplitNode : PipelineNode
    {
        private List<Task> Tasks { get; set; } = new List<Task>();

        public List<INode> SubProcesses { get; private set; } = new List<INode>();
        public MergeConditions Conditions { get; set; }
        public SplitNode(MergeConditions conditions)
        {
            Conditions = conditions;
        }

        public SplitNode(MergeConditions conditions, IEnumerable<INode> subprocesses)
        {
            Conditions = conditions;
            AddSubProcesses(subprocesses);
        }

        public SplitNode(MergeConditions conditions, INode subprocess)
        {
            Conditions = conditions;
            AddSubProcess(subprocess);
        }

        public override async Task<object?> Run(object? input)
        {
            foreach (var pipeline in SubProcesses)
            {
                var task = pipeline.Run(input);
                Tasks.Add(task);
            }

            Task bundleTask = Task.CompletedTask;
            switch (Conditions)
            {
                default:
                case MergeConditions.AllFinished:
                    bundleTask = Task.WhenAll(Tasks);
                    break;
                case MergeConditions.AnyFinished:
                    bundleTask = Task.WhenAny(Tasks);
                    break;
                case MergeConditions.FireAndForget:
                    bundleTask = Task.CompletedTask;
                    break;
            }

            await bundleTask;
            return await base.Run(input);
        }

        public override void Append(INode node)
        {
            if (node.Parent is not null)
            {
                throw new InvalidOperationException("This node already has a parent");
            }
            else
            {
                Child = node;
                node.Parent = this;
            }
        }

        public void AddSubProcess(INode node)
        {
            node.Parent = this;
            node.SplitExecutionTreeRoot = true;
            SubProcesses.Add(node);
        }

        public void AddSubProcesses(IEnumerable<INode> nodes)
        {
            foreach (var node in nodes)
            {
                AddSubProcess(node);
            }
        }
    }




}