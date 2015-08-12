namespace Assets.Code.Abstract
{
    public interface IEngineState
    {
        void Update();
        void Deactivate();
        void Activate();
    }
}