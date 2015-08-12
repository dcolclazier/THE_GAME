using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

	public Vector3 position;  
	public float guess;
	public bool open;
	public float pathDist;
	public float Value;
	public Node pointTo;

	public Node (Vector3 pos)
	{
		position = pos;
	}


	public Vector3 polygonCenter(Vector3[] points)
	{
		Vector3 pos = Vector3.zero;
		foreach (Vector3 v in points)
		{
			pos += v;
		}
		return pos / points.Length;
	}

	public Node[] polygonNodes(PolygonCollider2D poly)
	{


	}



}
