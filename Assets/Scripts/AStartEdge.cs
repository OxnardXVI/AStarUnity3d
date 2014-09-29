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



}
