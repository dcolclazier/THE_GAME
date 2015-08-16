using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Abstract;
using Assets.Code.Entities;
using UnityEngine;

namespace Assets.Code.Scripts {
    public class SandboxController : MonoBehaviour {
        private Dictionary<IObstructable, List<Node>> _entityNodes;

        protected void Awake() {
            _entityNodes = new Dictionary<IObstructable, List<Node>>();
            Messenger.AddListener<IObstructable>("EntityAppeared", EntityAppeared); 
            Messenger.AddListener<IObstructable>("EntityDisappeared", EntityAppeared); 
        }

        protected void Start() {
            
            //create a rock - make sure monobehaviour is last component added
            var rockcollider = (new GameObject("Rock").AddComponent<PolygonCollider2D>());
            rockcollider.points = new[] {
                new Vector2(-1, 1),
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(-1, -1)
            };
            var rock = rockcollider.gameObject.AddComponent<Rock>();
            //rock created.
        }

        public IEnumerable<Node> GetSolidNodes() {
            var completeNodeList = new List<Node>();
            foreach (var item in _entityNodes) {
                if (item.Key.Solid) {
                    completeNodeList.AddRange(item.Value);
                }            
            }
            return completeNodeList;
        } 
        private void EntityAppeared(IObstructable addedEntity)
        {
            if (addedEntity.Collider == null) {
                throw new NullReferenceException("Collider wasn't found - are you sure it was created prior to it's monobehaviour awake ran?");
            }

            if (_entityNodes.ContainsKey(addedEntity)) Debug.Log("Duplicate Entity detected...");
            else {
                _entityNodes.Add(addedEntity, NodeHelper.GetNodes(addedEntity).ToList());
                Debug.Log(string.Format("Controller: new Entity added: {0}, Solid? {1}", addedEntity.Collider.name, addedEntity.Solid));
            }
        }
    }
}