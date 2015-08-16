using UnityEngine;

namespace Assets.Code.Entities {
    public class Frisbee : Entity {
        protected override void Start() {
            Solid = true;
            base.Start();
        }
    }
}