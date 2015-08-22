namespace Assets.Code.Entities.Components {
    public interface IWeapon : IWieldable {
        WeaponType WeaponType { get; }

        float MainAttackModifier { get; }
        float SecondaryAttackModifier { get; }
        float ParryModifier { get; }

        void OnMainAttack(Entity attacker, Entity target, Entity weaponUsed);
        void OnSecondaryAttack(Entity attacker, Entity target, Entity weaponUsed);
        void OnParry(Entity attacker, Entity target, Entity weaponUsed);
        int WeaponRange { get; }

    }
}