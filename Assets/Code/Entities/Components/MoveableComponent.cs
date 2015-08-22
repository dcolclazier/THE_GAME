using System.Collections;
using System.Collections.Generic;
using Assets.Code.Abstract;
using Assets.Code.Statics;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Code.Entities.Components {
    public class MoveableComponent : IComponent{
        private GameObjectComponent _myGoComponent;
        private GameObject _myGameObject;

        public List<string> Dependencies {
            get {
                return new List<string>() {
                    "GameObject",
                    "Pathfinding", //assumes selectable and moveable
                    "UnitStats",
                };
            }
        }

        public void OnUpdate() {

            var selected = Parent.Attributes.Get<bool>("CurrentlySelected");
            var pathActive = Parent.Attributes.Get<bool>("PathIsActive");
            Debug.Log("Currently Selected? " + selected );
            Debug.Log("Path active? " + pathActive );
            if (!selected) return;
            //if (Parent.Attributes.Get<bool>("CurrentlyPathing")) return;
            if (!pathActive) return;

            if (Input.GetMouseButtonDown(0)) {
                Debug.Log("Got here!");
                var target = Parent.Attributes.Get<Vector2>("CurrentPathTarget");
                var radius = Parent.Attributes.Get<float>("ObstructRadius");

                if (Vector2.Distance(target,UnityUtilites.MouseWorldPoint()) < radius) {
                    var gameObject = Parent.Attributes.Get<GameObject>("GameObject");
                    var goComponent = Parent.Attributes.Get<GameObjectComponent>("GameObjectComponent");
                    var path = Parent.Attributes.Get<List<Vector3>>("CurrentPath");
                    Parent.Attributes.Update("CurrentMoveSource", gameObject.transform.position.ToVector2());
                    foreach (var point in path) {
                        Parent.Attributes.Update("CurrentMoveTarget", point.ToVector2());
                        goComponent.StartUnityCoroutine(Move);
                    }
                }
            }
        }
        
        public IEnumerator Move() {
            var speed = Parent.Attributes.Get<float>("MovementSpeed"); //Commplex algorithm here
            var from = Parent.Attributes.Get<Vector2>("CurrentMoveSource");
            var to = Parent.Attributes.Get<Vector2>("CurrentMoveTarget");
            var step = (speed*.75f / Vector2.Distance(from,to)) * Time.fixedDeltaTime;
            float t = 0;
            while (t <= 1.0f) {
                t += step; // Goes from 0 to 1, incrementing by step each time
                _myGameObject.transform.position = Vector3.Lerp(from, to, t); // Move objectToMove closer to b
                yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
            }
            _myGameObject.transform.position = to;
            Parent.Attributes.Update("CurrentMoveSource", to);
        }
       
        public void Init() {
            _myGameObject = Parent.Attributes.Get<GameObject>("GameObject");
            _myGoComponent = Parent.Attributes.Get<GameObjectComponent>("GameObjectComponnet");

            Parent.Attributes.Register("CurrentMoveTarget",new Vector2());
            Parent.Attributes.Register("CurrentMoveSource",new Vector2());
            Messenger.AddListener("OnUpdate",OnUpdate);
        }

        public Entity Parent { get; set; }
    }


    public enum WeaponType {
        OneHand, TwoHand, Dual, ShortRange, LongRange
    }
}