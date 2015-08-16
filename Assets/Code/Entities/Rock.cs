using UnityEngine;

namespace Assets.Code.Entities
{
    public class Rock : Entity
    {
       //created during run-time, this object is.
       protected override void Start() {
            Solid = true;
            base.Start();
        }
    }
}
