//using UnityEngine;
//using System.Collections;

//public class BufferMap : MonoBehaviour {

//    public Node[] polygonNodes;
//    public Node[] circleNodes;
//    public Node[] dynamicNodes;
//    public float buffer;
//    public GameObject map;

//    public BufferMap createBufferMap(float buffer)
//    {
//        BufferMap bufferMap = new BufferMap();
//        GameObject backGround = GameObject.Find("background");
//        bufferMap.map = GameObject.Instantiate(backGround);
//        bufferMap.buffer = buffer;

//        var noWalks = bufferMap.GetComponentsInChildren<PolygonCollider2D>();
//        var noWalkCircles = bufferMap.GetComponentsInChildren<CircleCollider2D>();
//        foreach (CircleCollider2D cc in noWalkCircles)
//        {
//            cc.radius += buffer;
//            cc.gameObject.layer = 11;
//        }
//        foreach (PolygonCollider2D pc in noWalks)
//        {
//            //Node.expandPoly(pc, buffer);
//            pc.gameObject.layer = 11;
//        }
//        bufferMap.map.layer = 11;
//        bufferMap.map.SetActive(false);
//        return bufferMap;
//    }

//    public void setPolygonNodes()
//    {


//    }

//    public void setCircleNodes()
//    {

//    }

//}
