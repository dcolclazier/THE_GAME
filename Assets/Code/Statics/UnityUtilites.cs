using System;
using System.Collections.Generic;
using Assets.Code.Entities.Components;
using UnityEngine;

namespace Assets.Code.Statics {
    static public class UnityUtilites {

        static public Vector2 MouseWorldPoint() {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public static void ScreenDrag() {
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");
            var cameraPos = new Vector3(-mouseX, -mouseY, 0);
            Camera.main.transform.position += cameraPos*.8f;

        }

        public static RaycastHit2D CheckHitOnLayer(LayerFlag layer) {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, (int)layer);
            return hit;
        }
        public static int ConvertBinaryFlag(LayerFlag test) {
            int i = 0;
            var test2 = (int) test;
            do {
                i++;
                test2 /= 2;
            } while (test2 != 1);
            //Debug.Log("Converted " + (int)test + " to " + i);
            return i;
        }

        public static Vector3[] SpriteLocalToWorld(Sprite sp, GameObject go)
        {
            Vector3 pos = go.transform.position;
            Vector3[] array = new Vector3[2];
            //top left
            array[0] = pos + sp.bounds.min;
            // Bottom right
            array[1] = pos + sp.bounds.max;
            return array;
        }

        //public static IEnumerable<LayerFlag> GetFlags(int input) {
        //    var flag = (LayerFlag)input;
        //    foreach (LayerFlag value in Enum.GetValues(flag.GetType()))
        //    {
        //        if (value.IsSet((LayerFlag) input)) {
        //            Debug.Log("Got one!");
        //            yield return value;
        //        }
        //    }
                

        //}
        
    }
}