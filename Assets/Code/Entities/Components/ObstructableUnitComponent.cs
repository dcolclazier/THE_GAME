using System;
using System.Collections.Generic;
using Assets.Code.Abstract;
using Assets.Code.Movement;
using Assets.Code.Statics;
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