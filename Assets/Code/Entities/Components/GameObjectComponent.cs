using System.Collections.Generic;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Abstract {
    public class GameObjectComponent : IComponent {

        public GameObject Go { get; private set; }

        private Vector2 _transformPosition;
        public Entity Parent { get; set; }
        public bool Enabled { get; private set; }

        Entity IComponent.Parent {
            get { return Parent; }
            set { Parent = value; }
        }

        public List<string> Dependencies {
            get {
                return new List<string>() {
                
                };
            }
        }

        public void OnUpdate() {
            if (_transformPosition.Equals(Go.transform.position.ToVector2())) return;
            
            _transformPosition = Go.transform.position;
            Parent.Attributes.Update("Position", _transformPosition);
            Messenger.Broadcast("GameObjectMoved", this);
        }
        public void OnAwake() {
            
        }
        public void OnStart() {
        }

        public void Init() {
            Enabled = true;
            if (Parent == null) return;
            Go = Parent.Attributes.Get<GameObject>("GameObject");
            if (Go != null) Parent.Attributes.Register("Position", Go.transform.position.ToVector2());
        }

        public void OnMessage() {
        }
    }
}