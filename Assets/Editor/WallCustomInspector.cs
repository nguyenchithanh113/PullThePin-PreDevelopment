using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(WallCreator))]
public class WallCustomInspector : Editor
{
    WallCreator wallCreator;
    Wall wall;
    Vector3 origin;
    bool isHoldingSpace;
    private void OnEnable()
    {
        wallCreator = (WallCreator)target;
        if (wallCreator.wall == null)
        {
            wallCreator.CreateWall();
            
        }
        wallCreator.InitComponent();
        wall = wallCreator.wall;
        origin = wallCreator.transform.position;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Create Wall"))
        {
            Undo.RecordObject(wallCreator, "Create new wall");
            wallCreator.CreateWall();
            wall = wallCreator.wall;
            SceneView.RepaintAll();
        }
        if (GUILayout.Button("Create Wall Mesh"))
        {
            wallCreator.CreateWallMesh();
            SceneView.RepaintAll();
        }
        if (GUILayout.Button("Create Wall Collider"))
        {
            wallCreator.CreateWallCollider();
            SceneView.RepaintAll();
        }
    }
    private void OnSceneGUI()
    {
        DrawBezier();
        FollowCenter();
        Input();
        WallPointMoveHandler();
    }
    void FollowCenter()
    {
        Vector3 pos = wallCreator.transform.position;
        pos = SnapV2(pos);
        if(wallCreator.transform.position != pos) wallCreator.transform.position = pos;
        if (origin != pos)
        {
            Vector3 vec = pos - origin;
            for (int i = 0; i < wall.points.Count; i++)
            {
                wall.points[i] = wall.points[i] + (Vector2)vec;
            }
            origin = wallCreator.transform.position;
        }
        
    }
    void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        if (guiEvent.shift && guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
        {
            Undo.RecordObject(wallCreator, "Add Segment");
            mousePos = SnapV2(mousePos);
            
            wall.AddSegment(mousePos);
        }
        if(guiEvent.type == EventType.KeyDown && guiEvent.keyCode == KeyCode.Space && !isHoldingSpace)
        {
            isHoldingSpace = true;
            wall.SetRegion();
        }
        if (guiEvent.type == EventType.KeyUp && guiEvent.keyCode == KeyCode.Space)
        {
            isHoldingSpace = false;
            wall.ClearRegion();
        }
        if (isHoldingSpace)
        {
            RaycastHit2D hit = Physics2D.CircleCast(mousePos, 0.15f, Vector2.up, 0, LayerMask.GetMask("WallRegion"));
            if (hit)
            {
                int index = hit.collider.transform.GetSiblingIndex();
                Vector2[] points = wall.GetSegmentPoints(index);
                Vector2[] evenlyPoints = Bezier.GetEvenlyPoints(points[0], points[1], points[2], points[3], 0.05f);
                float min = (evenlyPoints[0] - mousePos).sqrMagnitude;
                int minIndex = 0;
                for(int i = 1; i < evenlyPoints.Length; i++)
                {
                    float distance = (evenlyPoints[i] - mousePos).sqrMagnitude;
                    if (distance < min)
                    {
                        min = distance;
                        minIndex = i;
                    }
                }
                Vector2 targetPoint = evenlyPoints[minIndex];
                Handles.DrawSolidDisc(targetPoint, Vector3.forward, 0.1f);

                if(guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
                {
                    Undo.RecordObject(wallCreator, "Add segment at index");
                    wallCreator.AddSegmentAtIndex(index, targetPoint);
                }

                SceneView.RepaintAll();
            }
        }
    }

    void DrawBezier()
    {
        for (int i = 0; i < wall.segmentLength; i++)
        {
            Vector2[] _points = wall.GetSegmentPoints(i);
            Handles.DrawLine(_points[0], _points[1]);
            Handles.DrawLine(_points[2], _points[3]);
            Handles.DrawBezier(_points[0], _points[3], _points[1], _points[2], Color.white, null, 1f);
        }
    }
    void WallPointMoveHandler()
    {
        for (int i = 0; i < wall.points.Count; i++)
        {
            float size = 0.15f;
            Color color = Color.white;
            if (i % 3 == 0)
            {
                size = 0.25f;
                color = Color.yellow;
            }
            Handles.color = color;
            Vector2 target = Handles.FreeMoveHandle(wall.points[i], Quaternion.identity, size, Vector2.zero, Handles.CylinderHandleCap);
            if (wall.points[i] != target)
            {
                Undo.RecordObject(wallCreator, "Move Point");
                target = SnapV2(target, 0.25f);
                wall.MovePoint(i, target, !Event.current.control);
                wallCreator.CreateWallMesh();
                wallCreator.CreateWallCollider();
            }
            Handles.color = Color.white;
        }
    }
    Vector2 SnapV2(Vector2 vector, float snap = 0.25f)
    {
        float xPoint = vector.x - (int)vector.x;
        float yPoint = vector.y - (int)vector.y;
        float x = (int)vector.x + Mathf.FloorToInt(xPoint / snap) * snap;
        float y = (int)vector.y + Mathf.FloorToInt(yPoint / snap) * snap;
        return new Vector2(x, y);
    }

}
