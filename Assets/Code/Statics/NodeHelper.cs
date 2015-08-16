using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Entities;
using UnityEngine;

namespace Assets.Code {
    public sealed class NodeHelper : MonoBehaviour {

        public static Dictionary<Entity, List<Node>> EntityNodes;
        void Awake() {
            DontDestroyOnLoad(gameObject);
            EntityNodes = new Dictionary<Entity, List<Node>>();
            Messenger.AddListener<Entity>("EntityCreated", EntityCreated);
        }

        public void OnLevelWasLoaded(int unused) {
            NodeManager.Cleanup();
        }
        private void EntityCreated(Entity entity)
        {
            if (entity.Collider == null)
            {
                throw new NullReferenceException("Collider wasn't found - are you sure it was created prior to it's monobehaviour awake ran?");
            }
            if (EntityNodes.ContainsKey(entity)) Debug.Log("Duplicate Entity detected...");
            else
            {
                EntityNodes.Add(entity, NodeManager.GetNodes(entity).ToList());
                Debug.Log(string.Format("Controller: new Entity added: {0}, Solid? {1}", entity.Collider.name, entity.Solid));
            }
        }
    }
}