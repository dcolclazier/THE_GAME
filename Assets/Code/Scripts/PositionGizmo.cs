using UnityEngine;
using System.Collections;

public class PositionGizmo : MonoBehaviour {

	public float size;

	 public void OnDrawGizmo ()
	{
		Gizmos.DrawSphere(gameObject.transform.position, size);


	}
}
