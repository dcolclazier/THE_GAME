using UnityEngine;

namespace Assets.Code.Entities {
    public class Frisbee : Entity {
        //attached to prefab, this object is.
        protected override void Start() {
            Solid = true;
            base.Start();
        }
    }
}