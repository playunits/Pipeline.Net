namespace Pipelines.Net
{
    public class ActionNode : PipelineNode
    {
        public Func<INode, object?, object?> ExecuteAction { get; set; }
        private bool handlesReturn = true;

        public ActionNode(Action<object?> action) : this((x) => { action(x); return null; })
        {
            this.handlesReturn = false;
        }

        public ActionNode(Action<INode> action) : this((x) => { action(x); return null;} )
        {
            this.handlesReturn = false;
        }

        public ActionNode(Action action) : this((x) => action())
        {
            this.handlesReturn = false;
        }

        public ActionNode(Func<INode, object?, object?> func)
        {
            this.ExecuteAction = func;
        }

        public ActionNode(Func<object?, object?> func) : this((x, y) => func(y))
        {            
        }

        public ActionNode(Func<INode, object?> func) : this((x,y) => func(x))
        {            
        }

        public ActionNode(Func<object?> func) : this((x, y) => func())
        {
        }

        public override async Task<object?> Run(object? input)
        {
            var result = await Task.Factory.StartNew(() => this.ExecuteAction(this, input));

            if (!this.handlesReturn)
            {
                result = input;
            }

            return await base.Run(result);
        }
    }




}