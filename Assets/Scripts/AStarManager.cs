//=============================================================================
//  
// module:		AStar for Unity3d
// license:		GNU GPL
// author:		Chernomoretc Igor
// contacts:	oxnardxvi@gmail.com
//
//=============================================================================
 
using System.Linq;
using UnityEngine;

namespace AStar
{
    /// <summary>
    ///     Main class of the AStar module.
    ///     Use it for auto generation waypoints in editor mode and path searching in play mode.
    /// </summary>
    [ExecuteInEditMode]
    public class AStarManager : MonoBehaviour
    {
        /// <summary>
        ///     Nodes count in x dimension.
        /// </summary>
        public int NodesXCount = 3;

        /// <summary>
        ///     Nodes count in z dimension.
        /// </summary>
        public int NodesZCount = 3;

        /// <summary>
        ///     X size of the node field.
        /// </summary>
        public float SizeX = 2.0f;

        /// <summary>
        ///     Z size of the node field
        /// </summary>
        public float SizeZ = 2.0f;

        /// <summary>
        ///     Should draw node gizmos.
        /// </summary>
        public bool DrawGizmos;

        /// <summary>
        ///     Click for auto generate nodes in editor mode.
        /// </summary>
        public bool Build;

        private IAStarNode[,] _nodesField;

        [SerializeField]
        [HideInInspector]
        private AStarNode[] _nodes;

        private readonly AStarNodeGenerator _nodeGenerator = new AStarNodeGenerator();
        private readonly AStarPathFinder _pathFinder = new AStarPathFinder();
        private bool _drawGizmos;

        public Rect Bounds
        {
            get
            {
                var xSize = SizeX;
                var zSize = SizeZ;

                return new Rect(transform.position.x - xSize*0.5f,
                    transform.position.z - zSize * 0.5f,
                    xSize,
                    zSize);
            }
        }

        /// <summary>
        ///     Generates a node field with SizeX x SizeZ sizes and NodesXCount x NodesZCount nodes
        /// </summary>
        public void GenerateNodes()
        {
            CleanUpNodes();

            _nodesField = _nodeGenerator.Build(NodesXCount, 
                NodesZCount,
                 SizeX / (NodesXCount-1),
                 SizeZ / (NodesZCount-1),
                transform.position, 
                transform);

            _nodes = _nodesField.Cast<AStarNode>().ToArray();
        }

        /// <summary>
        ///     Finds a path from start to end node.
        /// </summary>
        /// <param name="start">Start node</param>
        /// <param name="finish">End node</param>
        /// <returns>Array of path nodes</returns>
        public IAStarNode[] FindPath(IAStarNode start, IAStarNode finish)
        {
            return _pathFinder.FindPath(start, finish);
        }

        /// <summary>
        ///     Finds a path from start to end point.
        /// </summary>
        /// <param name="start">Start pont</param>
        /// <param name="finish">End point</param>
        /// <returns>Array of path positions</returns>
        public Vector3[] FindPath(Vector3 start, Vector3 finish)
        {
            return FindPath(FindClosestNode(start), FindClosestNode(finish)).
                Select( t => t.Position).
                ToArray();
        }

        /// <summary>
        ///     eturn closest visible node from given position. 
        /// </summary>
        /// <param name="point">Given position</param>
        /// <returns>Closest node if find otherwise - null</returns>
        public IAStarNode FindClosestNode(Vector3 point)
        {
            return FindClosestNode(point, CheckObstacles);
        }

        /// <summary>
        ///     Return closest visible node from given position.
        /// </summary>
        /// <param name="point">Given position</param>
        /// <param name="checkVisFunc">Function that checks visibility of a node from given position</param>
        /// <returns>Closest node if find otherwise - null</returns>
        public IAStarNode FindClosestNode(Vector3 point, CheckObstaclesFuncDel checkVisFunc)
        {
            IAStarNode closestNode = null;
            var closestNodeDist = float.MaxValue;

            //todo: optimize this
            for (int i = 0; i < NodesXCount * NodesZCount; i++)
            {
                var dist = Vector3.Distance(point, _nodes[i].Position);
                if (dist < closestNodeDist &&
                    !checkVisFunc(_nodes[i].Position, point))
                {
                    closestNodeDist = dist;
                    closestNode = _nodes[i];
                }
            }

            return closestNode;
        }

        /// <summary>
        ///     Is there any obstacles between two given points.
        /// </summary>
        /// <returns>False if there no obstacles between points, otherwise - true</returns>
        private bool CheckObstacles(Vector3 point1, Vector3 point2)
        {
            var disnace = Vector3.Distance(point1, point2);

            return Physics.Raycast(point1, (point2 - point1).normalized, disnace) ||
                   Physics.Raycast(point2, (point1 - point2).normalized, disnace);
        }

        private void CleanUpNodes()
        {
            if (_nodesField == null)
            {
                return;
            }
            foreach (var node in GetComponentsInChildren<AStarNode>())
            {
                DestroyImmediate(node.gameObject);
            }

            _nodesField = null;
        }

        private void Update()
        {
            CheckInputData();

            //should draw gizmos
            if (_drawGizmos != DrawGizmos)
            {
                _drawGizmos = DrawGizmos;
                foreach (var node in GetComponentsInChildren<AStarNode>())
                {
                    node.DrawGizmos = DrawGizmos;
                }
            }

            if (Build)
            {
                Build = false;
                GenerateNodes();

                //enable draw gizmos
                _drawGizmos = false;
                DrawGizmos = false;
            }
        }

        /// <summary>
        ///     Checks and clamps input data from unity editor
        /// </summary>
        private void CheckInputData()
        {
            NodesXCount = Mathf.Clamp(NodesXCount, 3, 10000);
            NodesZCount = Mathf.Clamp(NodesZCount, 3, 10000);
            SizeX = Mathf.Clamp(SizeX, 1.0f, 1000.0f);
            SizeZ = Mathf.Clamp(SizeZ, 1.0f, 1000.0f);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            var bounds = Bounds;

            //draw nodes bounds
            //  1-------2
            //  |       |
            //  |       |
            //  0-------3
            var point0 = new Vector3(bounds.xMin, transform.position.y, bounds.yMin);
            var point1 = new Vector3(bounds.xMin, transform.position.y, bounds.yMax);
            var point2 = new Vector3(bounds.xMax, transform.position.y, bounds.yMax);
            var point3 = new Vector3(bounds.xMax, transform.position.y, bounds.yMin);

            Gizmos.DrawLine(point0, point1);
            Gizmos.DrawLine(point1, point2);
            Gizmos.DrawLine(point2, point3);
            Gizmos.DrawLine(point3, point0);
        }
    }
}