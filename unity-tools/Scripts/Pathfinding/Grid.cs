using System.Collections.Generic;
using UnityEngine;

namespace UnityTools.Pathfinding {
    public class Grid {
        private readonly Dictionary<Vector2Int, Node> _nodes = new(64 * 64);

        public void Clear() {
            _nodes.Clear();
        }

        public Node GetNode(Vector2Int index) {
            return _nodes.GetValueOrDefault(index);
        }

        public void SetWalkable(Vector2Int index, Vector3 position, bool isWalkable) {
            if (_nodes.TryGetValue(index, out var node))
                node.IsWalkable = isWalkable;
            else
                _nodes.Add(index, new Node(index, position, isWalkable));
        }
    }
}
