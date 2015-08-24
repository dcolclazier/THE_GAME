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
            Parent.Attributes.RegisterOrUpdate("Strength",7);
            Parent.Attributes.RegisterOrUpdate("Concentration",7);
            Parent.Attributes.RegisterOrUpdate("Perception",7);
            Parent.Attributes.RegisterOrUpdate("Magic",7);
            Parent.Attributes.RegisterOrUpdate("Agility",7);
            Parent.Attributes.RegisterOrUpdate("Wit",7);
            Parent.Attributes.RegisterOrUpdate("Wisdom",7);
            Parent.Attributes.RegisterOrUpdate("AttackSpeed",7);
            Parent.Attributes.RegisterOrUpdate("MovementSpeed",7f);
            Parent.Attributes.RegisterOrUpdate("BaseDamage", 1);
        }
    }
}