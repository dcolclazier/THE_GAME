using System;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

namespace Assets.Code.Abstract {
    public class SelectableComponent : IComponent, IToggle {
        private CircleCollider2D _selectableCollider;
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
        private void OnSelected(GameObject selectedObj) {
            Debug.Log("I got clicked!");
            
            Enabled = selectedObj == Parent.Attributes.Get<GameObject>("GameObject");
            Debug.Log("Enabled = " + Enabled);
            
            Parent.Attributes.Update("CurrentlySelected", Enabled);
            Debug.Log("Enabled after update = " + Enabled);
            Debug.Log("Collider radius: " + _selectableCollider.radius);
            SelectCircle.MakeCircle(_selectableCollider.transform.position, _selectableCollider.radius, 360);
            Debug.Log("MakeCircleCalled");
            SelectCircle.active = true;
            Debug.Log("Circle Active set to true");
        }
        public void OnUpdate() {
            
        }

        public void OnAwake() {
        }

        public void OnStart() {
        }

        public void Init() {
            _selectableCollider = Parent.Attributes.Get<GameObject>("GameObject").GetComponentInChildren<CircleCollider2D>();
            if (_selectableCollider == null)
                throw new Exception(
                    "Trying to init an Obstructable Component, but init couldn't find the collider. " +
                    "Make sure it is attached to the child game object, not the child collider object. ");
            
            Parent.Attributes.Register("SelectCollider", _selectableCollider);
            Parent.Attributes.Register("SelectRadius", _selectableCollider.radius);
            Parent.Attributes.Register("CurrentlySelected", false);


            SelectCircle = new VectorLine("Select Circle", new Vector3[720], null, _lineThickness);
            SelectCircle.Draw3DAuto();

            Messenger.AddListener<GameObject>("PlayerSelected", OnSelected);
            Messenger.AddListener("GroundClicked", OnDeselect);
        }

        private void OnDeselect() {
            if (Enabled) {
                Enabled = false;
                Debug.Log("I was unclicked! Setting SelectCirle from " + SelectCircle.active + " to " + Enabled );
                SelectCircle.active = false;
            }
        }

        public void OnMessage() {

        }
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                var changed = value != _enabled;
                _enabled = value;
                if (changed) Toggle();
            }
        }
        private void Toggle()
        {
            //SelectCircle.active = Enabled;

        }
       
        
    }
}