using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Code;
using Assets.Code.Abstract;

//public sealed class PathFindHelper : MonoBehaviour {
    
//}

public static class PathFind {
	
	private static GameObject testMap; 
	private static Node[] permNodes ;
    //private static PathFindHelper pathFindHelper = (new GameObject("PathFindHelper")).AddComponent<PathFindHelper>();

     
    
	static PathFind()
	{
        Debug.Log("Pathfind initialized.");
		//Pre-process collider expansions based on required buffers of units in scene and nodes for polygons
        GameObject map = createBufferMap(0.38f);
		permNodes = createPermNodes(map);
		testMap = map; //for testing only
        Debug.Log("Start PathFind().");
		//TODO figure out way to do this for each buffer map size needed
	}

    

    public static GameObject createBufferMap(float buffer)
	{
		GameObject backGround = GameObject.Find("background");
		GameObject BufferMap = GameObject.Instantiate(backGround);
		var noWalks = BufferMap.GetComponentsInChildren<PolygonCollider2D>();
	    var noWalkCircles = BufferMap.GetComponentsInChildren<CircleCollider2D>();
		foreach (var cc in noWalkCircles)
		{
			((CircleCollider2D)cc).radius += buffer;
			cc.gameObject.layer = 11;
		}
		foreach (PolygonCollider2D pc in noWalks)
		{
			Node.expandPoly(pc, buffer);
			pc.gameObject.layer = 11;
		}
		BufferMap.layer = 11;
		BufferMap.SetActive(false);
		return BufferMap;
	}

	 private static Node[] createPermNodes(GameObject map)
	{
		int i = 0;
        Collider2D[] noWalks = map.GetComponentsInChildren<Collider2D>();
		foreach(PolygonCollider2D pc in noWalks)
		{
			int n = pc.GetTotalPointCount();
			i += n;
		}
		Node[] permPolys = new Node[i];
		i=0;
		foreach(PolygonCollider2D pc in noWalks)
		{
			foreach (Vector2 v in pc.points)
			{
				permPolys[i] = new Node(v);
				i++;
			}
		}
		return permPolys;
	}

    


	//TODO need function for updating and adding dynamic units and props after each move to the buffer maps

	public static Vector3[] findPath(GameObject unit, Vector3 goal)
	{
		//create path and set first point as current location of unit
        Vector3[] path = new Vector3[99];
		CircleCollider2D circle = unit.GetComponentInChildren<CircleCollider2D>();
		path[0] = circle.transform.position;
        Debug.Log("Path: Step 1 complete.");

		//Get collider size of object that is path finding
		float buffer = circle.radius;   //figure out how to referance the buffermap created for that size
		GameObject bufferMap = testMap;
		Vector3 circPos = circle.transform.position;
		bufferMap.SetActive(true);
        Debug.Log("Path: Step 2 complete.");

		//Check if start point can see end point (if so no additional path finding required)

		Physics2D.raycastsStartInColliders = false; //TODO this is not a permenant solution
		if (Physics2D.Raycast(circle.transform.position, goal - circPos, Vector2.Distance(goal, circPos), 1 << 11))
		{Debug.Log("intial raycast hit something");}
		else
		if (Physics2D.Raycast(circle.transform.position, goal - circle.transform.position))
		{
			Debug.Log("intial raycast didn't hit something");
			path[1] = goal;
            Debug.Log("Found a path. Pathfind returning path now.");
			return path;
		}
        Debug.Log("Path: Path not found at first - lets start looking.");

		//else start iterrating through nodes to find the best path
		bool foundGoal = false;
		Node nextNode = new Node (circle.transform.position);
		List<Node> openList = new List<Node>();
		while (foundGoal == false)
		{
			nextNode.open = false;
			canSee (nextNode, testMap, goal, openList);
			//resort open list with new values
			foreach (Node node in openList) node.Value = node.pathDist + node.guess;
			openList.Sort((x, y) => x.Value.CompareTo(y.Value));
			nextNode = openList[0];
			openList.RemoveAt(0);
			//repeat until path to goal is found
			if (!Physics2D.Raycast(nextNode.position, (Vector2)goal - nextNode.position))
			{
				foundGoal = true;
				path = createPath(nextNode, goal);
				Debug.Log("Path Found");
			}
		}

		bufferMap.SetActive(false);
		return path;
	}

	private static void canSee (Node searchNode, GameObject bufferMap, Vector3 goal, List<Node> openList)
	{
		//create nodes for each circle collider 90 degrees off the angle pointing directly towards them
		List<Node> currentCircleNodes = new List<Node>();
		CircleCollider2D[] circTemp = testMap.GetComponentsInChildren<CircleCollider2D>();
		foreach (CircleCollider2D cc in circTemp)
		{
			//TODO ignore the temp circle collider of current unit
			Node[] temp = Node.circleNodes(cc, searchNode.position, 0);
			foreach (Node n in temp) currentCircleNodes.Add(n);
		}
		
		//check line of site to each node in list from start point
		foreach (Node node in permNodes) checkLOS(openList, searchNode, node, goal); //add all nodes with LOS to open list
		foreach(Node node in currentCircleNodes) checkLOS(openList, searchNode, node, goal); //add all nodes with LOS to open list
	}


	 private static void checkLOS(List<Node> openList, Node searchNode, Node node, Vector3 goal)
	{
		if (!Physics2D.Raycast(searchNode.position, node.position - searchNode.position, Mathf.Infinity, 1 << 11) && node.open == true)
		{ //if no collision between searching node and this node, and this node is not on closed list:
			float dist = Vector2.Distance(searchNode.position, node.position) + searchNode.pathDist; //dist of start point to searching node + dist from searching node to this node
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

	private static Vector3[] createPath(Node node, Vector3 goal)
	{
		int i = 1;
		bool start = false;
		Node newNode = node.pointTo;
		while (start == false)
		{
			i++;
			newNode = newNode.pointTo;
			if (newNode.pointTo == null) start = true;
		}

		Vector3[] path = new Vector3[i];
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
