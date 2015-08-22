using System.Collections.Generic;
using Assets.Code.Abstract;

namespace Assets.Code.Entities.Components {
    public class FullyEquippedComponent : IComponent
    {
        public Entity Parent { get; set; }
        public List<string> Dependencies
        {
            get
            {
                return new List<string>() {
                    "GameObject"
                };
            }
        }

        public void OnUpdate()
        {
            throw new System.NotImplementedException();
        }

        public void Init()
        {
            Parent.Attributes.Register("CanEquipAllBasic", true);
            Parent.Attributes.Register("Equip_Head", default(IEquippable));
            Parent.Attributes.Register("Equip_Neck", default(IEquippable));
            Parent.Attributes.Register("Equip_Shoulders", default(IEquippable));
            Parent.Attributes.Register("Equip_Chest", default(IEquippable));
            Parent.Attributes.Register("Equip_Wrist", default(IEquippable));
            Parent.Attributes.Register("Equip_Hands", default(IEquippable));
            Parent.Attributes.Register("Equip_Belt", default(IEquippable));
            Parent.Attributes.Register("Equip_Legs", default(IEquippable));
            Parent.Attributes.Register("Equip_Feet", default(IEquippable));
        }
    }
}