using System;
using System.Collections.Generic;
using Assets.Code.Statics;
using UnityEngine;
/*Notes about component:
* Setting Enabled will update entity's "CurrentlyObstructing" attribute accordingly
* If you you set Enabled to something it wasn't, it will broadcast a 
* "ObstructionAdded" and a "ObstructionRemoved" event including the entity object.
*/
        
namespace Assets.Code.Abstract {
    public class ObstructableComponent : IComponent, IToggle {

        public List<string> Dependencies {
            get {
                return new List<string>() {
                    "GameObject",
                };
            }
        }
        protected void OnSelected(GameObject selectedObject) {
            if (selectedObject != Parent.Attributes.Get<GameObject>("GameObject")) return;
            Debug.Log("I was selected!");
            Enabled = false;
        }

        protected void OnDeselected(GameObject deselectedObject) {
            if (deselectedObject != Parent.Attributes.Get<GameObject>("GameObject")) return;
            Debug.Log("I was deselected!");
            Enabled = true;
        }

        public virtual void OnUpdate() {
            if (_obstructionChanged) {
                ObstructCollider.enabled = Solid;
                Parent.Attributes.Update("CurrentlyObstructing", Solid);
                Messenger.Broadcast(Enabled ? "ObstructionAdded" : "ObstructionRemoved", Parent);
            }
        }
        public virtual void Init()
        {
            //get the obstruct collider from the game object and register it as an attribute for the entity.
            ObstructCollider = Parent.Attributes.Get<GameObject>("GameObject").GetComponentInChildren<Collider2D>();
            if (ObstructCollider == null) GetOuttaHere();

            Parent.Attributes.Register("ObstructCollider", ObstructCollider);
            Parent.Attributes.Register("ObstructColliderType", NodeManager.GetColliderType(ObstructCollider));
            Parent.Attributes.Register("CurrentlyObstructing", Solid);

            CollisionNodes = new List<Node>(EntityManager.GetNodesForEntity(Parent));
            Parent.Attributes.Register("CollisionNodes", CollisionNodes);

            Messenger.AddListener<GameObject>("GameObjectSelected", OnSelected);
            Messenger.AddListener<GameObject>("GameObjectDeselected", OnDeselected);
            Messenger.AddListener("OnUpdate", OnUpdate);
            Enabled = true;
        }
        public bool Enabled {
            get {
                return Solid;
            }
            protected set {
                _obstructionChanged = value != Solid;
                Solid = value;
            }
        }
        protected bool Solid = true;

        //No logic below here. 
        protected Collider2D ObstructCollider;
        private NodeManager.ColliderType _colliderType;
        protected List<Node> CollisionNodes;
        private bool _obstructionChanged;
        public Entity Parent { get; set; }
        public virtual void GetOuttaHere()
        {

            throw new Exception(
                    "Trying to init an Obstructable Component, but init couldn't find the collider. " +
                    "Make sure it is attached to the child game object, not the main game object. ");
        }
    }
}