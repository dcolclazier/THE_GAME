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

            Enabled = true;
            _selectCircle.active = true;
            Parent.Attributes.Update("CurrentlySelected", Enabled);

            //assign the radius to either the ObstructRadius, the SelectionRadius, or 0f for now - bad code

            var radius = 0f;
            var position = _selectCollider.transform.position;
            if (Parent.Attributes.Contains("ObstructRadius"))
            {
                radius = Parent.Attributes.Get<float>("ObstructRadius");
                position = Parent.Attributes.Get<Collider2D>("ObstructCollider").transform.position;
            }
            else if (Parent.Attributes.Contains("SelectionRadius")) radius = Parent.Attributes.Get<float>("SelectionRadius");
            _selectCircle.MakeCircle(position, radius, 360);
			_selectCircle.Draw3DAuto();
            Messenger.Broadcast("GameObjectSelected", Parent.Attributes.Get<GameObject>("GameObject"));
        }
     
        private void OnDeselect() {
            //if (!Enabled) return;
            //var pathTarget = Parent.Attributes.Get<Vector2>("CurrentPathTarget");
            //var radius = Parent.Attributes.Get<float>("ObstructRadius");
            //if (Parent.Attributes.Get<bool>("PathIsActive") && Vector2.Distance(pathTarget, position) < radius) return;
            
            Enabled = false;
            _selectCircle.active = false;

            Parent.Attributes.Update("CurrentlySelected", Enabled);
            Messenger.Broadcast("GameObjectDeselected", Parent.Attributes.Get<GameObject>("GameObject"));
        }
        public void Init() {
            _myGameObject = Parent.Attributes.Get<GameObject>("GameObject");
            _selectCollider = _myGameObject.GetComponentInChildren<Collider2D>();
            if (_selectCollider == null) GetOuttaHere();

            Parent.Attributes.Register("SelectCollider", _selectCollider);
            Parent.Attributes.Register("SelectColliderType", NodeManager.GetColliderType(_selectCollider));
            Parent.Attributes.Register("CurrentlySelected", false);

            _selectCircle = new VectorLine("Select Circle", new Vector3[720], null, _lineThickness);
            _selectCircle.Draw3DAuto();
            VectorLine.canvas3D.sortingLayerName = "Select Circle";

            

            //Messenger.AddListener<GameObject>("GameObjectSelected", OnSelected);
            //Messenger.AddListener<Vector2>("GroundClicked", OnDeselect);
            
            Messenger.AddListener<LayerFlag, RaycastHit2D>("LeftMouseDown",OnLeftMouseDown);
        }

        private void OnLeftMouseDown(LayerFlag layer, RaycastHit2D objClicked) {
            Debug.Log("Got here...");
            if (_myGameObject.layer != UnityUtilites.ConvertBinaryFlag(layer)) return;

            if (objClicked.transform.gameObject != _myGameObject) {
                OnDeselect();
            }
            else {
                OnSelected(objClicked.transform.gameObject);
            }

            Debug.Log("A selectable component was clicked on!");
            //if(myGO != objClicked.)
        }

        private float _lineThickness = 2.0f;
        private bool _enabled;
        public Entity Parent { get; set; }
        private Collider2D _selectCollider;
        private NodeManager.ColliderType _colliderType;
        private VectorLine _selectCircle;
        private GameObject _myGameObject;
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