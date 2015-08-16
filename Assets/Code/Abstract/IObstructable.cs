using UnityEngine;

namespace Assets.Code.Abstract {
    public interface IObstructable  {
        bool Solid { get; }
        Collider2D Collider { get; }

        void Phase();
    }
}