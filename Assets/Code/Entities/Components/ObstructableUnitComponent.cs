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

            Parent.Attributes.Register("ObstructCollider", ObstructCollider);
            Parent.Attributes.Register("ObstructColliderType", NodeManager.GetColliderType(ObstructCollider));
            Parent.Attributes.Register("CurrentlyObstructing", Solid);
            
            CollisionNodes = new List<Node>(EntityManager.GetNodesForEntity(Parent));
            Parent.Attributes.Register("CollisionNodes", CollisionNodes);

            Messenger.AddListener<GameObject>("GameObjectSelected", OnSelected);
            Messenger.AddListener<GameObject>("GameObjectDeselected", OnDeselected);
            Messenger.AddListener("OnUpdate",OnUpdate);
            Solid = true;
        }

        public override void GetOuttaHere() {
            throw new Exception(
                    "Trying to init an ObstructableUnit Component, but init couldn't find the collider. " +
                    "Make sure a circle collider is attached to the game object - It's the same collider as" +
                    "the selection circle.... ");
        }



      
    }
}