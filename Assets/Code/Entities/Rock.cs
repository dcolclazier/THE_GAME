using UnityEngine;

namespace Assets.Code.Entities
{
    //[System.Serializable]
    public class Rock : Entity
    {
       //created during run-time, this object is.
       public string test;
       protected override void Start() {
            Solid = true;
            base.Start();
        }
    }
}
