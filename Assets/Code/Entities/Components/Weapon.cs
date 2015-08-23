using System;
using UnityEngine;

namespace Assets.Code.Entities.Components {

    [Flags] public enum LayerFlag { //In order of precedence
        Props = 1 << 8,
        NoWalk = 1 << 10,
        SelectCircle = 1 << 9,
        //Water = 1 << 4,
        Ground = 1 << 11,
        //Default = 0,
        //TransparentFx = 1,
        //IgnoreRayCast = 1 << 2,
        //All = Default | TransparentFx | IgnoreRayCast | Water | Ui | Props | SelectCircle | Ground | NoWalk
    }
    
    public abstract class Weapon : Entity, IWeapon, IOwnable, IPickupable {
        public virtual void Wield() {
            throw new System.NotImplementedException();
        }

        protected Weapon(Entity owner) {
            Owner = owner;
        }

        protected Weapon() {
            
        }

        public WeaponType WeaponType { get; protected set; }

        public float MainAttackModifier { get; protected set; }

        public float SecondaryAttackModifier { get; protected set; }

        public float ParryModifier { get; protected set; }
       
        public virtual void OnMainAttack(Entity attacker, Entity target, Entity weaponUsed) {
            if (Owner != attacker) return;
            if (weaponUsed != this) return;
            OnAttack(target);

            Owner.Attributes.Update("MainAttackModifier", MainAttackModifier);
            Debug.Log("I added my attack damage to the weapon modifier!" + MainAttackModifier);
        }

        private void OnAttack(Entity target)
        {
            if (Owner == null) {
                Debug.Log("How can a weapon attack if it doesn't have an owner? Shennanigans!");
                throw new NullReferenceException();
            }
            if (target == null) throw new NullReferenceException("You passed in a null target to attack??");
            if (!target.Attributes.Contains("AttackableComponent") || !target.Attributes.Get<bool>("AttackableComponent")) {
                Debug.Log("You tried to attack something, but you can't do that!");
            }
        }

        public virtual void OnSecondaryAttack(Entity attacker, Entity target, Entity weaponUsed)
        {
            if (Owner != attacker || weaponUsed != this) return;
            OnAttack(target);
            Owner.Attributes.Update("SecondaryAttackModifier", SecondaryAttackModifier);
            Debug.Log("I added my attack damage to the weapon modifier! "+SecondaryAttackModifier);
        }

        public virtual void OnParry(Entity attacker, Entity target, Entity weaponUsed)
        {
            if (Owner == null) {
                Debug.Log("How can a weapon parry if it doesn't have an owner? Shennanigans!");
                throw new NullReferenceException();
            }
            Owner.Attributes.Update("ParryModifier", ParryModifier);
            Debug.Log("I added my parry damage to the weapon modifier! "+ ParryModifier);
        }

        public int WeaponRange { get; protected set; }

        public virtual void PickupWeapon(Entity owner) {
            if(Owner != owner) Owner = owner;
            if (!Owner.Attributes.Contains("MainAttackModifier")) Owner.Attributes.Register("MainAttackModifier", MainAttackModifier);
            if (!Owner.Attributes.Contains("SecondaryAttackModifier")) Owner.Attributes.Register("SecondaryAttackModifier", SecondaryAttackModifier);
            if (!Owner.Attributes.Contains("ParryModifier")) Owner.Attributes.Register("ParryModifier", ParryModifier);
        }

        public override void Init(Entity owner) {
            Owner = this;
        }

    }
}