using Assets.Code.Abstract;
using UnityEngine;

namespace Assets.Code.States
{
    public class MainMenuState : IEngineState
    {
        private readonly GameManager _manager;
        public MainMenuState(GameManager manager)
        {
            _manager = manager;
        }
        public void Update()
        {
            
        }

        public void Deactivate()
        {
            
        }

        public void Activate()
        {
            Application.LoadLevel("MainMenu");
        }
    }
}