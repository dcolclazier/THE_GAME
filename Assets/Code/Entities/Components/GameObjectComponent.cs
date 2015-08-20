using System.Collections.Generic;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Abstract {
    public class GameObjectComponent : MonoBehaviour, IComponent {

        public GameObject GameObject { get; private set; }

        private Vector2 _transformPosition;
        public GameObjectComponent(Entity parent, string playerName, Vector2 position) {
            Parent = parent;
            Enabled = true;
            GameObject = new GameObject(playerName);
            GameObject.transform.position = position;

            Parent.Attributes.Register("Position", position);
        }

        public Entity Parent { get; private set; }
        public bool Enabled { get; private set; }

        public List<string> Dependencies {
            get {
                return new List<string>() {
                    
                };
            }
        }

        public void OnUpdate() {
            if (!_transformPosition.Equals(gameObject.transform.position.ToVector2())) {
                _transformPosition = gameObject.transform.position;
                Messenger.Broadcast("GameObjectMoved", this);
            }
        }
        public void OnAwake() {
            
        }
        public void OnStart() {
        }
        public void OnMessage() {
        }
    }
}