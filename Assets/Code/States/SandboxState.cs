﻿using Assets.Code.Abstract;
using UnityEngine;

namespace Assets.Code.States
{
    public class SandboxState : IEngineState
    {
        private readonly GameManager _engine;
        
        public void Update()
        {
           
        }
        public void Deactivate()
        {
            
        }

        public void Activate()
        {
            Application.LoadLevel("Sandbox");
        }
    }
}