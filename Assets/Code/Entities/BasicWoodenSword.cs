using System;
using UnityEngine;

namespace Assets.Code.Entities.Components {
    public class BasicWoodenSword : Weapon {
        public BasicWoodenSword(Entity owner) : base(owner) {
            WeaponType = WeaponType.OneHand;
            MainAttackModifier = 5;
            SecondaryAttackModifier = 2;
            ParryModifier = 6;
            WeaponRange = 2;

        }

        public override void OnMainAttack(Entity attackingEntity)
        {
            base.OnMainAttack(attackingEntity);
            
            
            
        }

        public override void OnParry(Entity attackingEntity)
        {
            if (Owner == null) Debug.Log("How can a weapon attack if it doesn't have an owner? Shennanigans!");
            throw new NotImplementedException();
        }

        public override void OnSecondaryAttack(Entity attackedEntity)
        {
            if (Owner == null) Debug.Log("How can a weapon attack if it doesn't have an owner? Shennanigans!");
            
        }

        public override void Pickup(Entity pickedUpBy) {
            base.Pickup(pickedUpBy);
            
            Owner.Attributes.Update("Wielded_OneHand", this as IWieldable);
        }
    }
}