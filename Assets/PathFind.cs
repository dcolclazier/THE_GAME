using UnityEngine;
using System.Collections;



public class PathFind : MonoBehaviour {

	private GameObject testMap;

	public void Start()
	{
		//Pre-process collider expansions based on required buffers of units in scene and nodes for polygons
		GameObject map = createBufferMap(0.38f);
		Node[] permNodes = createPermNodes(map);
		testMap = map; //for testing only
		//TODO figure out way to do this for each buffer map size needed
	}

	public GameObject createBufferMap(float buffer)
	{
		GameObject backGround = GameObject.Find("background");
		GameObject BufferMap = GameObject.Instantiate(backGround);
		Collider2D[] noWalks = GetComponentsInChildren<Collider2D>(BufferMap);
		foreach (CircleCollider2D cc in noWalks)
		{
			cc.radius += buffer;
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

	public Node[] createPermNodes(GameObject map)
	{
		int i = 0;
		Collider2D[] noWalks = GetComponentsInChildren<Collider2D>(map);
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

	public Vector3[] findPath(GameObject unit, Vector3 goal)
	{
		//create path and set first point as current location of unit
		Vector3[] path = new Vector3[99];
		CircleCollider2D circle = unit.GetComponentInChildren<CircleCollider2D>();
		path[0] = circle.transform.position;

		//Get collider size of object that is path finding
		float buffer = circle.radius;   //figure out how to referance the buffermap created for that size
		GameObject bufferMap = testMap;
		bufferMap.SetActive(true);

		//Check if start point can see end point (if so no additional path finding required)
		if (!Physics2D.Raycast(circle.transform.position, goal))
		{
			path[1] = goal;
			return path;
		}

		//Create list of nodes based off expanded no walk colliders in scene


		//temporary circle nodes


		//check line of site to each node in list from start point


		//set distance from start point for each visible node


		//set guess distance for each visibile node


		//add nodes to open list and sort by smallest value on top


		//pop first value off open list, check for los to goal


		//add node to closed list


		//recalc circle nodes based off direction to current node


		//check for los to each node not on closed list


		//if current this node.pathDist + dist to ceck node < check node.pathDist, override values


		//add to open list any nodes found with los not previously on list


		//resort open list with new values


		//repeat until path to goal is found




		return findPath;
	}


}
