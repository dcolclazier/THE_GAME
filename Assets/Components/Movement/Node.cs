using System;
using System.Collections.Generic;
using UnityEngine;

public class Node {

	public Vector2 position;


    public bool IsSource { get; private set; }
    
    public float TotalScoreF { get; set; } 
    public float PathDistanceG { get; set; }
    public float GuessH { get; set; }
	public Node CameFrom { get; set; }

    private readonly List<Node> _neighbors; 
    public void AddOrUpdateNeighbor(Node neighbor) {
        if (_neighbors.Contains(neighbor)) {
            _neighbors.Find(p=>p == neighbor).position = neighbor.position;
            return;
        }
        
        _neighbors.Add(neighbor);
    }
    //public void AddNeighbors(IEnumerable<Node> neighbors) {
    //    foreach (var neighbor in neighbors) {
    //        AddOrUpdateNeighbor(neighbor);
    //    }
    //}
    public IEnumerable<Node> GetNeighbors() {
        return _neighbors;
    }
    //public bool ContainsNeighbor(Node node) {
    //    return _neighbors.Contains(node);
    //}
    public void RemoveNeighbor(Node neighbor) {
        if (!_neighbors.Contains(neighbor)) return;
        _neighbors.RemoveAll(n => n == neighbor);
    }

    public Node(Vector2 pos) {
        position = pos;
        _neighbors = new List<Node>();
    }
    public Node(Vector2 pos, bool isSource) {
        IsSource = isSource;
		if (IsSource) CameFrom = null;
        position = pos;
        _neighbors = new List<Node>();
    }
   
    public float DistanceTo(Node target) {
        return Vector2.Distance(position, target.position);
    }

    public bool CanSee(Node node) {
        var rayCast = Physics2D.Raycast(position, node.position - position, DistanceTo(node), 1 << 10);
        if (rayCast.collider != null) {
            //if (node.IsSource) Debug.DrawRay(position, node.position - position, Color.red);
            return false;
        }
        //Debug.DrawRay(position, node.position - position, Color.green);
        return true;
    } 

    public Vector3 ToVector3() {
        return new Vector3(position.x, position.y, 0);
    }

    //public bool CanSee(Node node, out Collider2D collider) {
    //    collider = Physics2D.Raycast(position, node.position - position, DistanceTo(node), 1 << 10).collider;
    //    return CanSee(node);

    //}
}
