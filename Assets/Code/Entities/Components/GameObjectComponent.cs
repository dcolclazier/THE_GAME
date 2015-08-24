using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Code.Abstract;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Entities.Components {
    public class GameObjectComponent : MonoBehaviour, IComponent {

        public GameObject Go { get; private set; }

        private Vector2 _entityPosition;
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
            _entityPosition = (Vector2)Go.transform.position + Go.GetComponent<Collider2D>().offset;
            if (Parent.Attributes.Get<Vector2>("Position") == _entityPosition) return;

            Parent.Attributes.Update("Position", _entityPosition);

            //var offset = Go.GetComponent<Collider2D>().offset;
            //Debug.Log(string.Format("offset: x: " + offset.x + " y: " + offset.y));
            Messenger.Broadcast("GameObjectMoved", Parent);
            Messenger.Broadcast("EntityMoved", Parent);
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