using Assets.Code.Abstract;

namespace Assets.Code.Entities {
    public class Entity{

        public Entity Owner { get; set; }
        public Lexicon<IComponent> Components { get; private set; }
        public AttributeRepo Attributes { get; private set; }

        public Entity() {
            Components = new Lexicon<IComponent>();
            Attributes = new AttributeRepo();
        }

        public virtual void Init(Entity entity) {
            //Only used by entitys created by the entity initializer.
        }
    }
}