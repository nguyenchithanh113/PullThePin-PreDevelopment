using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAutoPolygonCollider : MonoBehaviour
{
    [SerializeField] PolygonCollider2D polyCollider;
    int numStraightPoint = 6;
    Vector2[] convexPoints;
    void Start()
    {
        convexPoints = new Vector2[numStraightPoint * 2];
        CreateConvexCollider();
    }
    void CreateConvexCollider()
    {
        for(int i =0; i < numStraightPoint; i++)
        {
            Vector2 left = new Vector2(-1, i);
            Vector2 right = new Vector2(1, i);
            convexPoints[i] = left;
            convexPoints[convexPoints.Length - i - 1] = right;
        }
        polyCollider.SetPath(0, convexPoints);
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
