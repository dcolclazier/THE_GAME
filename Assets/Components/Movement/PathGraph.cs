using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Code;
using Assets.Code.Entities;
using Vectrosity;

public class PathGraph {
    public List<Node> StaticNodes { get; set; }
    public Node SourceNode { get; private set; }
    public Node TargetNode { get; set; }

    public PathGraph(Node from) {
        StaticNodes = NodeManager.GetAllSolidNodes(0f).ToList();
        SourceNode = from;
        TargetNode = null;
    }
    public IEnumerable<Vector3> GetBestPath() {

        //A* algorithm

        var openQueue = new Queue<Node>();
        var closedList = new List<Node>();

        openQueue.Enqueue(SourceNode);
        
        UpdateTargetNode();
        while (openQueue.Any()) {

            var sortedQueue = new Queue<Node>(openQueue.OrderBy(z => z.TotalScoreF));
            openQueue = sortedQueue;
            var current = openQueue.Dequeue();
            foreach (var neighbor in current.GetNeighbors()) {
                neighbor.CameFrom = current;
                
                if (neighbor == TargetNode) {
                    return BuildPath(neighbor);
                }

                neighbor.PathDistanceG = current.PathDistanceG + neighbor.DistanceTo(current);
                neighbor.GuessH = neighbor.DistanceTo(TargetNode);
                neighbor.TotalScoreF = neighbor.PathDistanceG + neighbor.GuessH;

                var openSimiliar = openQueue.Where(p => p.Position == neighbor.Position).Where(p => p.PathDistanceG < neighbor.PathDistanceG);
                if (openSimiliar.Any())
                {
                    Debug.Log("Open similiar triggered...");
                    continue;
                }
                var closedSimiliar = closedList.Where(p => p.Position == neighbor.Position).Where(p => p.PathDistanceG < neighbor.PathDistanceG);
                if (closedSimiliar.Any())
                {
                    Debug.Log("Closed Similiar triggered");
                    continue;
                }
                if (closedList.Contains(neighbor)) continue;
                openQueue.Enqueue(neighbor);
            }
            closedList.Add(current);
        }
        Debug.Log("No path found.");
        return new Vector3[0];
    }
    private IEnumerable<Vector3> BuildPath(Node endpoint) {
        //bug - UGH! This bug has been bugging me forever...
        //bug - seems to be creating infinitly long paths. :-\
        var path = new List<Node> {endpoint};
        
        int i = 0;
        
        while (endpoint.CameFrom != SourceNode) {
            i++;
            if (i == 10) {
                Debug.Log("BUG!!! BuildPath had more than 10 endpoints or infinite loop.");
                break;
            }
			if (!path.Contains(endpoint.CameFrom)) path.Add(endpoint.CameFrom);
            if (endpoint.IsSource) break;
            endpoint = endpoint.CameFrom;
            //Debug.Log("Is the current Parent we're looking at : Source?" + endpoint.CameFrom.IsSource);
        }
        path.Add(SourceNode);
        
        return UnityUtilites.ConvertToVectorArray(path);
    }
    public void UpdateTargetNode() {

        //if the source node can see the target, make sure it is a neighbor. If it can't, remove it if it exists.
        if (SourceNode.CanSee(TargetNode)) SourceNode.AddOrUpdateNeighbor(TargetNode);
        else SourceNode.RemoveNeighbor(TargetNode); 
        
        //update every node in Static nodes - if it can see the target, add it to neighbors.
        // if not, remove it from neighbors if it exists.
        foreach (var node in StaticNodes) {
            if (node.CanSee(TargetNode)) node.AddOrUpdateNeighbor(TargetNode);
            else node.RemoveNeighbor(TargetNode);
        }
        
    }
    public void UpdatePathGraph(Vector2 selectedPlayer) {
        //update SourceNode position
        SourceNode.Position = selectedPlayer;

        //Update all solid nodes
        StaticNodes = NodeManager.GetAllSolidNodes(0f).ToList();

        //If the list doesn't contain the sourcenode, add it.
        if (!StaticNodes.Contains(SourceNode)) StaticNodes.Add(SourceNode);

        //for each node in Static Nodes, update list of neighbors with other nodes
        //which are in LOS
        foreach (var node in StaticNodes) {
            var node1 = node;
            foreach (var childnode in StaticNodes.Where(n => n.CanSee(node1))) {
                node.AddOrUpdateNeighbor(childnode);
            }
        }

        //for each node that can see the sourceNode, make sure the sourcenode has it as a neighbor.
        foreach (var node in StaticNodes.Where(node => node.CanSee(SourceNode))) {
            SourceNode.AddOrUpdateNeighbor(node);
        }
    }
}



