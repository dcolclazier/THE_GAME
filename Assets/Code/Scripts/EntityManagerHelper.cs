using System;
using UnityEngine;

namespace Assets.Code.Abstract {
    public class EntityManagerHelper : MonoBehaviour {


        public void Awake() {
            
            DontDestroyOnLoad(this);
            Messenger.Broadcast("OnAwake");
        }

        protected void Update() {
            GameObject obj = GameObject.Find("EntityManagerHelper");
            if (obj != null && obj != gameObject) {
                Debug.Log("Deleted a copy of me...");
                Destroy(obj);
            }
            Messenger.Broadcast("OnUpdate");
        }

        protected void Start() {

            Messenger.Broadcast("OnStart");
        }
    }
}