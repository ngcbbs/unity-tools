using UnityEngine;

namespace UnityTools.Pathfinding {
    // 2d?
    public class Node {
        public Vector2Int Index;
        public Vector3 Position { get; private set; }
        public bool IsWalkable;
        public Node Parent;
        public float G, H; // G: 시작점부터 거리, H: 휴리스틱(목표점 거리)
        public float F => G + H; // F = G + H

        public bool Fill = false;

        public Node(Vector2Int index, Vector3 position, bool walkable) {
            Index = index;
            Position = position;
            IsWalkable = walkable;
        }
    }
}