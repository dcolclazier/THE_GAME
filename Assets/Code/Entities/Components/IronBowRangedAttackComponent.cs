using System;
using System.Collections.Generic;
using Assets.Code.Abstract;
using Assets.Code.Statics;

namespace Assets.Code.Entities.Components {
    public class IronBowRangedAttackComponent : IComponent {
        public Entity Parent { get; set; }
        public List<string> Dependencies {
            get { return new List<string>() {
                "PlayerCurrentTarget"
            }; }
        }


        private float maxRange;
        private float minRange;

        private void OnFiredWeapon(Entity weaponFired, Entity whoFired) {
            throw new NotImplementedException();
            //if (weaponFired != Parent) return;
            //if this weapon doesn't belong to whoFired, return
            // other cases I shouldn't run this code, return
            
            //var target = whoFired.Attributes.Get<Entity>("PlayerCurrentTarget");

            //var defenseValue = target.Components.Get<DefenderCompononent>("DefenderComponent").CalculateDefense(weaponFired);
            //var damageDone = weaponFired.Attributes.Get<float>("WeaponDamage") - defenseValue;

            //target.Attributes.Update("Health", target.Attributes.Get<float>("Health")-damageDone);
            //Messenger.Broadcast("WeaponHit!", );

        }

        public void OnUpdate() {
            throw new NotImplementedException();
        }

        public void OnAwake() {
            throw new NotImplementedException();
        }

        public void OnStart() {
            throw new NotImplementedException();
        }

        public void Init() {
            
        }

        public void OnMessage() {
            Messenger.AddListener<Entity, Entity>("PlayerFiredRangedWeapon", OnFiredWeapon);
        }
    }
}