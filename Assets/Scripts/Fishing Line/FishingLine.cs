using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLine : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private EdgeCollider2D edgeCollider;

    private Transform[] points;

    private void Update()
    {
        UpdateLine();
        UpdateCollider();
    }

    //initialize the two points the line with render between
    public void InitializeLine(Transform[] points)
    {
        line.positionCount = points.Length;
        this.points = points;
    }

    //updates the line renderer based on the target points
    private void UpdateLine()
    {
        for (int i = 0; i < points.Length; i++)
            line.SetPosition(i, points[i].position);
    }

    //update points of edge collider to match line 
    private void UpdateCollider()
    {
        List<Vector2> edges = new();

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 linePoint = line.GetPosition(i);
            edges.Add(linePoint);
        }

        edgeCollider.SetPoints(edges);
    }
}
