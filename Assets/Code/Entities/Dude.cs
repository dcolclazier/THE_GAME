using System.Collections.Generic;
using Assets.Code.Abstract;

namespace Assets.Code.Entities {
    public class Dude : EntityInitializer
    {
        protected override void Awake()
        {
            ComponentsToInit = new List<string>() {
                "GameObject",
				"UnitStats",
                "ObstructableProp",
                "SelectableProp",
                "AttackableComponent",

            };
            NumericalAttributesToSet = new Dictionary<string, int>()
			{
				{"Strength", 1},
				{"Actions", 3},
			};
            StringAttributesToSet = new Dictionary<string, string>()
			{
				{"Team", "Enemy"},

			};
            EntitysToInclude = new List<string>();
            base.Awake();
        }
    }
}