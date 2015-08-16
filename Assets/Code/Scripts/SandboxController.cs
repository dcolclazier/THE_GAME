using Assets.Code.Entities;
using UnityEngine;

namespace Assets.Code.Scripts {

    public class SandboxController : MonoBehaviour {
        protected void Start()
        {
            //Begin test code
            //create a rock - make sure monobehaviour is last component added
            var rockcollider = (new GameObject("Rock").AddComponent<PolygonCollider2D>());
            rockcollider.points = new[] {
                new Vector2(-1, 1),
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(-1, -1)
            };
            var rock = rockcollider.gameObject.AddComponent<Rock>();
            //End test code
        }

        public void GetAllSolidNodeTest() {
            //This function is tied to the test button in SandBox scene
            Debug.Log("-- Testing GetAllSolidNodes() --");
            foreach (var node in NodeManager.GetAllSolidNodes(2f))
            {
                Debug.Log(string.Format("Node - X: {0}, Y: {1}", node.position.x, node.position.y));
            }
        }
    }
}