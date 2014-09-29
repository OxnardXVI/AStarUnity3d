//=============================================================================
//  
// module:		AStar for Unity3d
// license:		GNU GPL
// author:		Chernomoretc Igor
// contacts:	oxnardxvi@gmail.com
//
//=============================================================================
 
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    /// <summary>
    ///     Returns True if between two points there are some obstacles.
    /// </summary>
    public delegate bool CheckObstaclesFuncDel(Vector3 from, Vector3 to);

    /// <summary>
    ///     Path finder class. Searches for path between 2 nodes.
    /// </summary>
    public class AStarPathFinder
    {
        #region PathNodeInfo declaration

        /// <summary>
        ///   Internal class which contains aditional information about node. 
        ///   Used in path searching process.
        /// </summary>
        private class PathNodeInfo
        {
            /// <summary>
            ///     Move cost. Distance cost from start to current node
            /// </summary>
            public float G { get; set; }
            /// <summary>
            ///     Distance from a node to destination node(heuristic).
            /// </summary>
            public float H { get; set; }
            /// <summary>
            ///     F = G + H
            /// </summary>
            public float F 
            {
                get { return G + H; } 
            }
            /// <summary>
            ///     Parent node(node from which me moved to current node)
            /// </summary>
            public AStarNode Parent { get; set; }
            /// <summary>
            ///     Current node
            /// </summary>
            public AStarNode Node { get; private set; }

            public PathNodeInfo(AStarNode node)
            {
                Node = node;
            }
        }

        #endregion

        private Dictionary<AStarNode, PathNodeInfo> _pathNodesCache = new Dictionary<AStarNode,PathNodeInfo>();
        
        /// <summary>
        ///     List of nodes that need to be checked
        /// </summary>
        //private readonly LinkedList<AStarNode> _open = new LinkedList<AStarNode>();

        private readonly PriorityQueue<float, AStarNode> _open = new PriorityQueue<float, AStarNode>();

        /// <summary>
        ///     List on nodes thats have been already checked
        /// </summary>
        private readonly HashSet<AStarNode> _closed = new HashSet<AStarNode>();

        /// <summary>
        ///     Path destination node
        /// </summary>
        private AStarNode _destNode; 

        /// <summary>
        ///     Finds a path from start to finish nodes.
        /// </summary>
        /// <param name="start">Node from which we start</param>
        /// <param name="finish">Destination node</param>
        /// <returns>Path representing of nodes array</returns>
        public AStarNode[] FindPath(AStarNode start, AStarNode finish)
        {     
            _destNode = finish;

            _open.Clear();
            _closed.Clear();
            _pathNodesCache.Clear();

            //add start node to open list
            _open.Enqueue(0, start);

            //current processing node
            AStarNode current = null;

            //for preventing infinite loop
            var maxIterCount = 10000;

            //main search loop
            while (_open.Count > 0 && maxIterCount > 0)
            {
                //get best candidate(with minimum F value) for processing from open list
                current = _open.Dequeue();

                if (current == finish)  //check if we reach finish node
                {
                    break;
                }

                ProcessNode(current);
                 
                maxIterCount--;
            }

            //UnityEngine.Debug.Log(string.Format("Iter count: {0}", 10000 - maxIterCount));

            //if a path has not been found
            if (current != finish)
            {
                UnityEngine.Debug.LogError("Cant find path");
                return new []{start};
            }

            
            //build a path from start to finish
            var path = new List<AStarNode>();
            path.AddRange(BuildPath(finish));

            return path.ToArray();
        }

        /// <summary>
        ///     Add node to visited(add to the close list remove from the open list).
        ///     Add node neighbors to the open list.
        /// </summary>
        private void ProcessNode(AStarNode node)
        {
            //node = _open.Dequeue();
            _closed.Add(node);

            //var minFDistance = 99999.0f;
            //AStarNode minFNode = null;

            var pathNode = GetPathNodeInfo(node);

            //add to open list nodes which we has not processed before
            foreach (var aStarEdge in node.Edges)
            {
                var currNeighbor = aStarEdge.Target;

                //if this node was already processed - continue to the next one
                if (_closed.Contains(currNeighbor)/*WasProcessed(currNeighbor)*/)
                {
                    continue;
                }

                //get path node info
                var neighborPathNode = GetPathNodeInfo(currNeighbor);
                
                if (_open.Contains(currNeighbor))
                {
                    //continue;
                    //special check
                    if (neighborPathNode.G < pathNode.G + aStarEdge.MovementCost)
                    {
                        //_closed.Add();
                        neighborPathNode.Parent = pathNode.Parent;
                        //neighborPathNode.G = pathNode.G + aStarEdge.MovementCost;
                    }
                }
                else
                {
                    //set the new parent
                    neighborPathNode.Parent = node;
                    //traversed path
                    neighborPathNode.G = pathNode.G + aStarEdge.MovementCost;
                    //distance to target(heuristic function)
                    neighborPathNode.H =/* Mathf.Abs(currNeighbor.transform.position.x - _destNode.transform.position.x) +
                                         Mathf.Abs(currNeighbor.transform.position.z - _destNode.transform.position.z);*/
                    Vector3.Distance(currNeighbor.transform.position,
                        _destNode.transform.position);  //TODO: make optimization by comparing square distance*/

                    //add to open queue
                    _open.Enqueue(neighborPathNode.F, currNeighbor);
                }
            }
        }

        /// <summary>
        ///     Returns node path info(used for store temporary data needed for calculating path) by given AStarNode
        /// </summary>
        /// <param name="node">Given node</param>
        /// <returns>PathNodeInfo</returns>
        private PathNodeInfo GetPathNodeInfo(AStarNode node)
        {
            if (!_pathNodesCache.ContainsKey(node))
            {
                _pathNodesCache.Add(node, new PathNodeInfo(node));
            }

            return _pathNodesCache[node];
        }

        /// <summary>
        ///     Traverses back from destination node to start node. 
        ///     Returns path from dest to start(start node's parent always equals NULL)
        /// </summary>
        /// <returns>Path from start to destination node</returns>
        private AStarNode[] BuildPath(AStarNode destNode)
        {
            var path = new Stack<AStarNode>();
            var curr = GetPathNodeInfo(destNode);

            while (curr.Parent != null)
            {
                path.Push(curr.Node);
                curr = GetPathNodeInfo(curr.Parent);
            }
            path.Push(curr.Node);

            return path.ToArray();
        }
    }
}

