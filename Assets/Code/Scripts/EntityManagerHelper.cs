using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Scripts {
    public class EntityManagerHelper : MonoBehaviour {


       protected void Update() {
            var obj = GameObject.Find("EntityManagerHelper");
            if (obj != null && obj != gameObject) {
                Debug.Log("Deleted a copy of me...");
                Destroy(obj);
            }
            Messenger.Broadcast("OnUpdate");
        }

    }
}