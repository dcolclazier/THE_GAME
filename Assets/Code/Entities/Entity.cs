using Assets.Code.Abstract;

namespace Assets.Code.Entities {
    public class Entity{

        public Lexicon<IComponent> Components { get; private set; }
        public AttributeRepo Attributes { get; private set; }

        public Entity() {
            Components = new Lexicon<IComponent>();
            Attributes = new AttributeRepo();
        }
        
    }
}