using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace DiceGirl.MainGame
{
    public class PointManager : MonoBehaviour
    {
        public Point[] points; //{ get { return _points != null ? _points : GetComponentsInChildren<Point>(); } }
                               //Point[] _points;

        private void Awake()
        {
            points = GetComponentsInChildren<Point>();
        }


#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Point[] points = GetComponentsInChildren<Point>();

            for (int i = 0; i < points.Length; i++)
            {
                var point = points[i];
                DrawString(i + "", point.transform.position, Color.red);
                //改名，避免和f2改名冲突
                if (point.name != "Point " + i)
                    point.name = "Point " + i;

                if (i > 0)
                    Gizmos.DrawLine(points[i - 1].transform.position, point.transform.position);
            }

            //尾部连起来
            if (points.Length >= 2)
                Gizmos.DrawLine(points[0].transform.position, points[points.Length - 1].transform.position);
        }

        //绘制文本
        void DrawString(string text, Vector3 worldPos, Color? colour = null)
        {
            UnityEditor.Handles.BeginGUI();
            if (colour.HasValue) GUI.color = colour.Value;
            var view = UnityEditor.SceneView.currentDrawingSceneView;
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
            GUI.Label(new Rect(screenPos.x - (size.x / 2) + 2, -screenPos.y + view.position.height - 10, size.x, size.y), text);
            UnityEditor.Handles.EndGUI();
        }
#endif

        public Point GetNextPoint(Point point)
        {
            int nextIndex = System.Array.IndexOf(points, point) + 1;
            if (nextIndex < points.Length)
                return points[nextIndex];
            return null;
        }

    }

}