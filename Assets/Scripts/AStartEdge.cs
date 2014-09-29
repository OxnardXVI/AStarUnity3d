using System;
using UnityEngine;

namespace AStar
{
    /// <summary>
    ///     Representing AStart edge interface. An edge connects 2 different nodes.
    /// </summary>
    public interface IAStarEdge
    {
        /// <summary>
        ///     Origin node
        /// </summary>
        AStarNode Origin { get; }

        /// <summary>
        ///     Node connected to origin
        /// </summary>
        AStarNode Target { get; }

        /// <summary>
        ///     Movement cost from origin to target
        /// </summary>
        float MovementCost { get; }
    }

    /// <summary>
    ///     An edge connects 2 different nodes.
    /// </summary>
    [Serializable]
    public class AStarEdge : IAStarEdge
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
}
