using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Scripts {
    public class DrawGizmos : MonoBehaviour {

        protected void Awake() {
       
        }

        void OnDrawGizmos() {
            int i = 0;
            foreach (var node in EntityManager.GetAllSolidNodes() ) {
                Gizmos.DrawSphere(node.Position, 0.1f);
                //TextGizmo.Draw(node.Position + new Vector2(0,0),  node.TotalScoreF.ToString());
                i++;
            }
            Debug.Log("Drawing " + i + " nodes.");
        }


    }
}
