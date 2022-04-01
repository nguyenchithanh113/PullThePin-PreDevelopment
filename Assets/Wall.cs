using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wall
{
    public List<Vector2> points;
    public List<PolygonCollider2D> regionTrigger = new List<PolygonCollider2D>();
    public List<GameObject> regionObj = new List<GameObject>();

    public int segmentLength => SegmentLength();
    public Transform trMain;

    public Wall(Vector2 center, Transform _trMain)
    {
        points = new List<Vector2>()
        {
            center + new Vector2(-2,0),
            center + new Vector2(-1, 1),
            center + new Vector2(1, -1),
            center + new Vector2(2, 0),
        };
        trMain = _trMain;

    }
    public void AddNewRegion()
    {
        
    }
    public void ClearRegion()
    {
        for (int i = 0; i < regionObj.Count; i++)
        {
            Object.DestroyImmediate(regionObj[i]);
        }
        regionTrigger.Clear();
        regionObj.Clear();
    }
    public void InitRegion()
    {
        for(int i = 0; i < segmentLength; i++)
        {
            GameObject _region = new GameObject("Region");
            _region.layer = LayerMask.NameToLayer("WallRegion");
            _region.transform.SetParent(trMain);
            Rigidbody2D rb = _region.AddComponent<Rigidbody2D>();
            PolygonCollider2D col = _region.AddComponent<PolygonCollider2D>();
            CompositeCollider2D compositeCol = _region.AddComponent<CompositeCollider2D>();
            rb.bodyType = RigidbodyType2D.Static;
            col.usedByComposite = true;
            compositeCol.geometryType = CompositeCollider2D.GeometryType.Outlines;
            col.isTrigger = true;
            compositeCol.isTrigger = true;
            regionTrigger.Add(col);
            regionObj.Add(_region);
        }
    }
    int SegmentLength()
    {
        int length = (points.Count - 1) / 3;
        return length;
    }
    public Vector2[] GetSegmentPoints(int index)
    {
        int startIndex = index * 3;
        Vector2[] _points = new Vector2[]
        {
            points[startIndex],
            points[startIndex+1],
            points[startIndex+2],
            points[startIndex+3],
        };
        return _points;
    }
    public void AddSegment(Vector2 _target)
    {
        Vector2 firstTangent = points[points.Count - 1] + (points[points.Count - 1] - points[points.Count - 2]);
        Vector2 secondTangent = _target + (points[points.Count - 2] - points[points.Count - 1]);
        Vector2 secondAnchor = _target;
        Vector2[] _seg = new Vector2[]
        {
            firstTangent,
            secondTangent,
            secondAnchor,
        };
        points.AddRange(_seg);
    }
    public void AddSegmentIndex(int indexSegment, Vector2 target)
    {
        int insertIndex = (indexSegment * 3 + 3)-1;
        Vector2 firstTangent = target + Vector2.up;
        Vector2 secondTangent  = target + Vector2.down;
        points.Insert(insertIndex, firstTangent);
        points.Insert(insertIndex + 1, target);
        points.Insert(insertIndex + 2, secondTangent);
 
    }
    public void MovePoint(int index, Vector2 _target, bool constraint = true)
    {
        if (index % 3 == 0)
        {
            Vector2 dir = (_target - points[index]);
            points[index] = _target;
            if (index != points.Count - 1)
            {
                points[index + 1] += dir;
            }
            if (index > 0)
            {
                points[index - 1] += dir;
            }
        }
        else
        {
            int indexAnchor = (index + 1) % 3 == 0 ? (index + 1) : (index - 1);
            points[index] = _target;
            if (index > 1 && constraint)
            {
                if (indexAnchor == index + 1 && indexAnchor != points.Count - 1)
                {
                    points[indexAnchor + 1] = points[indexAnchor] + (points[indexAnchor] - points[index]);
                }
                else if (indexAnchor == index - 1)
                {
                    points[indexAnchor - 1] = points[indexAnchor] + (points[indexAnchor] - points[index]);
                }
            }
        }
    }
    public Vector2[] GetEvenlySpacePoints(float spacing, int resolution = 1)
    {
        List<Vector2> evenlyPoints = new List<Vector2>();
        for (int i = 0; i < segmentLength; i++)
        {
            Vector2[] _points = GetSegmentPoints(i);
            float lastDistance = 0;
            Vector2 currentPoint = _points[0];
            evenlyPoints.Add(currentPoint);
            float segmentLength = Bezier.EstimateCubicCurveLength(_points[0], _points[1], _points[2], _points[3]);
            int division = Mathf.CeilToInt(segmentLength / spacing) * resolution * 10;

            float t = 0;
            while (t < 1)
            {
                t += 1f / (float)division;
                //t += 0.1f;
                Vector2 pointOnCurve = Bezier.CubicEvaluate(_points[0], _points[1], _points[2], _points[3], t);
                lastDistance += (pointOnCurve - currentPoint).magnitude;
                while (lastDistance > spacing)
                {
                    float overShoot = lastDistance - spacing;
                    Vector2 calculatePoint = (currentPoint - pointOnCurve).normalized * overShoot + pointOnCurve;
                    evenlyPoints.Add(calculatePoint);
                    lastDistance = overShoot;
                }
                currentPoint = pointOnCurve;
            }
        }
        evenlyPoints.Add(points[points.Count - 1]);

        return evenlyPoints.ToArray();
    }
    public Mesh WallMesh(Vector2 center, float spacing = 0.05f, float width = 0.5f, int resolution = 1)
    {
        Vector2[] evenlyPoints = GetEvenlySpacePoints(spacing, resolution);
        Vector3[] verts = new Vector3[evenlyPoints.Length * 2];
        Vector2[] uvs = new Vector2[verts.Length];
        int[] tris = new int[(verts.Length - 2) * 3];

        int vertIndex = 0;
        int trisIndex = 0;

        for (int i = 0; i < evenlyPoints.Length; i++)
        {
            Vector2 forward = Vector2.zero;
            if (i < evenlyPoints.Length - 1)
            {
                forward += (evenlyPoints[i + 1] - evenlyPoints[i]);
            }
            if (i > 0)
            {
                forward += (evenlyPoints[i] - evenlyPoints[i - 1]);
            }
            forward.Normalize();
            Vector2 left = evenlyPoints[i] + Vector2.Perpendicular(forward) * (width * 0.5f);
            Vector2 right = evenlyPoints[i] - Vector2.Perpendicular(forward) * (width * 0.5f);
            verts[vertIndex] = left - center;
            verts[vertIndex + 1] = right - center;
            Debug.DrawLine(evenlyPoints[i], left);
            Debug.DrawLine(evenlyPoints[i], right);

            float completePercent = (float)i / ((float)points.Count);

            uvs[vertIndex] = new Vector2(0, 1 - completePercent);
            uvs[vertIndex + 1] = new Vector2(1, 1 - completePercent);
            if (i < evenlyPoints.Length - 1)
            {
                //First Triangle
                tris[trisIndex] = vertIndex;
                tris[trisIndex + 1] = vertIndex + 2;
                tris[trisIndex + 2] = vertIndex + 1;
                //Seccond Triangle
                tris[trisIndex + 3] = vertIndex + 1;
                tris[trisIndex + 4] = vertIndex + 2;
                tris[trisIndex + 5] = vertIndex + 3;
            }
            vertIndex += 2;
            trisIndex += 6;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        return mesh;

    }
    public void SetCollider(Vector2 center, PolygonCollider2D collider2D, float spacing = 0.25f, float width = 0.5f)
    {
        Vector2[] evenlyPoints = GetEvenlySpacePoints(spacing);
        Vector2[] convexPoints = new Vector2[evenlyPoints.Length * 2];
        int stack = Mathf.CeilToInt(evenlyPoints.Length / segmentLength) * 2;


        for (int i = 0; i < evenlyPoints.Length; i++)
        {

            Vector2 forward = Vector2.zero;
            if (i < evenlyPoints.Length - 1)
            {
                forward += (evenlyPoints[i + 1] - evenlyPoints[i]);
            }
            if (i > 0)
            {
                forward += (evenlyPoints[i] - evenlyPoints[i - 1]);
            }
            forward.Normalize();
            Vector2 left = evenlyPoints[i] + Vector2.Perpendicular(forward) * (width * 0.5f);
            Vector2 right = evenlyPoints[i] - Vector2.Perpendicular(forward) * (width * 0.5f);
            convexPoints[i] = left - center;
            convexPoints[convexPoints.Length - i - 1] = right - center;

        }

        collider2D.SetPath(0, convexPoints);
    }
    public void SetRegion()
    {
        ClearRegion();
        InitRegion();
        for (int j = 0; j < segmentLength; j++)
        {
            float spacing = 0.3f;
            Vector2[] _points = GetSegmentPoints(j);
            float lastDistance = 0;
            Vector2 currentPoint = _points[0];
            List<Vector2> evenlyPoints = new List<Vector2>();
            evenlyPoints.Add(currentPoint);
            float segmentLength = Bezier.EstimateCubicCurveLength(_points[0], _points[1], _points[2], _points[3]);
            int division = Mathf.CeilToInt(segmentLength / spacing)  * 10;

            float t = 0;
            while (t < 1)
            {
                t += 1f / (float)division;
                //t += 0.1f;
                Vector2 pointOnCurve = Bezier.CubicEvaluate(_points[0], _points[1], _points[2], _points[3], t);
                lastDistance += (pointOnCurve - currentPoint).magnitude;
                while (lastDistance > spacing)
                {
                    float overShoot = lastDistance - spacing;
                    Vector2 calculatePoint = (currentPoint - pointOnCurve).normalized * overShoot + pointOnCurve;
                    evenlyPoints.Add(calculatePoint);
                    lastDistance = overShoot;
                }
                currentPoint = pointOnCurve;
            }

            Vector2[] convexPoints = new Vector2[evenlyPoints.Count * 2];
            for (int i = 0; i < evenlyPoints.Count; i++)
            {

                Vector2 forward = Vector2.zero;
                if (i < evenlyPoints.Count - 1)
                {
                    forward += (evenlyPoints[i + 1] - evenlyPoints[i]);
                }
                if (i > 0)
                {
                    forward += (evenlyPoints[i] - evenlyPoints[i - 1]);
                }
                forward.Normalize();
                Vector2 left = evenlyPoints[i] + Vector2.Perpendicular(forward) * (0.3f * 0.5f);
                Vector2 right = evenlyPoints[i] - Vector2.Perpendicular(forward) * (0.3f * 0.5f);
                convexPoints[i] = left - (Vector2)trMain.position; ;
                convexPoints[convexPoints.Length - i - 1] = right - (Vector2)trMain.position;
            }
            regionTrigger[j].SetPath(0, convexPoints);
        }
    }


}
