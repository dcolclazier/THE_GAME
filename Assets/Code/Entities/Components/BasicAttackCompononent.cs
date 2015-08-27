using System.Collections.Generic;
using Assets.Code.Abstract;
using UnityEngine.UI;
using UnityEngine;
using Assets.Code.Statics;
using Vectrosity;


namespace Assets.Code.Entities.Components {
	internal class BasicAttackComponent : IComponent {

		private Sprite[] sprite = Resources.LoadAll<Sprite>("diablo_pack");
		private int AbilityNum = 1;
		private GameObject button;
		private Sprite defaultSprite;
		private bool enabled;
		private VectorLine rangeCircle;

		private float range = 1.2f;
		private int damage = 1;
		private float _lineThickness = 2.2f;

		public List<string> Dependencies {
			get { return new List<string>() {
				"Selectable"	
				}; }
		}
		
		public void OnUpdate() 
		{
			throw new System.NotImplementedException();
		}
		
		public void Init() 
		{
			button = GameObject.Find("Button_" + AbilityNum);
			defaultSprite = button.GetComponent<Image>().sprite;

			Messenger.AddListener<GameObject>("GameObjectSelected", OnSelected);
			Messenger.AddListener<GameObject>("GameObjectDeselected", OnDeselect); //method of deselecting should change
			//Messenger.AddListener<Entity>("AttackableEnemyClicked", DoAttack);
			Messenger.AddListener<GameObject>("GroundClicked", OnDeselect); //method of deselecting should change
			Messenger.AddListener("ClickedAbility1", IAmActivated);
			Messenger.AddListener("MouseEnterAbility1", mouseEnter);
			Messenger.AddListener("MouseExitAbility1", mouseExit);

			rangeCircle = new VectorLine("Select Circle", new Vector3[720], null, _lineThickness);
			rangeCircle.Draw3DAuto();
			rangeCircle.color = Color.green;
		}
		private void OnSelected(GameObject selectedObj)
		{
			//if the object selected was us, enabled = true - otherwise, enable = false.
			enabled = selectedObj == Parent.Attributes.Get<GameObject>("GameObject");


		    if (enabled && Parent.Attributes.Get<bool>("CurrentlySelected")) 
			{
		        button.GetComponent<Image>().sprite = sprite[108];
		    }
		}
		private void OnDeselect(GameObject deselectedObject)
		{
			if (!enabled || Parent.Attributes.Get<bool>("CurrentlySelected")) return;
			button.GetComponent<Image>().sprite = defaultSprite;
		}
		private void IAmActivated()
		{
			if (enabled)
			{
				Debug.Log("############## BASIC ATTACK ACTIVATED ###############");
				//Activated = true;
				//draw gold box around ability
				//show range of ability even without mouse hovering over, from unit current local
				//change enemies in range tint red to show targetable
			}
		}
		private void DoAttack()
		{
			//if (activated = true) do the attack stuff 
			//calculate to damage done
			//broadcast X damage done to X Entity
			//play animation
		}
		private void mouseEnter()
		{
			if (enabled)
			{
				//should call this stuff from an external function probably
				Vector2 whereMe = Parent.Attributes.Get<Vector2>("CurrentPathTarget");
				if (whereMe == null) whereMe = Parent.Attributes.Get<Vector2>("Position");
				rangeCircle.MakeCircle(whereMe, range);
				rangeCircle.active = true;
				//see if enemy units would be in range
				Messenger.Broadcast("AttackableInRange", whereMe, range);
				//tooltip popup
			}
		}
		private void mouseExit()
		{
			rangeCircle.active = false;
			Messenger.Broadcast("AttackableStopHighlight");
		}

		public Entity Parent { get; set; }
	}
}