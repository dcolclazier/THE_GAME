using System;
using System.Collections.Generic;
using Assets.Code.Abstract;
using Assets.Code.Statics;
using Assets.Code.Entities;
using UnityEngine;
using UnityEngine.UI;
using Vectrosity;

public abstract class UnitAbility : IComponent {

	protected Sprite[] sprite;
	protected int spriteNum;
	protected int AbilityNum;
	protected float range;
	protected int damage;

	protected GameObject button {get; private set;}
	protected Sprite defaultSprite;
	protected bool enabled;

	protected VectorLine rangeCircle;
	protected float lineThickness = 2.2f;

	public List<string> Dependencies {
		get {
			return new List<string>() {
				"GameObject",
				"Selectable",
			};
		}
	}

	public virtual void OnUpdate() {
		//throw new System.NotImplementedException();
	}
	
	public virtual void Init() {
		button = GameObject.Find("Button_" + AbilityNum);
		defaultSprite = button.GetComponent<Image>().sprite;

		Messenger.AddListener<GameObject>("GameObjectSelected", OnSelected);
		Messenger.AddListener<GameObject>("GameObjectDeselected", OnDeselect); //method of deselecting should change
		//Messenger.AddListener<Entity>("AttackableEnemyClicked", DoAttack);
		Messenger.AddListener("ClickedAbility" + AbilityNum, IAmActivated);
		Messenger.AddListener("MouseEnterAbility" + AbilityNum, mouseEnter);
		Messenger.AddListener("MouseExitAbility" + AbilityNum, mouseExit);
		
		rangeCircle = new VectorLine("Select Circle", new Vector3[720], null, lineThickness);
		rangeCircle.Draw3DAuto();
		rangeCircle.color = Color.green;

	}

	protected virtual void OnSelected(GameObject selectedObj)
	{
		//if the object selected was us, enabled = true - otherwise, enable = false.
		enabled = selectedObj == Parent.Attributes.Get<GameObject>("GameObject");			
		if (enabled && Parent.Attributes.Get<bool>("CurrentlySelected")) 
		{
			button.GetComponent<Image>().sprite = sprite[spriteNum];
		}
	}

	protected virtual void OnDeselect(GameObject deselectedObject)
	{
		if (!enabled || Parent.Attributes.Get<bool>("CurrentlySelected")) return;
		button.GetComponent<Image>().sprite = defaultSprite;
	}

	protected virtual void IAmActivated()
	{

	}

	protected virtual void mouseEnter()
	{


	}

	protected virtual void mouseExit()
	{
		rangeCircle.active = false;
		Messenger.Broadcast("AttackableStopHighlight");
	}

	protected void RangeNova(Entity Parent, VectorLine rangeCircle, float range)
	{
		Vector2 whereMe = Parent.Attributes.Get<Vector2>("CurrentPathTarget");
		if (whereMe == null) whereMe = Parent.Attributes.Get<Vector2>("Position");
		rangeCircle.MakeCircle(whereMe, range);
		rangeCircle.active = true;
		Messenger.Broadcast("AttackableInRange", whereMe, range);
	}

	protected void RangeArc(VectorLine vectorLine, float range, int arcSize)
	{
		GameObject arc = GameObject.Instantiate(Resources.Load("Prefabs/ConeAttack")) as GameObject;
		arc.transform.position = whereMe();
	}

	protected virtual void DoAttack()
	{


	}

	protected Vector2 whereMe ()
	{
		Vector2 whereMe = Parent.Attributes.Get<Vector2>("CurrentPathTarget");
		//if (whereMe == null) whereMe = Parent.Attributes.Get<Vector2>("Position");
		if (whereMe == null) whereMe = Parent.Attributes.Get<GameObject>("GameObject").transform.position;
		return whereMe;
	}
	
	
	public Entity Parent { get; set; }
}

