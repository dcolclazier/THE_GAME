
namespace Assets.Code.Entities
{
    public sealed class Rock : Entity
    {
       //created during run-time, this object is.
       protected override void Start() {
           //objects are solid by default.
           //Solid = false;
           
           
           base.Start(); //must be called at end of override Start()
       }
    }
}
