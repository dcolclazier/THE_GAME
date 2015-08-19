using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Code;

public class DrawGizmos : MonoBehaviour {

    void OnDrawGizmos()
    {
        Debug.Log("Drawing nodes.");
        foreach (Node node in NodeManager.GetAllSolidNodes(0f) )
        {

            Gizmos.DrawSphere(node.position, 0.1f);
        }
    }


}
