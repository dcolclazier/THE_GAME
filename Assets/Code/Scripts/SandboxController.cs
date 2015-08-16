using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Abstract;
using Assets.Code.Entities;
using UnityEngine;

namespace Assets.Code.Scripts {

    public class SandboxController : MonoBehaviour {

        protected void Awake() {
            //required in the awake method of every scenecontroller!
            NodeManager.Init();
        }
        protected void Start()
        {

            //Begin test code
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
            //End test code
            
            

            
            
        }

        //testing GetAllSolidNodes
        //NOTE - DO NOT CALL GETALLSOLIDNODES() during Awake() or Start() if you
        //        need to rely on it for pathing - it may not catch all of the run-time
        //        objects that are also created during Awake() or Start()
        public void GetAllSolidNodeTest() {
            //This function is tied to the button in SandBox scene
            foreach (var node in NodeManager.GetAllSolidNodes(0f))
            {
                Debug.Log("-- Testing GetAllSolidNodes() --");
                Debug.Log(string.Format("Node - X: {0}, Y: {1}", node.position.x, node.position.y));
            }
        }

     
        
 
       
    }
}