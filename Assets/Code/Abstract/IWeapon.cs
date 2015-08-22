namespace Assets.Code.Entities.Components {
    public interface IWeapon : IWieldable {
        WeaponType WeaponType { get; }

        float MainAttackModifier { get; }
        float SecondaryAttackModifier { get; }
        float ParryModifier { get; }

        void OnMainAttack(Entity attackedEntity);
        void OnSecondaryAttack(Entity attackedEntity);
        void OnParry(Entity attackingEntity);
        int WeaponRange { get; }

    }
}