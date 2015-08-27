using System;
using System.Collections.Generic;
using Assets.Code.Abstract;

namespace Assets.Code.Entities {
	public class Priest : EntityInitializer {
		protected override void Awake() {
			ComponentsToInit = new List<string>() {
				"GameObject",
				"UnitStats",
				"ObstructableUnit",
				"Selectable",
				"Pathfinding",
				"Moveable",
				//"AttackableComponent",
				"CanWieldAll",
				"FullyEquipped",
				"ArcAttack",

			};
			EntitysToInclude = new List<string>() {
				//"BasicWoodenSword",
			};
			NumericalAttributesToSet = new Dictionary<string, int>() {
				{"Strength", 5},
				{"Agility", 2}
			};
			StringAttributesToSet = new Dictionary<string, string>() {
				{"Name", "Betty"},
			};
			
			base.Awake();
			
			//entity's he should be wielding at spawn, and where they go
			
		}
		
	}
}