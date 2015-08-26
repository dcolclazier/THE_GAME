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
        private Vector2 _offset;
        public Entity Parent { get; set; }

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

        public void Update() {
           if(EntityPositionChanged()) UpdatePosition();
        }

        private bool EntityPositionChanged() {
            return Parent.Attributes.Get<Vector2>("Position") != (Vector2)Go.transform.position + _offset;
        }

        private void UpdatePosition() {
            
            var first = Parent.Attributes.Get<Vector2>("Position") == Vector2.zero;
            Parent.Attributes.Update("Position", Go.transform.position.ToVector2() + _offset);
            if (!first) Messenger.Broadcast("EntityMoved", Parent);
        }

        public void OnUpdate() {
            throw new NotImplementedException();
        }

        public void Init() {
            Go = Parent.Attributes.Get<GameObject>("GameObject");
            _offset = Go.GetComponent<Collider2D>().offset;

            Parent.Attributes.Register("Position", Go.transform.position.ToVector2() + _offset);
            Parent.Attributes.Register("GameObjectComponent",this);
        }

        
        
    }
}