using System.Collections.Generic;
using Assets.Code.Abstract;

namespace Assets.Code.Entities {
    public class AttackableComponent : IComponent {
        public Entity Parent { get; set; }

        public List<string> Dependencies { get; private set; }

        public void OnUpdate() {
            throw new System.NotImplementedException();
        }

        public void Init() {
            Parent.Attributes.Register("AttackableComponent", true);
        }
    }
}