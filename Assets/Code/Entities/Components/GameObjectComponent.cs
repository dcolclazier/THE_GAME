using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Code.Abstract;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Entities.Components {
    public class GameObjectComponent : MonoBehaviour, IComponent {

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
            Messenger.Broadcast("GameObjectMoved", Parent);
        }
        public void Init() {
            Go = Parent.Attributes.Get<GameObject>("GameObject");
            Parent.Attributes.Register("Position", Go.transform.position.ToVector2());

            Messenger.AddListener("OnUpdate",OnUpdate);
            Enabled = true;

            Parent.Attributes.Register("GameObjectComponent",this);
        }

        public void OnMessage() {
        }

        public void StartUnityCoroutine(Func<IEnumerator> moveRoutine) {

            StartCoroutine(moveRoutine());
        }
    }
}