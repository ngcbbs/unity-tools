using System.Collections.Generic;
using UnityEngine;

namespace UnityTools.Pathfinding {
    public interface IAlgorithm {
        List<Node> FindPath(Vector2Int start, Vector2Int goal);
    }
}