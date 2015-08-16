using System.Collections.Generic;
using System.Linq;
using Assets.Code.Abstract;
using Assets.Code.Entities;
using UnityEngine;

namespace Assets.Code.Scripts {
    public class SandboxController : MonoBehaviour {
        public Dictionary<IObstructable, List<Node>> EntityNodes;

        protected void Awake() {
            EntityNodes = new Dictionary<IObstructable, List<Node>>();
            Messenger.AddListener<IObstructable>("EntityAppeared", TestEntityAdded); 
        }

        protected void Start() {
            
            //create a rock - make sure monobehaviour is last component added
            var polycollider = (new GameObject("TestRock").AddComponent<PolygonCollider2D>());
            var rock = polycollider.gameObject.AddComponent<TestRock>();
            polycollider.points = new[] {
                new Vector2(-1, 1),
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(-1, -1)
            };
        }
        private void TestEntityAdded(IObstructable addedEntity)
        {
            if (addedEntity.Collider == null) {
                Debug.Log("Well, shit... it was null.");
                Debug.Log("Solidity: " + addedEntity.Solid);
            }
            else {
                Debug.Log(string.Format("Controller: new Entity found! Collider: {0}, Solid? {1}", addedEntity.Collider.name, addedEntity.Solid));    
                if(EntityNodes.ContainsKey(addedEntity)) Debug.Log("Duplicate Entity detected...");
                else EntityNodes.Add(addedEntity, NodeHelper.GetNodes(addedEntity).ToList());
               
            }
        }
    }
}