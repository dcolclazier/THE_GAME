using System;
using System.Collections.Generic;
using Assets.Code.Entities;
using Assets.Code.Entities.Components;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Abstract {
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
                {"ObstructableUnit", typeof(ObstructableUnitComponent)},


            };
            foreach (var c in ComponentsToInit) {
                var temp = Activator.CreateInstance(componentList[c]);
                var component = temp as IComponent;
                if (component == null) continue;

                component.Parent = myParent;
                component.Init();
                myParent.Components.Register(c, component);
            }
            Messenger.Broadcast("EntityCreated", myParent);
        }
    }
}