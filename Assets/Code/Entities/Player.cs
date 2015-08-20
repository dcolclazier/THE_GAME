using System.Collections.Generic;
using Assets.Code.Abstract;

namespace Assets.Code.Entities {
    public class Player : EntityInitializer {
        protected override void Awake() {
            ComponentsToInit = new List<string>() {
                "GameObject",
                "ObstructableUnit",
                "Selectable",
                "Pathfinding"
            };
            base.Awake();
        }
    }
}