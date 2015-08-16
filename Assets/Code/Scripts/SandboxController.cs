using System.Collections.Generic;
using Assets.Code.Abstract;
using Assets.Code.Entities;
using UnityEngine;

namespace Assets.Code.Scripts {
    public class SandboxController : MonoBehaviour {
        public List<Node> EntityNodes;

        protected void Awake() {
            EntityNodes = new List<Node>();
            Messenger.AddListener<IObstructable>("EntityAppeared", TestEntityAdded); 
        }

        protected void Start() {
            
            //create a rock
            var y = (new GameObject("TestRock").AddComponent<PolygonCollider2D>());
            var rock = y.gameObject.AddComponent<TestRock>();
            y.points = new[] {
                new Vector2(-1, 1),
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(-1, -1)
            };
            //y.size = new Vector2(2,2);
        }

        private void TestEntityAdded(IObstructable addedEntity)
        {
            if (addedEntity.Collider == null) {
                Debug.Log("Well, shit... it was null.");
                Debug.Log("Solidity: " + addedEntity.Solid);
            }
            else {
                Debug.Log(string.Format("Controller: new Entity found! Collider: {0}, Solid? {1}", addedEntity.Collider.name, addedEntity.Solid));    
                var nodeList = NodeHelper.GetNodes(addedEntity);
                foreach(var node in nodeList) EntityNodes.Add(node);
            }
            

        }

        
    }
}