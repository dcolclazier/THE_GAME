using UnityEngine;

namespace Assets.Code {
    public sealed class StaticsHelper :   MonoBehaviour {
        void Awake() {
            DontDestroyOnLoad(gameObject);
        }
        public void OnLevelWasLoaded(int unused) {
           // Messenger.Cleanup();
           // NodeManager.ClearEntities();
        }
    }
}