using UnityEngine;
using System.Collections;
using Node;

public class PathFind : MonoBehaviour {

public Vector3[] findPath(GameObject unit, Vector3 goal)
	{
		//Get collider size of object that is path finding
		//Expand no walk colliders in scene to buffer radius of walking unit
		//Check if start point can see end point (if so no additional path finding required)
		//Create list of nodes based off expanded no walk colliders in scene
		//Polygon nodes can stay in memory for future use
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

	}


}
