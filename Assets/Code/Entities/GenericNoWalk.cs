using System.Collections.Generic;
using Assets.Code.Abstract;

namespace Assets.Code.Entities {
    public class GenericNoWalk : EntityInitializer {
        protected override void Awake() {
            
            ComponentsToInit = new List<string>() {
                "GameObject",
                "ObstructableProp",
            };
            NumericalAttributesToSet = new Dictionary<string, int>();
            StringAttributesToSet = new Dictionary<string, string>();
            EntitysToInclude = new List<string>();
            base.Awake();
        }
    }
}