using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PathSimple  {

	//this version of the pathfind does not use a Buffermap, instead it works
	//as if all the colliders already present on the background (that a 
	//buffer map would normally derive from) are what it needs to path
	//around.  Additionally it will only test itself against the circle
	//colliders in the scene, and will not path around polygon colliders.

	private static List<Node> gizmoCircle; //for debug use only

	static void OnDrawGizmos()
	{
		foreach(Node node in gizmoCircle) Gizmos.DrawWireSphere(node.position, 0.1f);
	}

	public static Vector3[] findPath(GameObject unit, Vector3 goal)
	{
		CircleCollider2D unitCollider = unit.GetComponentInChildren<CircleCollider2D>();
		Vector3 start = unitCollider.transform.position;

		//see if start point can see goal without pathfinding needed
		if (Physics2D.Raycast(start, goal - start, Vector2.Distance(goal, start), 1 << 10))
		{
			Debug.Log("intial raycast hit something");
		}
		else 
		{
			Debug.Log("intial raycast didn't hit something");
			Vector3[] path = new Vector3[2];
			path[0] = start;
			path[1] = goal;
			return path;
		}

		//else start iterrating through nodes to find the best path
		bool foundGoal = false;
		Node nextNode = new Node (start);
		List<Node> openList = new List<Node>();
		int i = 0;
		while (foundGoal == false)
		{
			i++;
			if (i==100)
			{
				Debug.Log("Infinate Loop detected");
				foundGoal = true;
				return new Vector3[0];
			}

			nextNode.open = false;
			canSee (nextNode, goal, openList); 

			//resort open list with new values
			foreach (Node node in openList) node.Value = node.pathDist + node.guess;
			if(openList.Count > 0)
			{
				openList.Sort((x, y) => x.Value.CompareTo(y.Value));
				Debug.Log("Open List size prior to picking new next node: " + openList.Count);
				nextNode = openList[0];
				openList.RemoveAt(0);
				Debug.Log("Open List size after picking new next node: " + openList.Count);
			}
			else
			{
				Debug.Log("Open List empty, no Path Found");
				foundGoal = true;
				return new Vector3[0];
			}

			//see if next Node can see the goal, if not repeat the process of having it check LOS to all nodes
			if (Physics2D.Raycast(nextNode.position, (Vector2)goal - nextNode.position, Vector2.Distance(nextNode.position, goal), 1 << 10))
			{
				//do nothing
			}
			else
			{
				foundGoal = true;
				Vector3[] path = createPath(nextNode, goal);
				Debug.Log("Path Found");
				return path;
			}
		}
		Debug.Log("No path found");
		return new Vector3[0];
	}
	

	public static void canSee (Node searchNode, Vector3 goal, List<Node> openList)
	{
		//create nodes for each circle collider 90 degrees off the angle pointing directly towards them
		List<Node> currentCircleNodes = new List<Node>();
		CircleCollider2D[] circTemp = GameObject.Find("background").GetComponentsInChildren<CircleCollider2D>();
		Debug.Log("Number of circle colliders found: " + circTemp.Length);
		foreach (CircleCollider2D cc in circTemp)
		{
			Node[] temp = Node.circleNodes(cc, searchNode.position, 0.1f); //create two nodes for each circle collider
			foreach (Node n in temp) 
			{
				if(Physics2D.OverlapPoint(n.position, 1 << 10)) {} //if this node position is inside another collider, dont add it to list
				else currentCircleNodes.Add(n); //add each created Node to the list of Nodes currentCircleNodes
			}
		}
		gizmoCircle = currentCircleNodes;  //debug use only
		Debug.Log("Number of circle nodes: " + currentCircleNodes.Count);

		//check line of site to each node in list from start point
		foreach(Node node in currentCircleNodes) checkLOS(openList, searchNode, node, goal); //add all nodes with LOS to open list
	}

    public static void checkLOS(List<Node> openList, Node searchNode, Node node, Vector3 goal)
	{
		float dist = Vector2.Distance(searchNode.position, node.position);
		if (Physics2D.Raycast(searchNode.position, node.position - searchNode.position, dist, 1 << 10))
		{
			//nothing happens with this node if the searching node has it's LOS to it blocked by a collider
			Debug.Log("Check LOS hit something, so node not added to open List");
		}
		else if (node.open == true)
		{ //if no collision between searching node and this node, and this node is not on closed list:
			Debug.Log("Raycast didn't hit anything, add Node to open list??");
			dist += searchNode.pathDist; //dist of start point to searching node + dist from searching node to this node
			if (node.pathDist == 0) //if first time this node has been found
			{
				node.pathDist = dist;
				node.pointTo = searchNode;
				openList.Add(node);
				node.guess = Vector2.Distance(node.position, goal);
			}
			else if (dist < node.pathDist) //override current path on this node of currently searching path is shorter
			{
				node.pathDist = dist;
				node.pointTo = searchNode;
			}
		}
	}


	public static Vector3[] createPath(Node node, Vector3 goal)
	{
		int i = 2;
		bool start = false;
		Node newNode = node.pointTo;
		while (start == false)
		{
			i++;
			if (newNode.pointTo == null) start = true;
			newNode = newNode.pointTo;
		}
		
		Vector3[] path = new Vector3[i];
		i--;
		path[i] = goal;
		i--;
		path[i] = (Vector3)node.position;
		
		start = false;
		newNode = node.pointTo;
		while (start == false)
		{
			i--;
			path[i] = (Vector3)newNode.position;
			if (i==0) 
			{
				start = true;
				return path;
			}
			newNode = newNode.pointTo;
		}
		return path;
	}

}
