using System.Collections.Generic;
using Assets.Code.Abstract;
using UnityEngine.UI;
using UnityEngine;
using Assets.Code.Abstract;
using Assets.Code.Statics;



namespace Assets.Code.Entities.Components {
	internal class BasicAttackComponent : IComponent {

		private Sprite[] sprite = Resources.LoadAll<Sprite>("diablo_pack");
		private int AbilityNum = 1;
		private GameObject button;
		private Sprite defaultSprite;
		private bool enabled;

		public List<string> Dependencies {
			get { return new List<string>() {
				"Selectable"	
				}; }
		}
		
		public void OnUpdate() {
			throw new System.NotImplementedException();
		}
		
		public void Init() {
			button = GameObject.Find("Button_" + AbilityNum);
			defaultSprite = button.GetComponent<Image>().sprite;
			Messenger.AddListener<GameObject>("GameObjectSelected", OnSelected);
			Messenger.AddListener<Vector2>("GroundClicked", OnDeselect); //method of deselecting should change
			//Messenger.AddListener<?>("Ability#3Clicked", IAmActivated);
			//Messenger.AddListener<Entity>("AttackableEnemyClicked", DoAttack);


		}
		private void OnSelected(GameObject selectedObj)
		{
			//if the object selected was us, enabled = true - otherwise, enable = false.
			enabled = selectedObj == Parent.Attributes.Get<GameObject>("GameObject");

			if (enabled) button.GetComponent<Image>().sprite = sprite[108];
			Debug.Log ("SHOW THE ABILITY ICON");

		}
		private void OnDeselect(Vector2 blah)
		{
			if (!enabled) return;
			button.GetComponent<Image>().sprite = defaultSprite;
		}
		private void IAmActivated()
		{
			//Activated = true;
			//draw gold box around ability
			//show range of ability even without mouse hovering over, from unit current local
			//change enemies in range tint red to show targetable

		}
		private void DoAttack()
		{
			//if (activated = true) do the attack stuff 
			//calculate to damage done
			//broadcast X damage done to X Entity
			//play animation
		}
		public Entity Parent { get; set; }
	}
}