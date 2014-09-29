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

namespace AStar.Debug
{
    /// <summary>
    ///     Represents traveler which moves to the destination point.
    /// </summary>
    public class Traveler : MonoBehaviour
    {
        /// <summary>
        ///     Destination of a path.
        /// </summary>
        public Transform Dest;

        /// <summary>
        ///     AStar manager for path searching
        /// </summary>
        public AStarManager AStarManager;

        /// <summary>
        ///     Our movement speed.
        /// </summary>
        public float MovementSpeed = 2.0f;

        private Vector3 _prevDestPos;
        private Vector3 _prevPos;
        private List<Vector3> _path = new List<Vector3>();
        
        private void Update()
        {
            if (Dest == null)
            {
                UnityEngine.Debug.Log("Dest is not initialized!");
                return;
            }
            if (AStarManager == null)
            {
                UnityEngine.Debug.Log("AStarManager is not initialized!");
                return;
            }

            GeneratePathIfNeeded();
            FollowPath();
            DebugDrawPath();
        }

        private void GeneratePathIfNeeded()
        {
            if (_prevDestPos == Dest.position)
            {
                return;
            }
            _prevDestPos = Dest.position;
            _path.Clear();
            _path.AddRange(AStarManager.FindPath(transform.position, Dest.position));
        }

        private void FollowPath()
        {
            if (_path.Count == 0)
            {
                UnityEngine.Debug.Log("We have reached our destination!");
                return;
            }
         
            var movementDir = (_path[0] - transform.position).normalized;
            transform.position += movementDir * Time.deltaTime * MovementSpeed;

            if (Vector3.Distance(_path[0], _prevPos) <=
                Vector3.Distance(_path[0], transform.position))
            {
                _path.RemoveAt(0);
            }

            _prevPos = transform.position;
        }

        private void DebugDrawPath()
        {
            if (_path == null)
            {
                return;
            }

            for (int i = 1; i < _path.Count; i++)
            {
                UnityEngine.Debug.DrawLine(_path[i-1], _path[i]);
            }
            
        }
    }
}

