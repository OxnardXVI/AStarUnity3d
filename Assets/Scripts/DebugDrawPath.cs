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
    public class DebugDrawPath : MonoBehaviour
    {
        public AStarNode Start;
        public AStarNode Finish;
        public AStarManager Manager;

        /// <summary>
        ///     Time in ms needed for searching path. 
        ///     Don't edit this manualy.
        /// </summary>
        public float SearchPathTimeMs;

        private void OnDrawGizmosSelected()
        {
            if (Start == null || Finish == null || Manager == null)
            {
                UnityEngine.Debug.LogWarning("Start == null || Finish == null || Manager == null");
                return;
            }

            var currTime = Time.realtimeSinceStartup;
            var path = Manager.FindPath(Start, Finish);
            UnityEngine.Debug.Log("Path nodes count: " + path.Length);
            SearchPathTimeMs = (Time.realtimeSinceStartup - currTime) * 1000;


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

