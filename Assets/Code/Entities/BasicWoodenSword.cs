using System;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Entities.Components {
    public sealed class BasicWoodenSword : Weapon {
        public BasicWoodenSword() {
            
        }
        public BasicWoodenSword(Entity owner) : base(owner) {
            Init(owner);

        }

        public override void Init(Entity owner) {
            Owner = owner;
            WeaponType = WeaponType.OneHand;
            MainAttackModifier = 5;
            SecondaryAttackModifier = 2;
            ParryModifier = 6;
            WeaponRange = 2;
            
            //temp code - shuld go in inventory
            Messenger.AddListener<Entity, Entity, Entity>("OnMainAttack", OnMainAttack);
            Messenger.AddListener<Entity, Entity, Entity>("OnSecondaryAttack", OnMainAttack);
            Messenger.AddListener<Entity, Entity, Entity>("OnParry", OnMainAttack);
            PickupWeapon(Owner);
        }

        public override void OnMainAttack(Entity attacker, Entity target, Entity weaponUsed)
        {
            base.OnMainAttack(attacker,target,weaponUsed);
            var position = Owner.Attributes.Get<Vector2>("Position");
            var targetPosition = target.Attributes.Get<Vector2>("Position");

            if (Vector2.Distance(position, targetPosition) > WeaponRange) {
                Debug.Log("You're too far away to attack that target!");
                return;
            }

            var damage = Owner.Attributes.Get<float>("BaseDamage") + Owner.Attributes.Get<float>("MainAttackModifier");

            Debug.Log("Would have done " + damage + "damage to " + target.Attributes.Get<GameObject>("GameObject").name);
            
            
        }

        public override void OnParry(Entity attacker, Entity target, Entity weaponUsed)
        {
            if (Owner == null) Debug.Log("How can a weapon attack if it doesn't have an owner? Shennanigans!");
            throw new NotImplementedException();
        }

        public override void OnSecondaryAttack(Entity attacker, Entity target, Entity weaponUsed)
        {
            if (Owner == null) Debug.Log("How can a weapon attack if it doesn't have an owner? Shennanigans!");
            
        }

        public override void PickupWeapon(Entity owner) {
            base.PickupWeapon(owner);
            Owner.Attributes.Update("Wielded_OneHand", this);
        }
    }
}