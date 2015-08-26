using System;
using System.Collections.Generic;
using Assets.Code.Abstract;
using Assets.Code.Movement;
using UnityEngine;

namespace Assets.Code.Entities.Components {
    public class ObstructableUnitComponent : ObstructableComponent {
        
        public override void Init() {
            ObstructCollider = Parent.Attributes.Get<GameObject>("GameObject").GetComponent<CircleCollider2D>();
            if (ObstructCollider == null) GetOuttaHere();

            var circleCollider2D = (CircleCollider2D) ObstructCollider;
            if (circleCollider2D == null)
                throw new NullReferenceException("Circle Collider on ObstructableUnit was null");
            
            Parent.Attributes.Register("ObstructRadius", circleCollider2D.radius);

            base.Init();
        }

        public override void GetOuttaHere() {
            throw new Exception(
                    "Trying to init an ObstructableUnit Component, but init couldn't find the collider. " +
                    "Make sure a circle collider is attached to the child game object.... ");
        }

        protected override void OnSelected(Entity selectedEntity) {
            base.OnSelected(selectedEntity);

            Solid = false;
        }
        protected override void OnDeselected(Entity selectedEntity) {
            base.OnSelected(selectedEntity);

            Solid = true;
        }
    }
}