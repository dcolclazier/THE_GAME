using System;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public Vector2 Position { get; set; }


    public bool IsSource { get; private set; }
    
    public int TotalScoreF { get; set; } 
    public float PathDistanceG { get; set; }
    public int GuessH { get; set; }
	public Node CameFrom { get; set; }

    private readonly List<Node> _neighbors; 
    public void AddOrUpdateNeighbor(Node neighbor) {
        if (_neighbors.Contains(neighbor)) {
            _neighbors.Find(p=>p == neighbor).Position = neighbor.Position;
            return;
        }
        
        _neighbors.Add(neighbor);
    }
  
    public IEnumerable<Node> GetNeighbors() {
        return _neighbors;
    }
  
    public void RemoveNeighbor(Node neighbor) {
        if (!_neighbors.Contains(neighbor)) return;
        _neighbors.RemoveAll(n => n == neighbor);
    }

    public Node(Vector2 pos) {
        Position = pos;
        _neighbors = new List<Node>();
		PathDistanceG = Mathf.Infinity;
    }
    public Node(Vector2 pos, bool isSource) {
        IsSource = isSource;
		if (IsSource) CameFrom = null;
        Position = pos;
        _neighbors = new List<Node>();
		PathDistanceG = Mathf.Infinity;
    }
   
    public float DistanceTo(Node target) {
        return Vector2.Distance(Position, target.Position);
    }

    public bool CanSee(Node node) {
        var rayCast = Physics2D.Raycast(Position, node.Position - Position, DistanceTo(node), 1 << 11);
        Debug.DrawRay(Position, node.Position - Position, rayCast.collider == null ? Color.green :Color.red);
        return rayCast.collider == null;
    } 

    public Vector3 ToVector3() {
        return new Vector3(Position.x, Position.y, 0);
    }

}
