using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Abstract;
using Assets.Code.Entities;
using UnityEngine;

namespace Assets.Code.Scripts {

    public class SandboxController : MonoBehaviour {

        protected void Awake() {
            NodeManager.GetSolidNodes();
        }
        protected void Start()
        {

            #region Testing Stuff - not real code
            //create a rock - make sure monobehaviour is last component added
            //tests ability of controller to detect announcements of entities created during
            //runtime
            var rockcollider = (new GameObject("Rock").AddComponent<PolygonCollider2D>());
            rockcollider.points = new[] {
                new Vector2(-1, 1),
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(-1, -1)
            };
            var rock = rockcollider.gameObject.AddComponent<Rock>();
            //rock created.

            //testing GetSolidNodes
            foreach (var node in NodeManager.GetSolidNodes()) {
                Debug.Log("-- Testing GetSolidNodes() --");
                Debug.Log(string.Format("Node - X: {0}, Y: {1}",node.position.x,node.position.y));
            }
            //End test
            #endregion

        }
      

     
        
 
       
    }
}