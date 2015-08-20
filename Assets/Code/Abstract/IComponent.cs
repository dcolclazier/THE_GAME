using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Abstract {
    public interface IComponent {

        Entity Parent { get; }
        List<string> Dependencies { get; }
        void OnUpdate();
        void OnAwake();
        void OnStart();
        void OnMessage();

    }
}
