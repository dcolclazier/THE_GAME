using System;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Entities.Components {
    public sealed class SelectablePropComponent : SelectableComponent
    {
        public override void Init()
        {
            MyGameObject = Parent.Attributes.Get<GameObject>("GameObject");
            SelectCollider = MyGameObject.GetComponentInChildren<Collider2D>();
            if (SelectCollider == null) GetOuttaHere();

            SelectRadius = Parent.Attributes.Get<float>("ObstructRadius");
            
            base.Init();
        }
        protected override void GetOuttaHere()
        {
            throw new Exception(
                "Trying to init an SelectableProp Component, but init couldn't find the collider. " +
                "Make sure it is attached to a child Child GameObject named selectCollider ");
        }

        protected override void DrawSelectVisual() {
            var position = Parent.Attributes.Get<CircleCollider2D>("ObstructCollider").transform.position.ToVector2()+SelectCollider.offset;
            Debug.Log("Naa, you're not crazy...");
            SelectCircle.MakeCircle(position,SelectRadius,360);
            SelectCircle.Draw3DAuto();
        }

        protected override void OnDeselect() {
            
            
            base.OnDeselect();
        }
    }
}