using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Assets.Code.Abstract;
using Assets.Code.Movement;
using Assets.Code.Statics;
using JetBrains.Annotations;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Code.Entities.Components {
    public class MoveableComponent : IComponent{
        private GameObjectComponent _myGoComponent;
        private GameObject _myGameObject;
        private Vector2 _myOffset;
        private bool _gotthere;
        private float _speed;
        private List<Vector3> _path;
        private Vector3 destination;
        private Vector2 _from;
        private Vector3 _to;
        private bool _moving;

        public List<string> Dependencies {
            get {
                return new List<string>() {
                    "GameObject",
                    "Pathfinding", //assumes selectable and moveable
                    "UnitStats",
                };
            }
        }

     
        private Vector2 _currentMoveTarget;
        public IEnumerator MoveFunction(Vector2 next, Vector2 _from)
        {
            while (true)
            {

                _speed = Parent.Attributes.Get<float>("MovementSpeed"); //Commplex algorithm here
                var step = (_speed * .5f / Vector2.Distance(_from, next)) * Time.fixedDeltaTime;
                float t = 0;
                while (t <= 1.0f) {
                    t += step; // Goes from 0 to 1, incrementing by step each time
                    _myGameObject.transform.position = Vector3.Lerp(_from, next, t).ToVector2() - _myOffset; // Move objectToMove closer to b
                    yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
                }

                // If the object has arrived, stop the coroutine
                if (MyPosition() == next) {
                    if (_path.Any()) Messenger.Broadcast("NotDoneYet", _path, Parent);
                    else Messenger.Broadcast("DoneMoving!", Parent);
                    yield break;
                }

                // Otherwise, continue next frame
                yield return null;
            }
            yield break;
        }
        public void OnUpdate() {
            throw new System.NotImplementedException();
        }


        public void Init() {
            
            _myGameObject = Parent.Attributes.Get<GameObject>("GameObject");
            _myOffset = Parent.Attributes.Get<Collider2D>("ObstructCollider").offset;
            _speed = Parent.Attributes.Get<float>("MovementSpeed");
            _myGoComponent = Parent.Attributes.Get<GameObjectComponent>("GameObjectComponent");
            
            Messenger.AddListener<LayerFlag,RaycastHit2D>("LeftMouseDown",OnLeftMouseDown);
            Messenger.AddListener<List<Vector3>,Entity>("NotDoneYet",MoveNext);
            Messenger.AddListener<Entity>("DoneMoving!", Cleanup);
            
        }

        private void Cleanup(Entity entity) {
            Debug.Log("Done! You should cleanup here.");
            Messenger.Broadcast("ResetPath",Parent);
            _myGoComponent.StopAllCoroutines();
            _moving = false;
            _path = null;

        }


        private void OnLeftMouseDown(LayerFlag arg1, RaycastHit2D arg2) {
            if (!Parent.Attributes.Get<bool>("CurrentlySelected")) return;
            if (!Parent.Attributes.Get<bool>("PathIsActive")) return;
            if (!ClickIsInRange()) return;
            if (_moving) return;

            StartMoving();
        }

        private void StartMoving() {
            _path = Parent.Attributes.Get<List<Vector3>>("CurrentPath");
            _moving = true;
            Debug.Log("Starting move - current waypoint count: " + _path.Count);
            _path.RemoveAt(_path.Count - 1); //remove source location from path
            MoveNext(_path,Parent);
            //Messenger.Broadcast("NotDoneYet");
        }

        private void MoveNext(List<Vector3> path, Entity entity) {
            if (Parent != entity) return;
            Debug.Log("MoveNext called.");
            _moving = true;
            if (path == null || path.Count == 0) return;
            var next = path[path.Count - 1];
            path.Remove(next);
            Debug.Log("Starting a coroutine...");
            _myGoComponent.StartCoroutine(MoveFunction(next, MyPosition()));
        }


        private bool ClickIsInRange() {
            var target = Parent.Attributes.Get<Vector2>("CurrentPathTarget");
            var radius = Parent.Attributes.Get<float>("ObstructRadius");
            var distance = Vector2.Distance(target, UnityUtilites.MouseWorldPoint());
            return distance < radius;
        }
        private Vector2 MyPosition()
        {
            return (Vector2)_myGameObject.transform.position + Parent.Attributes.Get<Collider2D>("ObstructCollider").offset;
        }
        public Entity Parent { get; set; }
    }
}





/*
        //public IEnumerator Move()
        //{
        //    Debug.Log("Running Move CoRoutine.");
        //    Debug.Log("About to move from " + _from.x + ":::" + _from.y + " to " + _to.x + ":::" + _to.y);

        //    _speed = Parent.Attributes.Get<float>("MovementSpeed"); //Commplex algorithm here
        //    var step = (_speed * .25f / Vector2.Distance(_from, _to)) * Time.fixedDeltaTime;
        //    float t = 0;
        //    _gotthere = false;
        //    while (t <= 1.0f)
        //    {
        //        t += step; // Goes from 0 to 1, incrementing by step each time
        //        _myGameObject.transform.position = Vector3.Lerp(_from, _to, t).ToVector2() - _myOffset; // Move objectToMove closer to b
        //        yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        //    }
        //    //Messenger.Broadcast("GotThere");
        //}
 
        protected IEnumerator Move()
        {
            while(true)
            {
                if (_path.Count == 0) break; // end of path
                moveTowards(destination);
                yield return 0;
            }
        }
        protected IEnumerator Path()
        {
            while(true) {
                destination = _path[_path.Count - 1]; // Next Point
                if (_path.Count == 0) break; // end of path
                _path.RemoveAt(_path.Count-1);
                while (!CloseEnough(destination)) yield return 0;
            }
        }

        private bool CloseEnough(Vector2 destination) {

            var xDiff = MyPosition().x - destination.x;
            var yDiff = MyPosition().y - destination.y;
            return (xDiff + yDiff < .1);
            
            return false;
        }
        protected void moveTowards(Vector3 to)
        {
            Debug.Log("Moving...");
            var lookRotation = Quaternion.LookRotation((to - (Vector3)MyPosition()).normalized);
            _myGameObject.transform.position += lookRotation * Vector3.forward * Time.deltaTime * _speed * .25f;
        }

        private Vector2 MyPosition() {
            return (Vector2)_myGameObject.transform.position + Parent.Attributes.Get<Collider2D>("ObstructCollider").offset;
        }
 */