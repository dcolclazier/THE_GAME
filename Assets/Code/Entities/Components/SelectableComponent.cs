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
        private void OnSelect() {
            Debug.Log("Running OnSelect");
            Enabled = true;
            _selectCircle.active = true;
            
            Parent.Attributes.Update("CurrentlySelected", Enabled);
            if (Parent.Attributes.Contains("ObstructRadius")) DrawSelected();

            //This is deprecated - use "EntitySelected" instead.
            Messenger.Broadcast("GameObjectSelected", Parent.Attributes.Get<GameObject>("GameObject"));
            Messenger.Broadcast("EntitySelected", Parent);

        }

        protected virtual void DrawSelected() {
            //var radius = Parent.Attributes.Get<float>("ObstructRadius");
            var position = Parent.Attributes.Get<Collider2D>("ObstructCollider").transform.position;
            _selectCircle.MakeCircle(position, SelectRadius, 360);
            //_selectCircle.MakeCircle(position, radius, 360);
            _selectCircle.Draw3DAuto();
        }

        protected virtual void OnDeselect() {
            Debug.Log("Running deselect.");
            Enabled = false;
            _selectCircle.active = false;

            Parent.Attributes.Update("CurrentlySelected", Enabled);

            //This is deprecated - use "EntitySelected" instead.
            Messenger.Broadcast("GameObjectDeselected", Parent.Attributes.Get<GameObject>("GameObject"));

            Messenger.Broadcast("EntityDeselected", Parent);
        }
        public virtual void Init() {
            if(SelectCollider == null) throw new NullReferenceException("You forgot to assign a collider to SelectCollider in the Init() method of your Selectable<whatever> component.");

            Parent.Attributes.Register("SelectCollider", SelectCollider);
            Parent.Attributes.Register("SelectColliderType", NodeManager.GetColliderType(SelectCollider));
            Parent.Attributes.Register("CurrentlySelected", false);

            //if we haven't defined the radius, do so now.
            if(Math.Abs(SelectRadius) < .0001) SelectRadius = GetRadius();

            _selectCircle = new VectorLine("Select Circle", new Vector3[720], null, _lineThickness);
            _selectCircle.Draw3DAuto();
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
            Debug.Log("Ok - we're dealing with a selectable...");
            var test = objClicked.transform.parent.gameObject.layer;
            if(test == UnityUtilites.ConvertBinaryFlag(LayerFlag.Units)) Debug.Log("You're getting somewhere..");
            //Debug.Log(string.Format("objectClicked : {0}, ",objClicked));
           // Debug.Log();
           // Debug.Log();


            if (objClicked.transform.parent.gameObject == SelectCollider.transform.gameObject) {

                OnSelect();
            }

            else {
                OnDeselect();
            }
            
        }

        private float _lineThickness = 2.0f;
        private bool _enabled;
        public Entity Parent { get; set; }
        protected Collider2D SelectCollider;
        private NodeManager.ColliderType _colliderType;
        protected VectorLine _selectCircle;
        protected GameObject MyGameObject;
        protected float SelectRadius;
        public bool Enabled { get; set; }
        protected virtual void GetOuttaHere() {
            throw new Exception(
                    "Trying to init an Selectable Component, but init couldn't find the collider. " +
                    "Make sure it is attached to the game object itself, not the child ObstructCollider object. ");
        }
        public virtual void OnUpdate()
        {

        }

    }
}