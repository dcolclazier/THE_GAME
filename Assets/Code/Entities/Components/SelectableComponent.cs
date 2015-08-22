using System;
using System.Collections.Generic;
using Assets.Code.Abstract;
using Assets.Code.Movement;
using Assets.Code.Statics;
using UnityEngine;
using Vectrosity;

namespace Assets.Code.Entities.Components {
    public class SelectableComponent : IComponent, IToggle {
        public List<string> Dependencies {
            get { return new List<string>() {
                    "GameObject",
                }; 
            }
        }
        private void OnSelected(GameObject selectedObj) {
            
            //if the object selected was us, enabled = true - otherwise, enable = false.
            Enabled = selectedObj == Parent.Attributes.Get<GameObject>("GameObject");

            //Update the currentlySelected attribute and Enable/Disable the select circle
            Parent.Attributes.Update("CurrentlySelected", Enabled);
            _selectCircle.active = Enabled;

            //assign the radius to either the ObstructRadius, the SelectionRadius, or 0f for now - bad code
            var radius = 0f;
            var position = _selectCollider.transform.position;
            if (Parent.Attributes.Contains("ObstructRadius")) {
                radius = Parent.Attributes.Get<float>("ObstructRadius");
                position = Parent.Attributes.Get<Collider2D>("ObstructCollider").transform.position;
            }
            else if (Parent.Attributes.Contains("SelectionRadius")) radius = Parent.Attributes.Get<float>("SelectionRadius");
            _selectCircle.MakeCircle(position, radius, 360);
			_selectCircle.Draw3DAuto();

        }
     
        private void OnDeselect(Vector2 position) {
            if (!Enabled) return;
            var pathTarget = Parent.Attributes.Get<Vector2>("CurrentPathTarget");
            var radius = Parent.Attributes.Get<float>("ObstructRadius");
            if (Parent.Attributes.Get<bool>("PathIsActive") && Vector2.Distance(pathTarget, position) < radius) return;


            Enabled = false;
            _selectCircle.active = false;
            Parent.Attributes.Update("CurrentlySelected", Enabled);
            Messenger.Broadcast("GameObjectDeselected", Parent.Attributes.Get<GameObject>("GameObject"));
        }
        public void Init()
        {
            _selectCollider = Parent.Attributes.Get<GameObject>("GameObject").GetComponent<Collider2D>();
            if (_selectCollider == null) GetOuttaHere();

            Parent.Attributes.Register("SelectCollider", _selectCollider);
            Parent.Attributes.Register("SelectColliderType", NodeManager.GetColliderType(_selectCollider));
            Parent.Attributes.Register("CurrentlySelected", false);

            _selectCircle = new VectorLine("Select Circle", new Vector3[720], null, _lineThickness);
            _selectCircle.Draw3DAuto();
            VectorLine.canvas3D.sortingLayerName = "Select Circles";

            Messenger.AddListener<GameObject>("GameObjectSelected", OnSelected);
            Messenger.AddListener<Vector2>("GroundClicked", OnDeselect);
            
            
        }

        private float _lineThickness = 2.0f;
        private bool _enabled;
        public Entity Parent { get; set; }
        private Collider2D _selectCollider;
        private NodeManager.ColliderType _colliderType;
        private VectorLine _selectCircle;
        public bool Enabled { get; set; }
        private static void GetOuttaHere()
        {
            throw new Exception(
                    "Trying to init an Selectable Component, but init couldn't find the collider. " +
                    "Make sure it is attached to the game object itself, not the child ObstructCollider object. ");
        }
        public void OnUpdate()
        {

        }

    }
}