using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Code;
using Vectrosity;

public class TestSceneController : MonoBehaviour {

	void Update()
    {
        //Left click on units 
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D playerUnit = overPlayerUnit();
            if (playerUnit) {
                var selected = playerUnit.transform.gameObject;
                Messenger.Broadcast("PlayerSelected", selected);
            }
        }
        //left mouse held to drag screen around
        if (Input.GetMouseButton(0))
        {
            screenDrag();
        }
    }
    RaycastHit2D overPlayerUnit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//1 << 8 raycasts only against layer 8 which has unit sprite hit boxes for unit selection
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, 1 << 8);
        return hit;
    }
    void screenDrag()
    {
        //Vector3 refPoint = new Vector3();
        //if (Input.GetMouseButtonDown(0)) 
        //    refPoint = Input.mousePosition;

        //Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - refPoint);
        //Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

        //Camera.main.transform.Translate(move, Space.World);
        Vector3 CameraPos;

        float MouseX;
        float MouseY;

        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");
        CameraPos = new Vector3(-MouseX, -MouseY, 0);

        Camera.main.transform.position += CameraPos;

    }
}

