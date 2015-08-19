using UnityEngine;

static public class UnityUtilites {
    static public Vector3 MouseWorldPoint()
    {
        Vector3 point;
        point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        point.z = 0;
        return point;
    }
}