using System;
using System.Collections.Generic;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Abstract {
    public class ObstructableComponent : IComponent, IToggle {
        protected Collider2D ObstructCollider;
        private NodeManager.ColliderType _colliderType;
        protected List<Node> CollisionNodes;
        public Entity Parent { get; set; }
        public List<string> Dependencies {
            get {
                return new List<string>() {
                    "GameObject",
                };
            }
        }

        public bool Enabled {
            get { return Solid; }
            protected set {
                var changed = value != Solid;
                Solid = value;
                if (changed) {
                    UpdateListeners();
                }
            } 
        }
        protected bool Solid = true;

        private void UpdateListeners() {
            Parent.Attributes.Update("CurrentlyObstructing",Solid);
            Messenger.Broadcast(Enabled ? "ObstructionAdded" : "ObstructionRemoved", Parent);
        }


        public virtual void Init() {
            //get the obstruct collider from the game object and register it as an attribute for the entity.
            ObstructCollider = Parent.Attributes.Get<GameObject>("GameObject").GetComponentInChildren<Collider2D>();
            if (ObstructCollider == null) GetOuttaHere();


            Parent.Attributes.Register("ObstructCollider", ObstructCollider);
            Parent.Attributes.Register("ObstructColliderType", NodeManager.GetColliderType(ObstructCollider));
            Parent.Attributes.Register("CurrentlyObstructing", Solid);


            CollisionNodes = new List<Node>(EntityManager.GetNodesForEntity(Parent));
            Parent.Attributes.Register("CollisionNodes", CollisionNodes);

            Messenger.AddListener<GameObject>("PlayerSelected",OnSelected);
            Messenger.AddListener<GameObject>("PlayerDeselected",OnDeselected);
            Enabled = true;
        }

        public virtual void GetOuttaHere() {

            throw new Exception(
                    "Trying to init an Obstructable Component, but init couldn't find the collider. " +
                    "Make sure it is attached to the child game object, not the main game object. ");
        }

        protected void OnDeselected(GameObject deselectedObject) {
            if (deselectedObject != Parent.Attributes.Get<GameObject>("GameObject")) return;
            //ObstructCollider.enabled = true;
            Parent.Attributes.Update("CurrentlyObstructing", true);
            Enabled = true;
        }

        protected void OnSelected(GameObject selectedObject) {
            if (selectedObject != Parent.Attributes.Get<GameObject>("GameObject")) return;

            Parent.Attributes.Update("CurrentlyObstructing", false);
            //ObstructCollider.enabled = false;
            Enabled = false;
        }


        public virtual void OnUpdate() {
        }
        public virtual void OnAwake() {
        }

        public virtual void OnStart() {
        }

        
    }
}