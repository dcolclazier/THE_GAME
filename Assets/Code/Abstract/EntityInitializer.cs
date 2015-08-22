using System;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Code.Entities;
using Assets.Code.Entities.Components;
using Assets.Code.Statics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Code.Abstract {
  public class EntityInitializer : MonoBehaviour, IEntityInitializer {
        public List<string> ComponentsToInit { get; protected set; }

        public List<string> EntitysToInclude { get; protected set; }

        public Dictionary<string, int> NumericalAttributesToSet { get; protected set; }
        public Dictionary<string, string> StringAttributesToSet { get; protected set; }
        protected virtual void Awake() {
            var myParent = new Entity();
            myParent.Attributes.Register("GameObject", gameObject);

           var componentList = new Dictionary<string, Type> {
                {"GameObject", typeof(GameObjectComponent)},
                {"Selectable", typeof(SelectableComponent)},
                {"Obstructable", typeof(ObstructableComponent)},
                {"Pathfinding", typeof(PathFindingComponent)},
                {"IronBowRangedAttack", typeof(IronBowRangedAttackComponent)},
                //{"Defender", typeof(DefenderCompononent)},
                {"ObstructableUnit", typeof(ObstructableUnitComponent)},
                {"UnitStats", typeof(UnitStatsComponent)},
                {"Moveable", typeof(MoveableComponent)},
                {"AttackableComponent", typeof(AttackableComponent)},
                {"FullyEquipped", typeof(FullyEquippedComponent)},
                {"CanWieldAll", typeof(CanWieldAllComponent)},
				{"BasicAttack", typeof(BasicAttackComponent)},

            };
            var validEntityList = new Dictionary<string, Type> {
                {"BasicWoodenSword", typeof(BasicWoodenSword)},
            };
            
            foreach (var c in ComponentsToInit) {

                if (c == "GameObject") {
                    var test = gameObject.AddComponent<GameObjectComponent>();
                    test.Parent = myParent;
                    test.Init();
                    myParent.Components.Register(c, (IComponent) test);
                }
                else {
                    var temp = Activator.CreateInstance(componentList[c]);
                    var component = temp as IComponent;
                    if (component == null) continue;
                    component.Parent = myParent;
                    component.Init();
                    myParent.Components.Register(c, component);
                }
            }
            foreach (var e in EntitysToInclude) {
                var test = Activator.CreateInstance(validEntityList[e]);
                var entity = test as Entity;
                if (entity == null) continue;
                entity.Init(myParent);
            }
            
            //todo - figure out a way to make this generic and type safe
            foreach (var entry in NumericalAttributesToSet) {
                if(!myParent.Attributes.Contains(entry.Key)) myParent.Attributes.Register(entry.Key,entry.Value);
                else myParent.Attributes.Update(entry.Key, entry.Value);
            }
            foreach (var entry in StringAttributesToSet) {
                if(!myParent.Attributes.Contains(entry.Key)) myParent.Attributes.Register(entry.Key,entry.Value);
                else myParent.Attributes.Update(entry.Key, entry.Value);
            }
            Messenger.Broadcast("EntityCreated", myParent);
        }
    }
}