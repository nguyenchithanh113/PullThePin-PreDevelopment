using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(CompositeCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class WallCreator : MonoBehaviour
{
    public Wall wall;
    [SerializeField] MeshRenderer render;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] Material mat;
    [SerializeField] PolygonCollider2D polyCollider;
    [SerializeField] CompositeCollider2D compositeCollider;
    [SerializeField] Rigidbody2D rb;
    void Start()
    {
        
    }
    public void InitComponent()
    {
        render = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        polyCollider = GetComponent<PolygonCollider2D>();
        compositeCollider = GetComponent<CompositeCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Static;
        polyCollider.usedByComposite = true;
        compositeCollider.geometryType = CompositeCollider2D.GeometryType.Outlines;
    }
    public void CreateWall()
    {
        wall = new Wall(transform.position, transform);

    }
    public void CreateWallMesh()
    {
        meshFilter.mesh = wall.WallMesh(transform.position,0.05f, 0.5f);
        render.material = mat;
    }
    public void CreateWallCollider()
    {
        polyCollider = GetComponent<PolygonCollider2D>();
        wall.SetCollider(transform.position,polyCollider, 0.3f, 0.5f);
    }
    public void AddSegmentAtIndex(int indexSegment, Vector2 target)
    {
        wall.AddSegmentIndex(indexSegment, target);
        CreateWallMesh();
        CreateWallCollider();
    }
    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnDrawGizmos()
    {
        Vector2[] evenlyPoints = wall.GetEvenlySpacePoints(0.05f);
        for (int i = 0; i < evenlyPoints.Length; i++)
        {
            Gizmos.DrawSphere(evenlyPoints[i], 0.025f);
        }
    }
}
