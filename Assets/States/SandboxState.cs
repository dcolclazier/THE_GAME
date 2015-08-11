using System.Net.Mime;
using Abstract;
using UnityEngine;

namespace States
{
    public class SandboxState : IEngineState
    {
        private readonly GameManager _engine;
        public SandboxState(GameManager engine)
        {
            _engine = engine;
            Application.LoadLevel("Sandbox");
        }
        public void Update()
        {
            //throw new System.NotImplementedException();
        }

        public void Render()
        {
            //throw new System.NotImplementedException();
        }
    }
}