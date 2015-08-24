using System;
using System.Collections.Generic;
using Assets.Code.Abstract;
using Assets.Code.Movement;
using Assets.Code.Statics;
using UnityEngine;
using Vectrosity;

namespace Assets.Code.Entities.Components {
    public abstract class SelectableComponent : IComponent, IToggle {
        public List<string> Dependencies {
            get { return new List<string>() {
                    "GameObject",
                }; 
            }
        }
        protected virtual void OnSelect() {
            //Debug.Log("Running OnSelect");
            Enabled = true;
            SelectCircle.active = true;
            
            Parent.Attributes.Update("CurrentlySelected", Enabled);
            DrawSelectVisual();

            //This is deprecated - use "EntitySelected" instead.
            Messenger.Broadcast("GameObjectSelected", Parent.Attributes.Get<GameObject>("GameObject"));
            Messenger.Broadcast("EntitySelected", Parent);

        }

        protected virtual void DrawSelectVisual() {
            throw new NotImplementedException("You HAVE to override DrawSelectVisual() in your child selectable!!!");
        }

        protected virtual void OnDeselect() {
            Enabled = false;
            SelectCircle.active = false;

            Parent.Attributes.Update("CurrentlySelected", Enabled);

            //This is deprecated - use "EntitySelected" instead.
            Messenger.Broadcast("GameObjectDeselected", Parent.Attributes.Get<GameObject>("GameObject"));

            Messenger.Broadcast("EntityDeselected", Parent);
        }
        public virtual void Init() {
            Playername = Parent.Attributes.Get<string>("Name");

            Debug.Log("Selectable Init for " + Playername);
            if(SelectCollider == null) throw new NullReferenceException("You forgot to assign a collider to SelectCollider in the Init() method of your Selectable<whatever> component.");

            Parent.Attributes.Register("SelectCollider", SelectCollider);
            Parent.Attributes.Register("SelectColliderType", NodeManager.GetColliderType(SelectCollider));
            Parent.Attributes.Register("CurrentlySelected", false);

            //if we haven't defined the radius, do so now.
            if (Math.Abs(SelectRadius) < .0001) {
                
                SelectRadius = GetRadius();
            }

            SelectCircle = new VectorLine("Select Circle", new Vector3[720], null, _lineThickness);
            SelectCircle.Draw3DAuto();
            VectorLine.canvas3D.sortingLayerName = "Select Circle";
            
            Messenger.AddListener<LayerFlag, RaycastHit2D>("LeftMouseDown",OnLeftMouseDown);
        }

        private float GetRadius() {
            var gameObject = Parent.Attributes.Get<GameObject>("GameObject");
            var sprite = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
            if(sprite == null) throw new NullReferenceException("You need a sprite renderer attached to the child game object...");
            var coords = UnityUtilites.SpriteLocalToWorld(sprite, gameObject);
            return (coords[0].x + coords[1].x)/2;

        }

        private void OnLeftMouseDown(LayerFlag layer, RaycastHit2D objClicked) {
            if (objClicked.transform.gameObject.layer != UnityUtilites.ConvertBinaryFlag(LayerFlag.Selection)) return;
            if (objClicked.transform.parent.gameObject == SelectCollider.transform.gameObject) OnSelect();
            else OnDeselect();
           
            
        }

        private float _lineThickness = 2.0f;
        private bool _enabled;
        private NodeManager.ColliderType _colliderType;
        protected string Playername { get; set; }
        public Entity Parent { get; set; }
        protected Collider2D SelectCollider { get; set; }
        protected VectorLine SelectCircle { get; private set; }
        protected GameObject MyGameObject { get; set; }
        protected float SelectRadius { get; set; }
        public bool Enabled { get; private set; }
        protected virtual void GetOuttaHere() {
            throw new Exception("Trying to init an Selectable Component, but init couldn't find the collider. ");
        }
        public virtual void OnUpdate()
        {

        }

    }
}