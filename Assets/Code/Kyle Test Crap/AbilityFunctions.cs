using System.Collections;
using System.Collections.Generic;
using Assets.Code.Abstract;
using UnityEngine.UI;
using UnityEngine;
using Assets.Code.Statics;
using Vectrosity;
using Assets.Code.Entities;

public static class AbilityFunctions {

	public static void RangeNova(Entity Parent, VectorLine rangeCircle, float range)
	{
		Vector2 whereMe = Parent.Attributes.Get<Vector2>("CurrentPathTarget");
		if (whereMe == null) whereMe = Parent.Attributes.Get<Vector2>("Position");
		rangeCircle.MakeCircle(whereMe, range);
		rangeCircle.active = true;
		Messenger.Broadcast("AttackableInRange", whereMe, range);
	}

	public static void RangeArc(Entity Parent, VectorLine vectorLine, float range, int arcSize)
	{
		Vector2 whereMe = Parent.Attributes.Get<Vector2>("CurrentPathTarget");
		if (whereMe == null) whereMe = Parent.Attributes.Get<Vector2>("Position");
		Vector2 direction = Vector2.up;

	}



}
