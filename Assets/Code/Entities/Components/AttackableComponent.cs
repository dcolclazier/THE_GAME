using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Abstract;
using Assets.Code.Statics;


namespace Assets.Code.Entities.Components {
    public class AttackableComponent : IComponent {
        public Entity Parent { get; set; }

        public List<string> Dependencies { get; private set; }

        public void OnUpdate() {
            throw new System.NotImplementedException();
        }

        public void Init() {
            Parent.Attributes.Register("AttackableComponent", true);
			Messenger.AddListener<Vector2, float>("AttackableInRange", inAttackRange);
			Messenger.AddListener("AttackableStopHighlight", stopHighlight);
        }

		private void inAttackRange(Vector2 where, float range)
		{
			Debug.Log("Attackble found in range message heard");
			Vector2 mePos = Parent.Attributes.Get<Vector2>("Position");
			GameObject me = Parent.Attributes.Get<GameObject>("GameObject");
			//float meRadius = Parent.Attributes.Get<float>("ObstructRadius");
			float meRadius = me.GetComponent<CircleCollider2D>().radius;

			if (Vector2.Distance(where, mePos) < meRadius + range)
			{
				me.GetComponent<SpriteRenderer>().color = Color.red;
			}
		}
		private void stopHighlight()
		{
			GameObject me = Parent.Attributes.Get<GameObject>("GameObject");
			me.GetComponent<SpriteRenderer>().color = Color.white;
		}
    }
}