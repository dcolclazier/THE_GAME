﻿using System.Collections.Generic;
using System.Linq;
using Assets.Code;
using Assets.Code.Abstract;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Components.Movement {
    public class PathMap {
        private List<Node> StaticNodes { get; set; }
        private Node SourceNode { get; set; }
        public Node TargetNode { get; set; }

        private static IEnumerable<Vector3> ConvertToVectorArray(ICollection<Node> nodes) {
            var vectors = new Vector3[nodes.Count];
            var i = 0;
            foreach (var node in nodes) {
                vectors[i] = node.ToVector3();
                i++;
            }
            return vectors;
        }
        public PathMap(Node from) {
            StaticNodes = EntityManager.GetAllSolidNodes().ToList();
            SourceNode = from;
            TargetNode = null;
            Messenger.AddListener<Entity>("ObstructionAdded", EntityNodesChanged);
            Messenger.AddListener<Entity>("ObstructionRemoved", EntityNodesChanged);
        }

        private void EntityNodesChanged(Entity entity) {
            StaticNodes = EntityManager.GetAllSolidNodes().ToList();
        }

        public IEnumerable<Vector3> GetBestPath() {

            //A* algorithm

            var openQueue = new Queue<Node>(); // think of a queue like a line at the grocery store - first in, first out.
            var closedList = new List<Node>(); 

            openQueue.Enqueue(SourceNode); //adds starting point to the queue
        
             // make sure any nodes that can see target have it as their neighbor
            int i = 0;
            while (openQueue.Any()) {
                var scrub = false;
                i++;
                if (i == 100) {
                    Debug.Log("Infinite Loop detected");
                    break;
                }
               // Debug.Log("while loop ran " + i + " times");
                //sort the queue by TotalScoreF
                var sortedQueue = new Queue<Node>(openQueue.OrderBy(z => z.TotalScoreF));
                openQueue = sortedQueue;
                //pull the first element in line out of the queue
                var current = openQueue.Dequeue();
                Debug.Log("Open queue count " + openQueue.Count);
                UpdateLOSToTarget(current);
                Debug.Log("Current neighbor count? " + current.GetNeighbors().Count());
                foreach (var neighbor in current.GetNeighbors()) {
                    //Debug.Log("Neighbor's neighbor count? " + neighbor.GetNeighbors().Count());
                    if(neighbor.PathDistanceG > current.PathDistanceG + neighbor.DistanceTo(current) || neighbor.PathDistanceG == 0f)
                    {
                        //Debug.Log("Got here.");
						neighbor.CameFrom = current;
						neighbor.PathDistanceG = current.PathDistanceG + neighbor.DistanceTo(current);
						neighbor.GuessH = neighbor.DistanceTo(TargetNode);
						neighbor.TotalScoreF = neighbor.PathDistanceG + neighbor.GuessH;
					}
                    
					if (neighbor == TargetNode) {
						neighbor.CameFrom = current;
						return BuildPath(neighbor);
                    }
                  

                    //var tempNeighbor = neighbor;

                    foreach (var node in openQueue) {
                        if (node.Position == neighbor.Position)
                            scrub = true;
						if (node.Equals(neighbor))
						    scrub = true;
                    }
                    /*
					foreach (var node in closedList) {
                        if (node.Position == neighbor.Position && node.TotalScoreF < neighbor.TotalScoreF) 
                            scrub = true;
						if (node.Equals(tempNeighbor))
							scrub = true;
                    }
                    */

                    if (closedList.Contains(neighbor))
                    {
                        Debug.Log("Found a node in the closed list!");
                        continue;
                    }
                    if (scrub) continue;

                    openQueue.Enqueue(neighbor);
                }
                closedList.Add(current);
            }
            Debug.Log("No path found.");
            return new Vector3[0];
        }

        private IEnumerable<Vector3> BuildPath(Node endpoint) {
            var path = new List<Node> {endpoint};
            int i = 0;
            while (endpoint.CameFrom != null) {
                i++;
				Debug.Log("Number of while loops executed in BuildPath:  " + i); 
                if (i == 10) {
                    Debug.Log("BUG!!! Max number of path segments reached");
                    break;
                }
                if (!path.Contains(endpoint.CameFrom)) path.Add(endpoint.CameFrom); //stop it from adding same point multiple times
                if (endpoint.IsSource) break;
                endpoint = endpoint.CameFrom;
            }
            path.Add(SourceNode);
        
            return ConvertToVectorArray(path);
        }

        private void UpdateLOSToTarget(Node current) {

            //if the source node can see the target, make sure it is a neighbor. If it can't, remove it if it exists.
            if (SourceNode.CanSee(TargetNode)) SourceNode.AddOrUpdateNeighbor(TargetNode);
            else SourceNode.RemoveNeighbor(TargetNode); 
        
            if(current.CanSee(TargetNode)) current.AddOrUpdateNeighbor(TargetNode);
            else current.RemoveNeighbor(TargetNode);
            //update every node in Static nodes - if it can see the target, add it to neighbors.
            // if not, remove it from neighbors if it exists.
            //foreach (var node in StaticNodes) {
            //    if (node.CanSee(TargetNode)) node.AddOrUpdateNeighbor(TargetNode);
            //    else node.RemoveNeighbor(TargetNode);
            //}
        
        }

        public void UpdatePathGraph(Vector2 selectedPlayer) {
            //update SourceNode position
            SourceNode.Position = selectedPlayer;

            //Update all solid nodes
            StaticNodes = EntityManager.GetAllSolidNodes().ToList();

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

        public void Clear() {
            
        }
    }
}



