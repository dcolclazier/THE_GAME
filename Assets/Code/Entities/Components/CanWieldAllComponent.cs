using System.Collections.Generic;
using Assets.Code.Abstract;
using UnityEngine;

namespace Assets.Code.Entities.Components {
    public class CanWieldAllComponent : IComponent {
        public Entity Parent { get; set; }

        public List<string> Dependencies {
            get { return new List<string>() {
                
            }; }
        }

        public void Wield(Entity weaponToWield, WeaponType weaponType) {
            if (!(weaponToWield is IWieldable)) {
                Debug.Log("Tried to wield something that isn't wieldable...");
                return;
            }
 
        }
        public void OnUpdate() {
            throw new System.NotImplementedException();
        }

        public void Init() {
            Parent.Attributes.Register("CanWieldDual", true);
            Parent.Attributes.Register("CanWieldOneHand", true);
            Parent.Attributes.Register("CanWieldTwoHand", true);
            Parent.Attributes.Register("Wielded_OneHand", default(IWieldable));
            Parent.Attributes.Register("Wielded_TwoHand", default(IWieldable));
            Parent.Attributes.Register("Wielded_Dual", default(IWieldable));

        }
    }
}