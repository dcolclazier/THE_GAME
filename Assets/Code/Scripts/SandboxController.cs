using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Abstract;
using Assets.Code.Entities;
using UnityEngine;

namespace Assets.Code.Scripts {
    public class SandboxController : MonoBehaviour {
        private Dictionary<Entity, List<Node>> _entityNodes;

        protected void Awake() {
            Messenger.AddListener<Entity>("EntityCreated", EntityCreated);
            _entityNodes = new Dictionary<Entity, List<Node>>();
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
            Messenger.Broadcast("EntityCreated", rock);
            //rock created.
        }

        public IEnumerable<Node> GetSolidNodes() {
            var completeNodeList = new List<Node>();
            foreach (var item in _entityNodes.Where(item => item.Key.Solid)) {
                completeNodeList.AddRange(item.Value);
            }
            return completeNodeList;
        } 
        private void EntityCreated(Entity entity)
        {
            if (entity.Collider == null) {
                throw new NullReferenceException("Collider wasn't found - are you sure it was created prior to it's monobehaviour awake ran?");
            }
            if (_entityNodes.ContainsKey(entity)) Debug.Log("Duplicate Entity detected...");
            else {
                _entityNodes.Add(entity, NodeHelper.GetNodes(entity).ToList());
                Debug.Log(string.Format("Controller: new Entity added: {0}, Solid? {1}", entity.Collider.name, entity.Solid));
            }
        }
    }
}