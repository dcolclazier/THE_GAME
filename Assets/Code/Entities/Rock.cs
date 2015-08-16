
namespace Assets.Code.Entities
{
    public class Rock : Entity
    {
       //created during run-time, this object is.
       protected override void Start() {
           colliderType = NodeManager.ColliderType.Polygon;
            Solid = true; //object is not solid by default.
            base.Start(); //must be called at end of override Start()
        }
    }
}
