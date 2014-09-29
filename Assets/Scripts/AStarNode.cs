//=============================================================================
//  
// module:		AStar for Unity3d
// license:		GNU GPL
// author:		Chernomoretc Igor
// contacts:	oxnardxvi@gmail.com
//
//=============================================================================
 
using System;
using UnityEngine;

namespace AStar
{
    /// <summary>
    ///     Edge than connects 2 different nodes.
    /// </summary>
    [Serializable]
    public class AStarEdge
    {
        /// <summary>
        ///     Origin node
        /// </summary>
        [SerializeField]
        private AStarNode _origin;
        /// <summary>
        ///     Node connected to origin
        /// </summary>
        [SerializeField]
        private AStarNode _target;
        /// <summary>
        ///     Movement cost from origin to target
        /// </summary>
        [SerializeField]
        private float _movementCost;

        /// <summary>
        ///     Origin node which is linked to target by this edge.
        /// </summary>
        public AStarNode Origin 
        {
            get
            {
                return _origin;
            }
            private set
            {
                _origin = value;
            } 
        }

        /// <summary>
        ///     Target node which is linked to origin by this edge.
        /// </summary>
        public AStarNode Target
        {
            get
            {
                return _target;
            }
            private set
            {
                _target = value;
            } 
        }

        /// <summary>
        ///     Movement cost from origin to target node.
        /// </summary>
        public float MovementCost
        {
            get
            {
                return _movementCost;
            }
            private set
            {
                _movementCost = value;
            } 
        }

        public AStarEdge(AStarNode origin, AStarNode target, float movementCost)
        {
            Origin = origin;
            Target = target;
            MovementCost = movementCost;
        }
    }

    public class AStarNode : MonoBehaviour
    {
        /// <summary>
        ///     Node edges(connections)
        /// </summary>
        public AStarEdge[] Edges = new AStarEdge[0];

        /// <summary>
        ///     Should draw debug gizmos of this node and it's edges
        /// </summary>
        public bool DrawGizmos = false;

        private void OnDrawGizmos()
        {
            if (!DrawGizmos)
            {
                return;
            }

            Gizmos.color = new Color(0.2f, 0.2f, 0.8f, 0.7f);
            Gizmos.DrawSphere(transform.position, 0.3f);

            //draw linked nodes
            Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 1.0f);
            foreach (var edge in Edges)
            {
                Gizmos.DrawLine(transform.position, edge.Target.transform.position);
            }
        }
    }
}
