using System;
using System.Collections.Generic;
using Assets.Code.Abstract;

namespace Assets.Code.Entities {
    public class Player : EntityInitializer {
        protected override void Awake() {
            ComponentsToInit = new List<string>() {
                "GameObject",
                "UnitStats",
                "ObstructableUnit",
                "SelectableUnit",
                "Pathfinding",
                "Moveable",
                //"AttackableComponent",
                "FullyEquipped",
                "CanWieldAll",
				"BasicAttack"
            };
            EntitysToInclude = new List<string>() {
                //"BasicWoodenSword",
            };
            NumericalAttributesToSet = new Dictionary<string, int>() {
                 {"Strength", 18},
                 {"Agility", 2}
            };
            StringAttributesToSet = new Dictionary<string, string>() {
                {"Name", "David"},
            };
              
            base.Awake();

            
        }

    }
}