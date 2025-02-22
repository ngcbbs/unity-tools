using System.Collections.Generic;
using UnityEngine;

namespace UnityTools.Pathfinding {
    public abstract class PathfindingAlgorithm : IAlgorithm {
        public virtual List<Node> FindPath(Vector2Int startIndex, Vector2Int goalIndex, bool optimize) => null;
        protected readonly Grid Grid;
        
        protected Dictionary<TerrainType, float> Weights { get; }

        protected PathfindingAlgorithm(Grid grid) {
            Grid = grid;
            Weights = new Dictionary<TerrainType, float> {
                { TerrainType.Normal, 1.0f },
                { TerrainType.Rough, 2.0f },
                { TerrainType.Water, 3.0f },
                { TerrainType.Road, 0.8f }
            };
        }
        
        protected List<Node> OptimizePath(List<Node> path) {
            if (path == null || path.Count <= 2)
                return path;

            var optimizedPath = new List<Node> { path[0] };
            int currentIndex = 0;

            while (currentIndex < path.Count - 1) {
                int furthestVisible = currentIndex + 1;

                // 현재 노드에서 보이는 가장 먼 노드 찾기
                for (int i = currentIndex + 2; i < path.Count; i++) {
                    if (IsPathClear(path[currentIndex], path[i])) {
                        furthestVisible = i;
                    }
                }

                optimizedPath.Add(path[furthestVisible]);
                currentIndex = furthestVisible;
            }

            return optimizedPath;
        }

        private bool IsPathClear(Node start, Node end) {
            var direction = (Vector2)(end.Index - start.Index);
            var distance = direction.magnitude;
            direction.Normalize();

            // 경로 상의 장애물 확인
            for (float i = 1; i < distance; i++) {
                Vector2 point = (Vector2)start.Index + direction * i;
                Vector2Int gridPoint = Vector2Int.RoundToInt(point);

                Node node = Grid.GetNode(gridPoint);
                if (node is not { IsWalkable: true })
                    return false;
            }

            return true;
        }
        
        protected List<Node> ReconstructPath(Node goalNode, bool optimizePath) {
            var path = new List<Node>();
            var current = goalNode;
            while (current != null) {
                path.Add(current);
                current = current.Parent;
            }

            path.Reverse();
            return optimizePath ? OptimizePath(path) : path;
        }
    }
}