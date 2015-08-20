using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Abstract {
    /*
         So the general idea for this script is that it should be attached to any prefab for any 
         * object that needs to exist in the Unity engine and not logically in the game engine
         * There should be a list of components in here (strings) that point to a list of all of the necessary
         * components, and a list of attributes(that aren't already configured by components) that should be initialized
         * 
         * Each component has a list of dependencies, so there will need to be a correct order.
         * 
         * For each item in the list of components that should be have, find the first one that does not contain
         * any of the other components in its dependency list (default should be gameobject)
         * 
         * activate that component, first by initializing and activating any dependencies and then by finally adding 
         * componnent/attribute to the list for the entity.
         * 
         * Then remove it from the list of components that still need to be activated.
         
         */
    public class EntityInitializer : MonoBehaviour, IEntityInitializer {
        public List<string> ComponentsToInit { get; protected set; }
        public List<string> UniqueEntityTags { get; protected set; }
        protected virtual void Awake() {
            var myParent = new Entity();
            myParent.Attributes.Register("GameObject", gameObject);

           var componentList = new Dictionary<string, Type> {
                {"GameObject", typeof(GameObjectComponent)},
                {"Selectable", typeof(SelectableComponent)},
                {"Obstructable", typeof(ObstructableComponent)},
                {"Pathfinding", typeof(PathFindingComponent)},
                {"IronBowRangedAttack", typeof(IronBowRangedAttackComponent)},
                {"Defender", typeof(DefenderCompononent)},


            };
            foreach (var c in ComponentsToInit) {
                var temp = Activator.CreateInstance(componentList[c]);
                var component = temp as IComponent;
                if (component != null) {
                    component.Parent = myParent;
                    component.Init();
                    myParent.Components.Register(c, component);
                }
            }
            Messenger.Broadcast("EntityCreated", myParent);
        }
    }
}