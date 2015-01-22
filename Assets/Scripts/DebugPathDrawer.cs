//=============================================================================
//  
// module:		AStar for Unity3d
// license:		GNU GPL
// author:		Chernomoretc Igor
// contacts:	oxnardxvi@gmail.com
//
//=============================================================================
 
using UnityEngine;

namespace AStar.Debug
{
    public class DebugPathDrawer : MonoBehaviour
    {
        public AStarNode Start;
        public AStarNode Finish;
        public AStarManager Manager;
        public bool DrawOnSelected = false;
       

        /// <summary>
        ///     Time in ms needed for searching path. 
        ///     Don't edit this manualy.
        /// </summary>
        public float SearchPathTimeMs;
        /// <summary>
        ///     Count of path nodes needed to reach the target.
        /// </summary>
        public int PathNodesCount = 0;

        private void OnDrawGizmos()
        {
            if (!DrawOnSelected)
            {
                DrawPath();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (DrawOnSelected)
            {
                DrawPath();
            }
        }

        private void DrawPath()
        {
            if (Start == null || Finish == null || Manager == null)
            {
                UnityEngine.Debug.LogWarning("Start == null || Finish == null || Manager == null");
                return;
            }

            var startTime = Time.realtimeSinceStartup;
            var path = Manager.FindPath(Start, Finish);
            SearchPathTimeMs = (Time.realtimeSinceStartup - startTime)/* * 1000*/;
            //UnityEngine.Debug.Log("Path nodes count: " + path.Length);
            PathNodesCount = path.Length;

            var pathLen = path.Length;

            //draw start sphere
            Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.7f);
            Gizmos.DrawSphere(path[0].Position, 0.3f);

            //draw finish sphere
            Gizmos.color = new Color(0.8f, 0.2f, 0.2f, 0.7f);
            Gizmos.DrawSphere(path[path.Length - 1].Position, 0.3f);

            //draw path
            Gizmos.color = Color.cyan;
            for (int i = 1; i < pathLen; i++)
            {
                Gizmos.DrawLine(path[i - 1].Position, path[i].Position);
            }
        }

    }

}

