using UnityEngine;

namespace Assets.Code.Statics {
    static public class UnityUtilites {
        static public Vector2 MouseWorldPoint()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public static void ScreenDrag()
        {
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");
            var cameraPos = new Vector3(-mouseX, -mouseY, 0);
            Camera.main.transform.position += cameraPos;

        }

        public static RaycastHit2D CheckHitLayer8()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //1 << 8 raycasts only against layer 8 which has unit sprite hit boxes for unit selection
            var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, 1 << 8);
            return hit;
        }
    }
}