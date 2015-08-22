using System;
using UnityEngine;

namespace Assets.Code.Entities.Components {
    public abstract class Weapon : Entity, IWeapon, IOwnable, IPickupable {
        public virtual void Wield() {
            throw new System.NotImplementedException();
        }

        protected Weapon(Entity owner) {
            Owner = owner;
        }
        public WeaponType WeaponType { get; protected set; }

        public float MainAttackModifier { get; protected set; }

        public float SecondaryAttackModifier { get; protected set; }

        public float ParryModifier { get; protected set; }
       
        public virtual void OnMainAttack(Entity attackedEntity) {
            OnAttack(attackedEntity);
            Owner.Attributes.Update("MainAttackModifier", MainAttackModifier);
            Debug.Log("I added my attack damage to the weapon modifier!" + MainAttackModifier);
        }

        private void OnAttack(Entity attackedEntity) {
            if (Owner == null) {
                Debug.Log("How can a weapon attack if it doesn't have an owner? Shennanigans!");
                throw new NullReferenceException();
            }
            if (attackedEntity == null) throw new NullReferenceException("You passed in a null attackedEntity to attack??");
            if (!attackedEntity.Attributes.Contains("AttackableComponent") || !attackedEntity.Attributes.Get<bool>("AttackableComponent")) {
                Debug.Log("You tried to attack something, but you can't do that!");
            }
        }

        public virtual void OnSecondaryAttack(Entity attackedEntity)
        {
            OnAttack(attackedEntity);
            Owner.Attributes.Update("SecondaryAttackModifier", SecondaryAttackModifier);
            Debug.Log("I added my attack damage to the weapon modifier! "+SecondaryAttackModifier);
        }

        public virtual void OnParry(Entity attackedEntity)
        {
            if (Owner == null) {
                Debug.Log("How can a weapon parry if it doesn't have an owner? Shennanigans!");
                throw new NullReferenceException();
            }
            Owner.Attributes.Update("ParryModifier", ParryModifier);
            Debug.Log("I added my parry damage to the weapon modifier! "+ ParryModifier);
        }

        public int WeaponRange { get; protected set; }

        public Entity Owner { get; private set; }
        public virtual void Pickup(Entity pickedUpBy) {
            Owner = pickedUpBy;
            if (!Owner.Attributes.Contains("MainAttackModifier")) Owner.Attributes.Register("MainAttackModifier", MainAttackModifier);
            if (!Owner.Attributes.Contains("SecondaryAttackModifier")) Owner.Attributes.Register("MainAttackModifier", SecondaryAttackModifier);
            if (!Owner.Attributes.Contains("ParryModifier")) Owner.Attributes.Register("MainAttackModifier", ParryModifier);
        }
    }
}