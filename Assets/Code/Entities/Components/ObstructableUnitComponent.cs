using System;
using System.Collections.Generic;
using Assets.Code.Abstract;
using Assets.Code.Movement;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Entities.Components {
    public class ObstructableUnitComponent : ObstructableComponent {
        
        public override void Init()
        {
            ObstructCollider = Parent.Attributes.Get<GameObject>("GameObject").GetComponentInChildren<CircleCollider2D>();
            if (ObstructCollider == null) GetOuttaHere();
            Parent.Attributes.Register("ObstructRadius", ((CircleCollider2D)ObstructCollider).radius);

            base.Init();
            
            //Parent.Attributes.Register("ObstructCollider", ObstructCollider);
            //Parent.Attributes.Register("ObstructColliderType", NodeManager.GetColliderType(ObstructCollider));
            //Parent.Attributes.Register("CurrentlyObstructing", true);
            
            //CollisionNodes = new List<Node>(EntityManager.GetNodesForEntity(Parent));
            //Parent.Attributes.Register("CollisionNodes", CollisionNodes);
            
            //Messenger.AddListener<Entity>("EntitySelected", OnDeselected);
            //Messenger.AddListener<Entity>("EntityDeselected", OnDeselected);


            //Messenger.AddListener("OnUpdate",OnUpdate);
            //Solid = true;
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