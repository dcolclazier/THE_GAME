using UnityEngine;

namespace Assets.Code.Abstract {
    public class Player : Entity {

        public Player(string playerName, Vector2 position) {
            Components.Register("GameObject", new GameObjectComponent(this, playerName, position) );
            Components.Register("Selectable", new SelectableComponent(this));
            Components.Register("Obstructable", new ObstructableComponent(this));
            Components.Register("PathFinding", new PathFindingComponent(this));
        }
    }
}