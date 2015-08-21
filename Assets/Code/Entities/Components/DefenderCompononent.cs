using System.Collections.Generic;
using Assets.Code.Abstract;

namespace Assets.Code.Entities.Components {
    internal class DefenderCompononent : IComponent {
        public List<string> Dependencies {
            get { return new List<string>() {
                
            }; }
        }

        public void OnUpdate() {
            throw new System.NotImplementedException();
        }

        public void Init() {
            throw new System.NotImplementedException();
        }

        public Entity Parent { get; set; }
    }
}