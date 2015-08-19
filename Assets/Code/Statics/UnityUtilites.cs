using System.Collections.Generic;
using UnityEngine;

static public class UnityUtilites {
    static public Vector3 MouseWorldPoint()
    {
        Vector3 point;
        point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        point.z = 0;
        return point;
    }
    public static Vector3[] ConvertToVectorArray(List<Node> nodes)
    {
        var vectors = new Vector3[nodes.Count];
        var i = 0;
        foreach (var node in nodes)
        {
            vectors[i] = node.ToVector3();
            i++;
        }
        return vectors;
    }
}