using System.Collections.Generic;
using Assets.Code.Abstract;

namespace Assets.Code.Entities {
    public class Statue : EntityInitializer {
        protected override void Awake() {
            ComponentsToInit = new List<string>() {
                "GameObject",
                "Obstructable"
            };
            base.Awake();
        }
    }
}