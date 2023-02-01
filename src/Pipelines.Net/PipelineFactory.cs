using System;
using Pipelines.Net.Nodes;

namespace Pipelines.Net
{

    public class PipelineFactory
    {
        private INode? StartElement;

        #region Public Properties
        public INode? Parent { get; set; }
        #endregion

        #region Constructors & Destructors
        public PipelineFactory()
        {
            this.Reset();
        }
        #endregion

        #region Private Methods
        private void Append(INode element)
        {
            if (this.StartElement is null)
            {
                this.StartElement = element;                
            }

            if (Parent is not null)
            {
                Parent.Append(element);
            }
            Parent = element;
        }

        private void Reset()
        {
            this.Parent = null;
            this.StartElement = null;
        }        
        #endregion

        #region Public Methods
        public Pipeline Build()
        {
            var pipeline = new Pipeline();
            pipeline.StartElement = this.GetStartElement();

            this.Reset();

            return pipeline;
        }
        public INode? GetStartElement() => this.StartElement;

        public PipelineFactory Add(INode node)
        {
            this.Append(node);
            return this;
        }                                                                
        #endregion        
    }
}