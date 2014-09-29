//=============================================================================
//  
// module:		AStar for Unity3d
// license:		GNU GPL
// author:		Chernomoretc Igor
// contacts:	oxnardxvi@gmail.com
//
//=============================================================================
 
using System;
using System.Collections.Generic;
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

    public interface IAStarNode
    {
        /// <summary>
        ///     Node edges(connections)
        /// </summary>
        IEnumerable<AStarEdge> Edges { get; }

        /// <summary>
        ///     Adds given node edge to the eadges list.
        /// </summary>
        /// <param name="edge">Eadge to add</param>
        void AddEdge(AStarEdge edge);

        /// <summary>
        ///     Adds given node edges to the eadges list.
        /// </summary>
        /// <param name="edges">Eadges to add</param>
        void AddEdges(AStarEdge[] edges);

        /// <summary>
        ///     Removes given node edge from the eadges list.
        /// </summary>
        /// <param name="edge">Eadge to remove</param>
        void RemoveEdge(AStarEdge edge);

        /// <summary>
        ///     Removes all node edges from the eadges list.
        /// </summary>
        void RemoveAllEdges();
    }

    public class AStarNode : MonoBehaviour, IAStarNode
    {
        /// <summary>
        ///     Node edges(connections)
        /// </summary>
        [SerializeField]
        private List<AStarEdge> _edges = new List<AStarEdge>();

        /// <summary>
        ///     Should draw debug gizmos of this node and it's edges
        /// </summary>
        public bool DrawGizmos = false;

        /// <summary>
        ///     Node edges(connections)
        /// </summary>
        public IEnumerable<AStarEdge> Edges
        {
            get { return _edges; }
        }

        /// <summary>
        ///     Adds given node edge to the eadges list.
        /// </summary>
        /// <param name="edge">Eadge to add</param>
        public void AddEdge(AStarEdge edge)
        {
            _edges.Add(edge);
        }

        /// <summary>
        ///     Adds given node edges to the eadges list.
        /// </summary>
        /// <param name="edges">Eadges to add</param>
        public void AddEdges(AStarEdge[] edges)
        {
            _edges.AddRange(edges);
        }


        /// <summary>
        ///     Removes given node edge from the eadges list.
        /// </summary>
        /// <param name="edge">Eadge to remove</param>
        public void RemoveEdge(AStarEdge edge)
        {
            _edges.Remove(edge);
        }

        /// <summary>
        ///     Removes all node edges from the eadges list.
        /// </summary>
        public void RemoveAllEdges()
        {
            _edges.Clear();
        }

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
