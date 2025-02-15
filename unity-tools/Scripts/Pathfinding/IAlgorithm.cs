using System.Collections.Generic;
using UnityEngine;

namespace UnityTools.Pathfinding {
    public interface IAlgorithm {
        List<Node> FindPath(Vector2Int startIndex, Vector2Int goalIndex, bool optimize = true);
    }
}