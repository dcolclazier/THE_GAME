using UnityEngine;
using Assets.Code.Statics;

public class DrawGizmos : MonoBehaviour {

    void OnDrawGizmos() {
        Debug.Log("Drawing nodes.");
        foreach (var node in NodeManager.GetAllSolidNodes() ) {
            Gizmos.DrawSphere(node.Position, 0.1f);
        }
    }


}
