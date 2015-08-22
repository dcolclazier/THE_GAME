using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Code.Entities.Components;
using Assets.Code.Statics;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour {

    

    private EventSystem _eventSystem; 
	public void MouseOver(GameObject who) {
	}

    public void Awake() {
        _eventSystem = GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>();
        Messenger.AddListener<Vector2>("RightMouseDown",TestMethod);
        Messenger.MarkAsPermanent("RightMouseDown");
    }

    public void Start() {
        

    }
    public void Update() {
        Debug.Log("Running UI Update");
        if (Input.GetMouseButtonDown(0)) HandleLeftClick();
        if (Input.GetMouseButtonDown(1)) HandleRightClick();
    
    }

    private void HandleRightPress() {
        if (UIClicked()) return;
        foreach (LayerFlag layer in Enum.GetValues(typeof(LayerFlag)))
        {
            if (UnityUtilites.CheckHitOnLayer(layer))
            {
                Messenger.Broadcast("RightMouseHeld", UnityUtilites.MouseWorldPoint(), layer);
                Debug.Log("Mouse down on layer " + layer);
            }
        }
    }

    private void HandleLeftPress() {
        if (UIClicked()) return;
        foreach (LayerFlag layer in Enum.GetValues(typeof(LayerFlag)))
        {
            if (UnityUtilites.CheckHitOnLayer(layer))
            {
                Messenger.Broadcast("LeftMouseHeld", UnityUtilites.MouseWorldPoint(), layer);
                Debug.Log("Mouse down on layer " + layer);
            }
        }

    }

    private void HandleRightClick() {
        if (UIClicked()) return;
        foreach (LayerFlag layer in Enum.GetValues(typeof(LayerFlag)))
        {
            if (UnityUtilites.CheckHitOnLayer(layer))
            {
                Messenger.Broadcast("RightMouseDown", UnityUtilites.MouseWorldPoint(), layer);
                Debug.Log("Mouse down on layer " + layer);
            }
        }

    }

    private void HandleLeftClick() {
        if (UIClicked()) return;
        
        foreach (LayerFlag layer in Enum.GetValues(typeof(LayerFlag))) {
            if (UnityUtilites.CheckHitOnLayer(layer)) {
                Messenger.Broadcast("LeftMouseDown",UnityUtilites.MouseWorldPoint(),layer);
                Debug.Log("Mouse down on layer " + layer);
            }
        }
    }

    bool UIClicked() {
        return _eventSystem.IsPointerOverGameObject();
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

    public void TestMethod(Vector2 position) {
        
      
    }
	
}