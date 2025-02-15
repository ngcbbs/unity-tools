using System;
using UnityEngine;

namespace UnityTools.Pathfinding {
    // 2d?
    public class Node : IComparable<Node> {
        public Vector2Int Index;
        public bool IsWalkable;
        public Vector3 Position { get; private set; }
        public TerrainType TerrainType;
        public Node Parent;
        public float G, H; // G: 시작점부터 거리, H: 휴리스틱(목표점 거리)
        public float F => G + H; // F = G + H

        public bool Fill = false;

        public Node(Vector2Int index, Vector3 position, bool walkable, TerrainType type = TerrainType.Normal) {
            Index = index;
            Position = position;
            IsWalkable = walkable;
            TerrainType = type;
        }

        public int CompareTo(Node other) {
            if (other == null)
                throw new ArgumentException("Object is null");
            return F.CompareTo(other.F);
        }
    }
}