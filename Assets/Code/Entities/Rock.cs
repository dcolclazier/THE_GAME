﻿using UnityEngine;

namespace Assets.Code.Entities
{
    public class Rock : Entity
    {
        protected override void Start() {
            Solid = true;
            base.Start();
        }
    }
}
