using System.Collections.Generic;
using UnityEngine;

namespace UnityTools.Pathfinding {
    public class JumpPointSearchPathfinder : PathfindingAlgorithm {
        private static readonly Vector2Int[] Directions = {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
            new(-1, -1), new(-1, 1), new(1, -1), new(1, 1)
        };

        private readonly PriorityQueue<Node> _open = new();
        private readonly HashSet<Node> _closed = new();

        public JumpPointSearchPathfinder(Grid grid) : base(grid) { }

        public override List<Node> FindPath(Vector2Int startIndex, Vector2Int goalIndex, bool optimize) {
            var start = grid.GetNode(startIndex);
            var goal = grid.GetNode(goalIndex);

            if (start == null || goal is not { IsWalkable: true })
                return null;

            _open.Clear();
            _closed.Clear();

            _open.Enqueue(start, 0);

            while (_open.Count > 0) {
                var current = _open.Dequeue();

                if (current == goal)
                    return ReconstructPath(goal, optimize);

                _closed.Add(current);

                foreach (var neighbor in GetJumpPoints(current, goal)) {
                    if (_closed.Contains(neighbor))
                        continue;

                    // 지형 가중치를 고려한 G 비용 계산
                    var terrainWeight = Weights[neighbor.TerrainType];
                    var gCost = current.G + Vector2Int.Distance(current.Index, neighbor.Index) * terrainWeight;
                    if (_open.Contains(neighbor) && !(gCost < neighbor.G))
                        continue;

                    neighbor.G = gCost;
                    neighbor.H = Vector2Int.Distance(neighbor.Index, goal.Index);
                    neighbor.Parent = current;

                    if (!_open.Contains(neighbor))
                        _open.Enqueue(neighbor, neighbor.F);
                }
            }

            return null;
        }

        private List<Node> GetJumpPoints(Node current, Node goal) {
            var jumpPoints = new List<Node>();

            foreach (var direction in Directions) {
                var next = grid.GetNode(current.Index + direction);
                while (next is { IsWalkable: true }) {
                    if (next.Index == goal.Index || HasForcedNeighbor(next, direction)) {
                        jumpPoints.Add(next);
                        break;
                    }

                    next = grid.GetNode(next.Index + direction);
                }
            }

            return jumpPoints;
        }

        private bool HasForcedNeighbor(Node node, Vector2Int direction) {
            var leftIndex = new Vector2Int(-direction.y, direction.x);
            var rightIndex = new Vector2Int(direction.y, -direction.x);

            var left = grid.GetNode(node.Index + leftIndex);
            var right = grid.GetNode(node.Index + rightIndex);

            var isForced = left is { IsWalkable: false } || right is { IsWalkable: false };

            // 대각선 이동 시 추가적인 강제 이웃 검사
            if (direction.x == 0 || direction.y == 0)
                return isForced;

            var diagonalLeft = grid.GetNode(node.Index + new Vector2Int(direction.x, 0));
            var diagonalRight = grid.GetNode(node.Index + new Vector2Int(0, direction.y));
            isForced |= diagonalLeft is { IsWalkable: false } ||
                        diagonalRight is { IsWalkable: false };

            return isForced;
        }
    }
}