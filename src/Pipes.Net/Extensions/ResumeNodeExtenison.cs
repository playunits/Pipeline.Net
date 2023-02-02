using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pipes.Net.Nodes;

namespace Pipes.Net.Extensions
{
    public static class ResumeNodeExtenison
    {
        public static PipelineFactory Resume(this PipelineFactory factory)
        {
            return factory.Resume(1);
        }

        public static PipelineFactory Resume(this PipelineFactory factory, int resumationLevel)
        {
            return factory.Add(new ResumeNode(resumationLevel));
        }
    }
}
