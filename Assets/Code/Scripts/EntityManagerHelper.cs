﻿using UnityEngine;

namespace Assets.Code.Abstract {
    public class EntityManagerHelper : MonoBehaviour {
        public void Awake() {
            DontDestroyOnLoad(this);
            Messenger.Broadcast("OnAwake");
        }

        protected void Update() {
            Messenger.Broadcast("OnUpdate");
        }

        protected void Start() {
            Messenger.Broadcast("OnStart");
        }
    }
}