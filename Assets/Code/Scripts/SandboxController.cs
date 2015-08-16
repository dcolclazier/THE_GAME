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

            //testing GetSolidNodes
            foreach (var node in NodeManager.GetSolidNodes()) {
                Debug.Log("-- Testing GetSolidNodes() --");
                Debug.Log(string.Format("Node - X: {0}, Y: {1}",node.position.x,node.position.y));
            }

            // take note - calling GetSolidNodes in the Start method executes before the Rock
            // component's entity node braodcast , even though it is later in execution 
            // - I assume this is based on how Unity starts the program. Thus, try to avoid 
            //  calling GetSolidNodes in a Start or Awake function until we get this fixed 
            // or figure it out a workaround.
            //End test code
            
        }
      

     
        
 
       
    }
}