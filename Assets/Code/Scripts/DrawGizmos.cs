using Assets.Code.Abstract;
using UnityEngine;
using Assets.Code.Statics;

public class DrawGizmos : MonoBehaviour {

    protected void Awake() {
       
    }

    void OnDrawGizmos() {
        int i = 0;
        foreach (var node in EntityManager.GetAllSolidNodes() ) {
            Gizmos.DrawSphere(node.Position, 0.1f);
            //GUI.TextField(new Rect(
            //    node.Position, new Vector2(1, 2)), "Test");
            TextGizmo.Draw(node.Position + new Vector2(0,0), "(F)" + node.TotalScoreF);
            //TextGizmo.Draw(node.Position + new Vector2(0,-1f), "(G)" + node.PathDistanceG);
            //TextGizmo.Draw(node.Position + new Vector2(0,-2f), "(H)"  + node.GuessH);
            i++;
        }
        Debug.Log("Drawing " + i + " nodes.");
    }


}
