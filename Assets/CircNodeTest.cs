using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircNodeTest : MonoBehaviour {

	private List<Node> currentCircleNodes = new List<Node>();

	void Update()
	{
		if (Input.GetMouseButton(1))
		{
			Vector3 pos = gameObject.transform.position;

			CircleCollider2D[] circTemp = GameObject.Find("background").GetComponentsInChildren<CircleCollider2D>();

			foreach (CircleCollider2D cc in circTemp)
			{
				Node[] temp = Node.circleNodes(cc, pos, 0.1f); //create two nodes for each circle collider
				foreach (Node n in temp) 
				{
					if(Physics2D.OverlapPoint(n.position, 1 << 10)) {} //if this node position is inside another collider, dont add it to list
					else currentCircleNodes.Add(n); //add each created Node to the list of Nodes currentCircleNodes
				}
			}
		}
	}


	void OnDrawGizmos()
	{
		foreach (Node node in currentCircleNodes)
		{
			Gizmos.DrawSphere(node.position, 0.1f);
		}
	}


}
