using System;
using UnityEngine;

namespace Assets.Code.Entities.Components {
    public sealed class SelectableUnitComponent : SelectableComponent
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
                "Trying to init an SelectablUnit Component, but init couldn't find the collider. " +
                "Make sure it is attached to the game object itself, not the child ObstructCollider object. ");
        }
        protected override void DrawSelectVisual()
        {
            //Debug.Log("Running Unit Draw....");
            var position = Parent.Attributes.Get<Vector2>("Position");
            //var position = Parent.Attributes.Get<CircleCollider2D>("ObstructCollider").transform.position.ToVector2() + SelectCollider.offset;
            SelectCircle.MakeCircle(position, SelectRadius, 360);
            SelectCircle.Draw3DAuto();
        }

       
    }
}