using System;
using UnityEngine;

namespace Assets.Code.Entities.Components {
    public class ObstructablePropComponent : ObstructableComponent {
        public override void Init() {
            ObstructCollider = Parent.Attributes.Get<GameObject>("GameObject").GetComponent<Collider2D>();
            if (ObstructCollider == null) GetOuttaHere();
        }
        public override void GetOuttaHere() {
            throw new Exception(
                "Trying to init an ObstructableProp Component, but init couldn't find the collider. " +
                "Did you attach a collider to the main game object?");
        }
    }
}