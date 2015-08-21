using System.Collections.Generic;
using Assets.Code.Entities;

namespace Assets.Code.Abstract {
    public interface IComponent {

        Entity Parent { get; set; }
        List<string> Dependencies { get; }
        void OnUpdate();
        void Init();
    }
}
