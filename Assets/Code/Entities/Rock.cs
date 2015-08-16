
namespace Assets.Code.Entities
{
    public class Rock : Entity
    {
       //created during run-time, this object is.
       protected override void Start() {
           base.Start(); //must be called at end of override Start()
           Solid = true; //object is not solid by default.
       }
    }
}
