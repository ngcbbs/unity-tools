using System.Collections.Generic;
using UnityEngine;

namespace UnityTools.Pathfinding {
    public class AStarPathfinder : PathfindingAlgorithm {
        private static readonly Vector2Int[] Directions = {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
            new(-1, -1), new(-1, 1), new(1, -1), new(1, 1)
        };
        
        private readonly PriorityQueue<Node> _open = new();
        private readonly HashSet<Node> _closed = new();

        public AStarPathfinder(Grid grid) : base(grid) { }

        public override List<Node> FindPath(Vector2Int startIndex, Vector2Int goalIndex, bool optimize) {
            var start = Grid.GetNode(startIndex);
            var goal = Grid.GetNode(goalIndex);

            if (start == null || goal is not { IsWalkable: true })
                return null;

            _open.Clear();
            _closed.Clear();

            Grid.PrepareFindWay();

            _open.Enqueue(start, 0);

            while (_open.Count > 0) {
                var current = _open.Dequeue();

                if (current.Index == goal.Index)
                    return ReconstructPath(goal, optimize);

                _closed.Add(current);

                foreach (var neighbor in GetNeighbors(current)) {
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

        private List<Node> GetNeighbors(Node node) {
            var neighbors = new List<Node>();

            foreach (var dir in Directions) {
                var neighbor = Grid.GetNode(node.Index + dir);
                if (neighbor is { IsWalkable: true })
                    neighbors.Add(neighbor);
            }

            return neighbors;
        }
    }
}