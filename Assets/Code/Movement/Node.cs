using System;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public Vector2 Position { get; set; }


    public bool IsSource { get; private set; }
    
    public float TotalScoreF { get; set; } 
    public float PathDistanceG { get; set; }
    public float GuessH { get; set; }
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
    }
    public Node(Vector2 pos, bool isSource) {
        IsSource = isSource;
		if (IsSource) CameFrom = null;
        Position = pos;
        _neighbors = new List<Node>();
    }
   
    public float DistanceTo(Node target) {
        return Vector2.Distance(Position, target.Position);
    }

    public bool CanSee(Node node) {
        var rayCast = Physics2D.Raycast(Position, node.Position - Position, DistanceTo(node), 1 << 11);
        return rayCast.collider == null;
        //Debug.DrawRay(position, node.position - position, Color.green);
    } 

    public Vector3 ToVector3() {
        return new Vector3(Position.x, Position.y, 0);
    }

}
