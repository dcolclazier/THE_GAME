using UnityEngine;

namespace Assets.Code.Entities
{
    public class Rock : Entity
    {
        public new Collider2D Collider {
            get { return GetComponent<BoxCollider2D>(); }
        }
        
        protected override void Start() {
            Solid = true;
            base.Start();
        }
    }
}
