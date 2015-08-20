using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

namespace Assets.Code.Abstract {
    public class SelectableComponent : IComponent, IToggle {
        private CircleCollider2D _selectableCollider;

        public List<string> Dependencies {
            get { return new List<string>() {
                    "GameObject",
                    "SelectCollider"
            }; 
            }
        }
        public SelectableComponent(Entity parent)
        {
            Parent = parent;

            _selectableCollider = Parent.Components.Get<CircleCollider2D>("SelectCollider");
           
            Parent.Attributes.Register("Radius", _selectableCollider.radius);
            Parent.Attributes.Register("SelectCollider", _selectableCollider);
            
            SelectCircle = new VectorLine("Select Circle", new Vector3[720], null, _lineThickness);
            SelectCircle.Draw3DAuto();
            
            Messenger.AddListener<GameObject>("PlayerSelected", OnSelected);
        }
        
        private void OnSelected(GameObject selectedObj) {
            Enabled = selectedObj == Parent.Components.Get<GameObject>("GameObject");
            if(Parent.Attributes.Contains("CurrentlySelected"))
            Parent.Attributes.Update("CurrentlySelected", Enabled);
        }
        public void OnUpdate() {
            
        }

        public void OnAwake() {
        }

        public void OnStart() {
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
            SelectCircle.active = Enabled;

        }
        private VectorLine SelectCircle { get; set; }
        public Entity Parent { get; private set; }
        private readonly float _lineThickness = 2.0f;
        private bool _enabled;
        
    }
}