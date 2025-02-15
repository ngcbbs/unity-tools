using System.Collections.Generic;
using UnityEngine;

namespace UnityTools.Pathfinding {
    public class AStarPathfinder : IAlgorithm {
        private readonly Grid _grid;

        private static readonly Vector2Int[] Directions = {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
            new Vector2Int(-1, -1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(1, 1)
        };

        public AStarPathfinder(Grid grid) {
            _grid = grid;
        }

        public List<Node> FindPath(Vector2Int start, Vector2Int goal) {
            Node startNode = _grid.GetNode(start);
            Node goalNode = _grid.GetNode(goal);

            if (startNode == null || goalNode == null || !goalNode.IsWalkable)
                return null;

            List<Node> openSet = new List<Node> { startNode };
            HashSet<Node> closedSet = new HashSet<Node>();

            while (openSet.Count > 0) {
                Node current = openSet[0];
                foreach (var node in openSet)
                    if (node.F < current.F)
                        current = node;

                openSet.Remove(current);
                closedSet.Add(current);

                if (current.Index == goalNode.Index)
                    return ReconstructPath(goalNode);

                foreach (var neighbor in GetNeighbors(current)) {
                    if (closedSet.Contains(neighbor))
                        continue;

                    float gCost = current.G + Vector2Int.Distance(current.Index, neighbor.Index);
                    if (!openSet.Contains(neighbor) || gCost < neighbor.G) {
                        neighbor.G = gCost;
                        neighbor.H = Vector2Int.Distance(neighbor.Index, goalNode.Index);
                        neighbor.Parent = current;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            return null;
        }

        private List<Node> GetNeighbors(Node node) {
            List<Node> neighbors = new List<Node>();

            foreach (var dir in Directions) {
                Node neighbor = _grid.GetNode(node.Index + dir);
                if (neighbor != null && neighbor.IsWalkable)
                    neighbors.Add(neighbor);
            }

            return neighbors;
        }

        private List<Node> ReconstructPath(Node goalNode) {
            List<Node> path = new List<Node>();
            Node current = goalNode;
            while (current != null) {
                path.Add(current);
                current = current.Parent;
            }

            path.Reverse();
            return path;
        }
    }
}