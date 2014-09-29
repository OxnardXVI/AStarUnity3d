//=============================================================================
//  
// module:		AStar for Unity3d
// license:		GNU GPL
// author:		Chernomoretc Igor
// contacts:	oxnardxvi@gmail.com
//
//=============================================================================
 
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AStar
{
    /// <summary>
    ///     Automatically generates given number of nodes within given rectangle and generates links between nodes(only if there are not obstacles between them).
    /// </summary>
    public class AStarNodeBuilder
    {
        private int _nodesXCount;
        private int _nodesZCount;
        private float _nodesDistanceX;
        private float _nodesDistanceZ;
        private Vector3 _center;
        private Transform _parentTr;

        public IAStarNode[,] Build(int nodesXCount, 
            int nodesZCount, 
            float nodesDistanceX,
            float nodesDistanceZ, 
            Vector3 center,
            Transform parentTr)
        {
            //init private fields
            _nodesXCount = nodesXCount;
            _nodesZCount = nodesZCount;
            _nodesDistanceX = nodesDistanceX;
            _nodesDistanceZ = nodesDistanceZ;
            _center = center;
            _parentTr = parentTr;

            //create nodes
            var nodes = new IAStarNode[_nodesXCount, _nodesZCount];
            for (int i = 0; i < _nodesXCount; i++)
            {
                for (int j = 0; j < _nodesZCount; j++)
                {
                    var node = new GameObject("_node_" + (i * _nodesZCount + j)).AddComponent<AStarNode>();
                    node.transform.position = GetNodePos(i, j);
                    node.transform.parent = _parentTr;

                    nodes[i, j] = node;
                }
            }

            //generates links between them
            for (int i = 0; i < _nodesXCount; i++)
            {
                for (int j = 0; j < _nodesZCount; j++)
                {
                    nodes[i, j].RemoveAllEdges();

                    nodes[i, j].AddEdges(GetNodeNeighbors(nodes, i, j).
                        Where(t => !CheckObstacles(nodes[i, j], t)).
                        Select(t => new AStarEdge(nodes[i, j] as AStarNode, t as AStarNode, Vector3.Distance(nodes[i, j].Position, t.Position))).
                        ToArray());
                }
            }

            return nodes;
        }

        private Vector3 GetNodePos(int nodeXInd, int nodeZInd)
        {
            return new Vector3(_center.x + _nodesDistanceX * (nodeXInd) - _nodesDistanceX * (_nodesXCount-1) * 0.5f,
                _center.y,
                _center.z + _nodesDistanceZ * (nodeZInd) - _nodesDistanceZ * (_nodesZCount - 1) * 0.5f);
        }

        private IAStarNode[] GetNodeNeighbors(IAStarNode[,] nodes, int nodeXInd, int nodeZInd)
        {
            var neighbors = new List<IAStarNode>();

            var nodesXCount = nodes.GetLength(0);
            var nodesZCount = nodes.GetLength(1);

            var xMinInd = Mathf.Max(0, nodeXInd - 1);
            var xMaxInd = Mathf.Min(nodesXCount - 1, nodeXInd + 1);
            var zMinInd = Mathf.Max(0, nodeZInd - 1);
            var zMaxInd = Mathf.Min(nodesZCount - 1, nodeZInd + 1);

            for (int i = xMinInd; i <= xMaxInd; i++)
            {
                for (int j = zMinInd; j <= zMaxInd; j++)
                {
                    neighbors.Add(nodes[i, j]);
                }
            }

            //remove self from neighbors
            neighbors.Remove(nodes[nodeXInd, nodeZInd]);

            return neighbors.ToArray();
        }

        /// <summary>
        ///     Is there any obstacles between two given nodes.
        /// </summary>
        /// <returns>False if there no obstacles between nodes(nodes can be linked)</returns>
        private bool CheckObstacles(IAStarNode node1, IAStarNode node2)
        {
            var node1Pos = node1.Position;
            var node2Pos = node2.Position;
            var disnace = Vector3.Distance(node1Pos, node2Pos);

            return Physics.Raycast(node1Pos, (node2Pos - node1Pos).normalized, disnace) ||
                   Physics.Raycast(node2Pos, (node1Pos - node2Pos).normalized, disnace);
        }
    }
}
