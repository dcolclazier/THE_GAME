using System.Collections.Generic;
using System.Linq;
using Assets.Code.Entities;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Movement {
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

        public IEnumerable<Vector3> GetBestPath(Vector2 target, Entity entity) {

            if (TargetNode == null) TargetNode = new Node(target, entity);
            else TargetNode.Position = target;

            var openQueue = new Queue<Node>(); 
            var closedList = new List<Node>(); 

            openQueue.Enqueue(SourceNode); 
        
            while (openQueue.Any()) {
                var scrub = false;
                var sortedQueue = new Queue<Node>(openQueue.OrderBy(z => z.TotalScoreF));
                openQueue = sortedQueue;
                var current = openQueue.Dequeue();
                UpdateLosToTarget(current);
                foreach (var neighbor in current.GetNeighbors()) {
                    if(neighbor.PathDistanceG > current.PathDistanceG + neighbor.DistanceTo(current) || neighbor.PathDistanceG == 0f)
                    {
						neighbor.CameFrom = current;
						neighbor.PathDistanceG = current.PathDistanceG + neighbor.DistanceTo(current);
						neighbor.GuessH = neighbor.DistanceTo(TargetNode);
						neighbor.TotalScoreF = neighbor.PathDistanceG + neighbor.GuessH;
					}
                    
					if (neighbor == TargetNode) {
						neighbor.CameFrom = current;
						return BuildPath(neighbor);
                    }

                    foreach (var node in openQueue) {
                        if (node == neighbor)
                            scrub = true;
						if (node.Equals(neighbor))
						    scrub = true;
                    }
                   
                    if (closedList.Contains(neighbor)) {
                        continue;
                    }
                    if (scrub) continue;

                    openQueue.Enqueue(neighbor);
                }
                closedList.Add(current);
            }
            Debug.Log("No path found.");
            NodeManager.ClearNodes();
            return new Vector3[0];
        }

        private IEnumerable<Vector3> BuildPath(Node endpoint) {
            var path = new List<Node> {endpoint};
            
            while (endpoint != SourceNode) {
                if(!path.Contains(endpoint.CameFrom)) path.Add(endpoint.CameFrom);
                endpoint = endpoint.CameFrom;
            }
            
            NodeManager.ClearNodes();
            return ConvertToVectorArray(path);
        }

        private void UpdateLosToTarget(Node current) {

            if(current.CanSee(TargetNode)) current.AddOrUpdateNeighbor(TargetNode);
            else current.RemoveNeighbor(TargetNode);
        }

        public void UpdatePathGraph(Vector2 selectedPlayer) {
            
            SourceNode.Position = selectedPlayer;
            StaticNodes = EntityManager.GetAllSolidNodes().ToList();

            if (!StaticNodes.Contains(SourceNode)) StaticNodes.Add(SourceNode);

            foreach (var node in StaticNodes) {
                var node1 = node;
                foreach (var childnode in StaticNodes.Where(n => n.CanSee(node1))) {
                    node.AddOrUpdateNeighbor(childnode);
                }
            }

            foreach (var node in StaticNodes.Where(node => node.CanSee(SourceNode))) {
                SourceNode.AddOrUpdateNeighbor(node);
            }
        }
    }
}



