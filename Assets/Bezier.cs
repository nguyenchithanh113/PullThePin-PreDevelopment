using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier
{
    public static Vector2 QuadraticEvaluate(Vector2 p1, Vector2 p2, Vector2 p3, float t) 
    {
        Vector2 v1 = Vector2.Lerp(p1, p2, t);
        Vector2 v2 = Vector2.Lerp(p2, p3, t);
        return Vector2.Lerp(v1, v2, t);
    }
    public static Vector2 CubicEvaluate(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, float t)
    {
        Vector2 v1 = QuadraticEvaluate(p1, p2, p3, t);
        Vector2 v2 = QuadraticEvaluate(p2, p3, p4, t);
        return Vector2.Lerp(v1, v2, t);
    }
    public static float EstimateCubicCurveLength(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        float straight = Vector2.Distance(p1, p4);
        float bound = Vector2.Distance(p1, p2) + Vector2.Distance(p2, p3) + Vector2.Distance(p3, p4);
        float estimateLength = straight + bound / 2;
        return estimateLength;
    }
    public static Vector2[] GetEvenlyPoints(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,float spacing)
    {
        List<Vector2> evenlyPoints = new List<Vector2>();
        float lastDistance = 0;
        Vector2 currentPoint = p1;
        evenlyPoints.Add(currentPoint);
        float segmentLength = Bezier.EstimateCubicCurveLength(p1, p2, p3, p4);
        int division = Mathf.CeilToInt(segmentLength / spacing)  * 10;

        float t = 0;
        while (t < 1)
        {
            t += 1f / (float)division;
            //t += 0.1f;
            Vector2 pointOnCurve = Bezier.CubicEvaluate(p1, p2, p3, p4, t);
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
        evenlyPoints.Add(p4);

        return evenlyPoints.ToArray();
    }
}
