using System.Collections.Generic;
using Assets.Code.Abstract;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Entities.Components {
    public class UnitStatsComponent : IComponent {
        public Entity Parent { get; set; }
        public List<string> Dependencies {
            get { return new List<string>() {
                "GameObject"
            };}
        }

        public void OnUpdate() {
            throw new System.NotImplementedException();
        }

        public void Init() {
            Parent.Attributes.RegisterOrUpdate("Strength",7);
            Parent.Attributes.RegisterOrUpdate("Concentration",7);
            Parent.Attributes.RegisterOrUpdate("Perception",7);
            Parent.Attributes.RegisterOrUpdate("Magic",7);
            Parent.Attributes.RegisterOrUpdate("Agility",7);
            Parent.Attributes.RegisterOrUpdate("Wit",7);
            Parent.Attributes.RegisterOrUpdate("Wisdom",7);
            Parent.Attributes.RegisterOrUpdate("AttackSpeed",7);
            Parent.Attributes.RegisterOrUpdate("MovementSpeed",7f);
            Parent.Attributes.RegisterOrUpdate("BaseDamage", 1);
			Parent.Attributes.RegisterOrUpdate("Actions", 2);

			Messenger.AddListener("New Round", sendActions);
        }

		public void sendActions()
		{
			Messenger.Broadcast("Actions For Team", Parent.Attributes.Get<string>("Team"), Parent.Attributes.Get<int>("Actions"));
			Debug.Log("Game Object:  " + Parent.Attributes.Get<GameObject>("GameObject") + ",  Team:  " + Parent.Attributes.Get<string>("Team") + ",  Actions:  " + Parent.Attributes.Get<int>("Actions"));
			Debug.Log("Game Object:  " + Parent.Attributes.Get<GameObject>("GameObject") + ",  STR:  " + Parent.Attributes.Get<int>("Strength"));
		}
    }
}