using System;
using System.Collections.Generic;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Abstract {
    public class ObstructableUnitComponent : ObstructableComponent {
        
        public override void Init()
        {
            //get the obstruct collider from the game object and register it as an attribute for the entity.
            ObstructCollider = Parent.Attributes.Get<GameObject>("GameObject").GetComponentInChildren<CircleCollider2D>();
            if (ObstructCollider == null) GetOuttaHere();

            Parent.Attributes.Register("ObstructCollider", ObstructCollider);
            Parent.Attributes.Register("ObstructColliderType", NodeManager.GetColliderType(ObstructCollider));
            Parent.Attributes.Register("ObstructRadius", ((CircleCollider2D)ObstructCollider).radius);
            Parent.Attributes.Register("CurrentlyObstructing", Solid);
            
            CollisionNodes = new List<Node>(EntityManager.GetNodesForEntity(Parent));
            Parent.Attributes.Register("CollisionNodes", CollisionNodes);

            Messenger.AddListener<GameObject>("PlayerSelected", OnSelected);
            Messenger.AddListener<GameObject>("PlayerDeselected", OnDeselected);
            Enabled = true;
        }

        public override void GetOuttaHere() {
            throw new Exception(
                    "Trying to init an ObstructableUnit Component, but init couldn't find the collider. " +
                    "Make sure a circle collider is attached to the game object(It's the same collider as" +
                    "the selection circle.... ");
        }



      
    }
}