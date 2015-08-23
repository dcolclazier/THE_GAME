using System;
using System.Collections.Generic;
using Assets.Code.Entities;

namespace Assets.Code.Abstract {
    public class TooltipComponent : IComponent {
        public Entity Parent { get; set; }

        public List<string> Dependencies {
            get { return new List<string>() {
                "GameObject"
            }; }
        }

        public void OnUpdate() {
            throw new NotImplementedException();
        }

        public void Init() {
            Parent.Attributes.Register("TooltipMessage","JUST THE TOOLTIP");
        }
    }
}