using System.Collections.Generic;
using Assets.Code.Abstract;

namespace Assets.Code.Entities {
    public class Tree : EntityInitializer {
        protected override void Awake() {
            ComponentsToInit = new List<string>() {
                "GameObject",
                "ObstructableUnit",
            };
            NumericalAttributesToSet = new Dictionary<string, int>();
            StringAttributesToSet = new Dictionary<string, string>();
            EntitysToInclude = new List<string>();
            base.Awake();
        }
    }
}