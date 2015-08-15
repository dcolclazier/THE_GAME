using UnityEngine;
using System.Collections;

public class Node {

	public Vector2 position;
	public float guess = 0;
	public float pathDist = 0;
	public float Value;
	public Node pointTo;
	public bool open = true;

	public Node (Vector2 pos)
	{
		position = pos;
	}


	public static Vector2 polygonCenter(Vector2[] points)
	{
		Vector2 pos = Vector2.zero;
		foreach (Vector2 v in points)
		{
			pos += v;
		}
		return pos / points.Length;
	}

	public static Node[] polygonNodes(PolygonCollider2D poly)
	{
		Vector2 center = polygonCenter(poly.points);
        Debug.Log(string.Format("CenterX: {0}, CenterY: {1}",center.x,center.y));

		Vector2[] polyPoints = poly.points;
        Debug.Log("Length of poly points: " + poly.points);

        //Debug.Log(string.Format("PolyX: {0}, PolyY: {1}", polyPoints, center.y));
		Node[] nodeList = new Node[polyPoints.Length];
        Debug.Log("PolyPoints.length: "+ polyPoints.Length);
		foreach(Vector2 vect in polyPoints)
		{
			int size = nodeList.Length;
			nodeList[size-1] = new Node(vect);
		}
		return nodeList;
	    return null;
	}

	public static Node[] circleNodes(CircleCollider2D circ, Vector2 from, float buffer)
	{
		Node[] nodeList = new Node[2];
		Vector2 center = (Vector2)circ.transform.position;
		Ray2D ray = new Ray2D(center, from - center);
		Quaternion q = Quaternion.AngleAxis(90, Vector2.up);
		ray.direction = q * ray.direction;
		nodeList[0] = new Node(ray.GetPoint(buffer));
		nodeList[1] = new Node(ray.GetPoint(-buffer));
		return nodeList;
	}

	public static void expandPoly(PolygonCollider2D poly, float buffer)
	{
		Vector2 center = polygonCenter(poly.points);
		Vector2[] polyPoints = poly.points;
		for (int i=0; i<polyPoints.Length; i++)
		{
			Ray2D ray = new Ray2D(polyPoints[i], polyPoints[i] - center);
			polyPoints[i] = ray.GetPoint(buffer);
		}
		poly.points = polyPoints;
	}

}
