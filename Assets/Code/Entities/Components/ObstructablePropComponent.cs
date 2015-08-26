using System;
using UnityEngine;

namespace Assets.Code.Entities.Components {
    public class ObstructablePropComponent : ObstructableComponent {
        public override void Init() {
            ObstructCollider = Parent.Attributes.Get<GameObject>("GameObject").GetComponent<Collider2D>();
            if (ObstructCollider == null) GetOuttaHere();

            //var circleCollider2D = ObstructCollider as CircleCollider2D;
            //BoxCollider2D boxCollider = null;
            //if (circleCollider2D == null) {
            //    boxCollider = ObstructCollider as BoxCollider2D;
            //    if(boxCollider == null) throw new Exception("You're doin this wrong...");
            //}
            //if(circleCollider2D) Parent.Attributes.Register("ObstructRadius", circleCollider2D.radius);
            //if(boxCollider) Parent.Attributes.Register("ObstructRadius",box.);
            base.Init();
        }
        public override void GetOuttaHere() {
            throw new Exception(
                "Trying to init an ObstructableProp Component, but init couldn't find the collider. " +
                "Did you attach a collider to the main game object?");
        }
    }

    
}