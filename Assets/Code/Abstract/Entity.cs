using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Assets.Code.Abstract {
    public abstract class Entity{

        public Lexicon<IComponent> Components { get; private set; }
        public AttributeRepo Attributes { get; private set; }

        protected Entity() {
            Components = new Lexicon<IComponent>();
            Attributes = new AttributeRepo();
        }
        
    }
}