using Assets.Code.Abstract;
using UnityEngine;

namespace Assets.Code.States
{
    public class PreStartState : IEngineState
    {
        private readonly GameManager _manager;

        public PreStartState(GameManager gameManager)
        {
            _manager = gameManager;
        }

        public void Update()
        {
            //throw new NotImplementedException();
        }

        public void Deactivate()
        {
            //throw new NotImplementedException();
        }

        public void Activate()
        {
            //This state can be used to preform any pre-loading functionality before the main menu appears. 
            Debug.Log("Changing State to Main Menu!");
            _manager.ChangeState(new MainMenuState(_manager));
        }
    }
}