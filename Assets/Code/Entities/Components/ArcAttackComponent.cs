using System.Collections.Generic;
using Assets.Code.Abstract;
using UnityEngine.UI;
using UnityEngine;
using Assets.Code.Statics;
using Vectrosity;


namespace Assets.Code.Entities.Components {
	public class ArcAttackComponent : UnitAbility {

		public override void Init() {
			sprite = Resources.LoadAll<Sprite>("diablo_pack");
			spriteNum = 8;
			AbilityNum = 1;
			damage = 1;
			range = 3;
			base.Init();
		}

		public override void OnUpdate() {
			throw new System.NotImplementedException();
		}


		protected override void OnSelected(GameObject selectedObj)
		{

			base.OnSelected(selectedObj);
		}

		protected override void OnDeselect(GameObject deselectedObject)
		{

			base.OnDeselect(deselectedObject);
		}

		protected override void IAmActivated()
		{
			if (!enabled) return;
			//Activated = true;
			//draw gold box around ability
			//show range of ability even without mouse hovering over, from unit current local
			//change enemies in range tint red to show targetable
		}

		protected override void DoAttack()
		{
			//if (activated = true) do the attack stuff 
			//calculate to damage done
			//broadcast X damage done to X Entity
			//play animation
		}

		protected override void mouseEnter()
		{
			if (!enabled) return;
			RangeArc(rangeCircle, range, 45);
		}

		protected override void mouseExit()
		{

			base.mouseExit();
		}
	}
}