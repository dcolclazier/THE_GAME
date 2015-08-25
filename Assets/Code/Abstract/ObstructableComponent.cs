using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Abstract;
using Assets.Code.Movement;
using Assets.Code.Statics;
using UnityEngine;

/*Notes about component:
* Setting Enabled will update entity's "CurrentlyObstructing" attribute accordingly
* If you you set Enabled to something it wasn't, it will broadcast a 
* "ObstructionAdded" and a "ObstructionRemoved" event including the entity object.
*/
        
namespace Assets.Code.Entities.Components {
    public abstract class ObstructableComponent : IComponent, IToggle {

        public List<string> Dependencies {
            get {
                return new List<string>() {
                    "GameObject",
                };
            }
        }
        protected virtual void OnSelected(Entity selectedEntity) {
            if (selectedEntity != Parent) return;
        }

        protected virtual void OnDeselected(Entity deselectedObject) {
            if (deselectedObject != Parent) return;
        }

        public virtual void OnUpdate() {
            if (Solid == Enabled) return;
            
            Enabled = Solid;
            Parent.Attributes.Update("CurrentlyObstructing", Solid);
            Messenger.Broadcast(Enabled ? "ObstructionAdded" : "ObstructionRemoved", Parent);
        }
        public virtual void Init()
        {
            CollisionNodes = new List<Node>();
            Playername = Parent.Attributes.Get<string>("Name");

            Debug.Log("Obstructable Init for " + Playername);

            Parent.Attributes.Register("ObstructCollider", ObstructCollider);
            Parent.Attributes.Register("ObstructColliderType", NodeManager.GetColliderType(ObstructCollider));
            Parent.Attributes.Register("CurrentlyObstructing", Solid);

            UpdateCollisionNodes();
            
            Messenger.AddListener<Entity>("EntitySelected", OnDeselected);
            Messenger.AddListener<Entity>("EntityDeselected", OnDeselected);
            Messenger.AddListener<Entity>("EntityMoved",EntityMoved);
            Messenger.AddListener("OnUpdate", OnUpdate);
            
            Solid = true;
        }

        private void EntityMoved(Entity arg1) {
            UpdateCollisionNodes();
        }

        private void UpdateCollisionNodes() {
            
            //if (CollisionNodes == null) CollisionNodes = new List<Node>();

            CollisionNodes = new List<Node>(EntityManager.GetNodesForEntity(Parent).ToList());
            Parent.Attributes.RegisterOrUpdate("CollisionNodes", CollisionNodes);
        }

        public bool Enabled { get; private set; }
        protected bool Solid;

        //No logic below here. 
        protected Collider2D ObstructCollider;
        private NodeManager.ColliderType _colliderType;
        protected List<Node> CollisionNodes;
        private bool _obstructionChanged;
        protected string Playername { get; private set; }
        public Entity Parent { get; set; }
        public virtual void GetOuttaHere() {

            throw new Exception(
                    "Trying to init an Obstructable Component, but init couldn't find the collider. " +
                    "Make sure it is attached to the child game object, not the main game object. ");
        }
    }
}