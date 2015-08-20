using Assets.Code.Abstract;
using UnityEngine;
using Assets.Code.Statics;

public class DrawGizmos : MonoBehaviour {

    void OnDrawGizmos() {
        int i = 0;
        foreach (var node in EntityManager.GetAllSolidNodes() ) {
            Gizmos.DrawSphere(node.Position, 0.1f);
            i++;
        }
        Debug.Log("Drawing " + i + " nodes.");
    }


}
