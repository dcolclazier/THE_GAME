using System;
using System.Collections.Generic;
using Assets.Code.Statics;
using UnityEngine;
using Vectrosity;

namespace Assets.Code.Abstract {
    public class SelectableComponent : IComponent, IToggle {
        //private BoxCollider2D _selectableCollider;

        private Collider2D _selectCollider;
        private NodeManager.ColliderType _colliderType;

        private VectorLine SelectCircle { get; set; }
        public Entity Parent { get; set; }
        private float _lineThickness = 2.0f;
        private bool _enabled;
        public List<string> Dependencies {
            get { return new List<string>() {
                    "GameObject",
            }; 
            }
        }
        public void Init()
        {

            //_selectableCollider = Parent.Attributes.Get<GameObject>("GameObject").GetComponent<CircleCollider2D>();
            _selectCollider = Parent.Attributes.Get<GameObject>("GameObject").GetComponent<Collider2D>();
            if (_selectCollider == null)
                throw new Exception(
                    "Trying to init an Selectable Component, but init couldn't find the collider. " +
                    "Make sure it is attached to the game object itself, not the child ObstructCollider object. ");

            Parent.Attributes.Register("SelectColliderType", NodeManager.GetColliderType(_selectCollider));
            Parent.Attributes.Register("CurrentlySelected", false);

            SelectCircle = new VectorLine("Select Circle", new Vector3[720], null, _lineThickness);
            SelectCircle.Draw3DAuto();
            VectorLine.canvas3D.sortingLayerName = "Select Circles";

            Messenger.AddListener<GameObject>("PlayerSelected", OnSelected);
            Messenger.AddListener("GroundClicked", OnDeselect);
        }
        private void OnSelected(GameObject selectedObj) {
            
            //if the object selected was us, enabled = true - otherwise, enable = false.
            Enabled = selectedObj == Parent.Attributes.Get<GameObject>("GameObject");
            Parent.Attributes.Update("CurrentlySelected", Enabled);
            SelectCircle.active = Enabled;

            var radius = 0f;
            if (Parent.Attributes.Contains("ObstructRadius")) radius = Parent.Attributes.Get<float>("ObstructRadius");
            else if (Parent.Attributes.Contains("SelectionRadius")) radius = Parent.Attributes.Get<float>("SelectionRadius");
            SelectCircle.MakeCircle(_selectCollider.transform.position, radius, 360);

        }
        public void OnUpdate() {
            
        }

        public void OnAwake() {
        }

        public void OnStart() {
        }

       

        private void OnDeselect() {
            if (!Enabled) return;
            
            Enabled = false;
            SelectCircle.active = false;
            Messenger.Broadcast("PlayerDeselected", Parent.Attributes.Get<GameObject>("GameObject"));
        }

        //public bool Enabled
        //{
        //    get { return _enabled; }
        //    set
        //    {
        //        var changed = value != _enabled;
        //        _enabled = value;
        //        if (changed) Toggle();
        //    }
        //}
        //private void Toggle()
        //{
        //    //SelectCircle.active = Enabled;

        //}


        public bool Enabled { get; set; }
    }
}