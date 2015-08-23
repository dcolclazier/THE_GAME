﻿using System;
using System.Collections;
using Assets.Code.Entities.Components;
using Assets.Code.Statics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code.Scripts {
    public class UiController : MonoBehaviour {

        private EventSystem _eventSystem; 
        public void MouseOver(GameObject who) {
        }

        public void Awake() {
            _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            Messenger.AddListener<Vector2>("RightMouseDown",TestMethod);
            Messenger.MarkAsPermanent("RightMouseDown");
        }

        public void Update() {
            var hoverDelay = .75f;
            if (Input.GetMouseButtonDown(0)) HandleMouseClick("LeftMouseDown");
            if (Input.GetMouseButtonDown(1)) HandleMouseClick("RightMouseDown");

            if (_eventSystem.IsPointerOverGameObject()) StartCoroutine("HandleUiHover", hoverDelay);
            else {
                StopCoroutine("HandleHover");
                Messenger.Broadcast("UIHoverStopped");
            }

        }

        private void HandleMouseClick(string eventToBroadcast) {
            //Gets top most layer clicked, and broadcast the object it hits if it hits anything, along with the layer
            if (_eventSystem.IsPointerOverGameObject()) return;
            foreach (LayerFlag layer in Enum.GetValues(typeof(LayerFlag))) {
                var hit = UnityUtilites.CheckHitOnLayer(layer);
                if (hit.collider != null) {
                    Messenger.Broadcast(eventToBroadcast, layer, hit);
                    Debug.Log("Mouse down on layer " + layer);
                    break;
                }
            }
        }

        private IEnumerator HandleUiHover(float seconds) {
            int i = 0;
            yield return new WaitForSeconds(seconds);
            if (_eventSystem.IsPointerOverGameObject()) {
                if (i < 1) {
                    Messenger.Broadcast("UIHover", Input.mousePosition);
                    i++;
                }
                Debug.Log("Tooltip Appeared here!");
            }
        }
    
      
    

    bool UIClicked() {
        return _eventSystem.IsPointerOverGameObject();
    }

    public void AbilityClicked(int buttonNumber)
		{
			Messenger.Broadcast("ClickedAbility" + buttonNumber);
		}
	public void MouseEnterAbility(int buttonNumber)
	{
		Messenger.Broadcast("MouseEnterAbility" + buttonNumber);
	}
	public void MouseExitAbility(int buttonNumber)
	{
		Messenger.Broadcast("MouseExitAbility" + buttonNumber);
	}


        public void TestMethod(Vector2 position) {
        
      
        }
	
    }
}