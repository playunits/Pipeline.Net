using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pipes.Net.Nodes;

namespace Pipes.Net.Extensions
{
    public static class GotoNodeExtension
    {
        public static PipelineFactory Goto(this PipelineFactory factory, INode node)
        {
            return factory.Add(new GotoNode(node));
        }
    }
}
