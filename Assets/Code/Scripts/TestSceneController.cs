using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Scripts {
    public class TestSceneController : MonoBehaviour {

        protected void Update()
        {
            //Left click
            if (Input.GetMouseButtonDown(0))
            {
                //Check to see if a player was clicked on. If so, broadcast the player to necessary listeners.
                var playerClicked = UnityUtilites.CheckHitLayer8(); // Layer 8 for player characters
                if (playerClicked) Messenger.Broadcast("PlayerSelected", playerClicked.transform.gameObject); 
            }
            //left mouse held
            if (Input.GetMouseButton(0)){
                //Allow screen to be dragged.
                UnityUtilites.ScreenDrag();
            }
        }
    }
}

