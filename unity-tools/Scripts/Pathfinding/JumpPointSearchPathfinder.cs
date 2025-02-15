using System.Collections.Generic;
using UnityEngine;

namespace UnityTools.Pathfinding {
    public class JumpPointSearchPathfinder : IAlgorithm {
        private readonly Grid _grid;

        private static readonly Vector2Int[] Directions = {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
            new Vector2Int(-1, -1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(1, 1)
        };

        public JumpPointSearchPathfinder(Grid grid) {
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

                if (current == goalNode)
                    return ReconstructPath(goalNode);

                foreach (var neighbor in GetJumpPoints(current, goalNode)) {
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

        private List<Node> GetJumpPoints(Node current, Node goal) {
            List<Node> jumpPoints = new List<Node>();

            foreach (var direction in Directions) {
                Node next = _grid.GetNode(current.Index + direction);
                while (next != null && next.IsWalkable) {
                    if (next.Index == goal.Index || HasForcedNeighbor(next, direction)) {
                        jumpPoints.Add(next);
                        break;
                    }

                    next = _grid.GetNode(next.Index + direction);
                }
            }

            return jumpPoints;
        }

        private bool HasForcedNeighbor(Node node, Vector2Int direction) {
            Vector2Int left = new Vector2Int(-direction.y, direction.x);
            Vector2Int right = new Vector2Int(direction.y, -direction.x);

            Node leftNode = _grid.GetNode(node.Index + left);
            Node rightNode = _grid.GetNode(node.Index + right);

            bool isForced = (leftNode != null && !leftNode.IsWalkable) || (rightNode != null && !rightNode.IsWalkable);

            // 대각선 이동 시 추가적인 강제 이웃 검사
            if (direction.x != 0 && direction.y != 0) {
                Node diagonalLeft = _grid.GetNode(node.Index + new Vector2Int(direction.x, 0));
                Node diagonalRight = _grid.GetNode(node.Index + new Vector2Int(0, direction.y));
                isForced |= (diagonalLeft != null && !diagonalLeft.IsWalkable) ||
                            (diagonalRight != null && !diagonalRight.IsWalkable);
            }

            return isForced;
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