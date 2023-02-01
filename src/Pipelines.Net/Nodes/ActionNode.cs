namespace Pipelines.Net.Nodes
{
    public class ActionNode : PipelineNode
    {
        public Func<INode, object?, object?> ExecuteAction { get; set; }
        public bool HandlesReturnValues { get; set; } = true;


        public static ActionNode Create(Action action)
        {
            return new ActionNode((node, input) => { action(); return null; })
            {
                HandlesReturnValues = false
            };
        }

        public static ActionNode Create(Action<INode> action)
        {
            return new ActionNode((node, input) => { action(node); return null; })
            {
                HandlesReturnValues = false
            };
        }

        public static ActionNode Create<TInput>(Action<TInput> action)
        {
            var tmp = new Func<INode, object?, object?>((node, input) =>
            {
                var args = TypeConverter.Convert<TInput>(input);

                action(args);
                return null;
            });

            return new ActionNode(tmp)
            {
                HandlesReturnValues = false
            };
        }

        public static ActionNode Create<TInput>(Action<INode, TInput> action)
        {
            var tmp = new Func<INode, object?, object?>((node, input) =>
            {
                var args = TypeConverter.Convert<TInput>(input);

                action(node, args);
                return null;
            });

            return new ActionNode(tmp)
            {
                HandlesReturnValues = false
            };
        }

        public static ActionNode Create(Func<object?> action)
        {
            return new ActionNode((node, input) => action());
        }

        public static ActionNode Create(Func<INode, object?> action)
        {
            return new ActionNode((node, input) => action(node));
        }

        public static ActionNode Create<TInput>(Func<TInput, object?> action)
        {
            var tmp = (INode node, object? input) =>
            {
                var args = TypeConverter.Convert<TInput>(input);

                return action(args);
            };

            return new ActionNode(tmp);
        }

        public static ActionNode Create<TInput>(Func<INode, TInput, object?> action)
        {
            var tmp = (INode node, object? input) =>
            {
                var args = TypeConverter.Convert<TInput>(input);

                return action(node, args);
            };

            return new ActionNode(tmp);
        }

        public ActionNode(Func<INode, object?, object?> func)
        {
            ExecuteAction = func;
        }

        public override async Task<object?> Run(object? input)
        {
            var result = await Task.Factory.StartNew(() => ExecuteAction(this, input));

            if (!HandlesReturnValues)
            {
                result = input;
            }

            return await base.Run(result);
        }
    }




}