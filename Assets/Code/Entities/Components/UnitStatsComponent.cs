using System.Collections.Generic;
using Assets.Code.Abstract;

namespace Assets.Code.Entities.Components {
    public class UnitStatsComponent : IComponent {
        public Entity Parent { get; set; }
        public List<string> Dependencies {
            get { return new List<string>() {
                "GameObject"
            };}
        }

        public void OnUpdate() {
            throw new System.NotImplementedException();
        }

        public void Init() {
            Parent.Attributes.Register("Strength",7f);
            Parent.Attributes.Register("Concentration",7f);
            Parent.Attributes.Register("Perception",7f);
            Parent.Attributes.Register("Magic",7f);
            Parent.Attributes.Register("Agility",7f);
            Parent.Attributes.Register("Wit",7f);
            Parent.Attributes.Register("Wisdom",7f);
            Parent.Attributes.Register("AttackSpeed",7f);
            Parent.Attributes.Register("MovementSpeed",7f);
        }
    }
}