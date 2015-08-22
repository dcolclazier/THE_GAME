using UnityEngine;
using System.Collections;
using Assets.Code.Statics;

public class UIController : MonoBehaviour {
	
	public void MouseOver(GameObject who)
	{
		
	}
	public void ClickedButton1()
	{
		Messenger.Broadcast("AbilityOneClicked");
	}
	public void ClickedButton2()
	{
		Messenger.Broadcast("AbilityTwoClicked");
	}
	public void ClickedButton3()
	{
		Messenger.Broadcast("AbilityThreeClicked");
	}
	public void ClickedButton4()
	{
		Messenger.Broadcast("AbilityFourClicked");
	}
	public void ClickedButton5()
	{
		Messenger.Broadcast("AbilityFiveClicked");
	}

	
}