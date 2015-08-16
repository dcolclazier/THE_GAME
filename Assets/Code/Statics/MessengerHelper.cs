using UnityEngine;

namespace Assets.Code {
    public sealed class MessengerHelper :   MonoBehaviour {
        void Awake() {
            DontDestroyOnLoad(gameObject);
        }
        public void OnLevelWasLoaded(int unused) {
            Messenger.Cleanup();
        }
    }
}