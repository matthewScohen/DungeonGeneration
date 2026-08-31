using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DelaunayTriangulation
{
    // Implementation of the Bowyer-Watson algorithm described here https://www.gorillasun.de/blog/bowyer-watson-algorithm-for-delaunay-triangulation/
    public static List<Triangle> Triangulate(List<Vector2> points)
    {
        if (points == null || points.Count < 3) return new List<Triangle>();

        // Construct super triangle who's vertices contain all points in triangulation
        float minX = points.Min(p => p.x);
        float minY = points.Min(p => p.y);
        float maxX = points.Max(p => p.x);
        float maxY = points.Max(p => p.y);

        float boundingBoxWidth = maxX - minX;
        float boundingBoxHeight = maxY - minY;
        float largerBoundingBoxDimension = Math.Max(boundingBoxWidth, boundingBoxHeight);
        float boundingBoxCenterX = (minX + maxX) / 2;
        float boundingBoxCenterY = (minY + maxY) / 2;

        // The scale factor of 20 used here is much larger than needed, but it is easier to create a very large triangle here
        // than to find the smallest triangle that can contain all points
        Vector2 superTriangleP1 = new(boundingBoxCenterX - 20 * largerBoundingBoxDimension, boundingBoxCenterY - largerBoundingBoxDimension);
        Vector2 superTriangleP2 = new(boundingBoxCenterX, boundingBoxCenterY + 20 * largerBoundingBoxDimension);
        Vector2 superTriangleP3 = new(boundingBoxCenterX + 20 * largerBoundingBoxDimension, boundingBoxCenterY - largerBoundingBoxDimension);

        Triangle superTriangle = new(superTriangleP1, superTriangleP2, superTriangleP3);
        List<Triangle> triangulation = new() { superTriangle };

        // Add each point to triangulation
        foreach (Vector2 point in points)
        {
            List<Triangle> badTriangles = new();

            // Find all triangles who's circumcircles contain the new point
            foreach (Triangle triangle in triangulation)
                if (triangle.CircumcircleContains(point))
                    badTriangles.Add(triangle);

            // Find all edges in bad triangles that belong to only 1 bad triangle
            Dictionary<Edge, int> edgeCounts = new();
            foreach (Triangle badTriangel in badTriangles)
            {
                foreach(Edge edge in badTriangel.Edges)
                {
                    if (edgeCounts.ContainsKey(edge)) edgeCounts[edge]++;
                    else edgeCounts[edge] = 1;
                }
            }

            // Remove bad triangles from triangulation
            triangulation.RemoveAll(triangle => badTriangles.Contains(triangle));

            // Add back edges that were only part of 1 bad triangle and connect them to the new point
            foreach (KeyValuePair<Edge, int> edgeCountPair in edgeCounts)
                if (edgeCountPair.Value == 1) 
                    triangulation.Add(new Triangle(edgeCountPair.Key.P1, edgeCountPair.Key.P2, point));
        }

        // Remove triangles that share vertices with the initial super triangle
        triangulation.RemoveAll(t => t.ContainsVertex(superTriangleP1) || t.ContainsVertex(superTriangleP2) || t.ContainsVertex(superTriangleP3));

        return triangulation;
    }

    public static List<Triangle> Triangulate(List<Vector2Int> points)
    {
        return Triangulate(points.Select(point => new Vector2(point.x, point.y)).ToList());
    }
}

public readonly struct Triangle
{
    public Vector2 P1 { get; }
    public Vector2 P2 { get; }
    public Vector2 P3 { get; }
    public Edge[] Edges { get; }

    public Triangle(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        P1 = p1;
        P2 = p2;
        P3 = p3;

        Edges = new Edge[3];
        Edges[0] = new(P1, P2);
        Edges[1] = new(P2, P3);
        Edges[2] = new(P3, P1);
    }

    public bool CircumcircleContains(Vector2 point)
    {
        double circumcenterX = (Math.Pow(P1.magnitude, 2) * (P3.y - P2.y) + Math.Pow(P2.magnitude, 2) * (P1.y - P3.y) + Math.Pow(P3.magnitude, 2) * (P2.y - P1.y)) / 
                               (P1.x * (P3.y - P2.y) + P2.x * (P1.y - P3.y) + P3.x * (P2.y - P1.y)) / 2;

        double circumcenterY = (Math.Pow(P1.magnitude, 2) * (P2.x - P3.x) + Math.Pow(P2.magnitude, 2) * (P3.x - P1.x) + Math.Pow(P3.magnitude, 2) * (P1.x - P2.x)) / 
                               (P1.y * (P2.x - P3.x) + P2.y * (P3.x - P1.x) + P3.y * (P1.x - P2.x)) / 2;

        double circumcircleRadiusSquared = Math.Pow(P1.x - circumcenterX, 2) + Math.Pow(P1.y - circumcenterY, 2);
        double distanceSquared = Math.Pow(point.x - circumcenterX, 2) + Math.Pow(point.y - circumcenterY, 2) ;

        return distanceSquared < circumcircleRadiusSquared;
    }

    public bool ContainsVertex(Vector2 p) => P1.Equals(p) || P2.Equals(p) || P3.Equals(p);
}

public readonly struct Edge
{
    public Vector2 P1 { get; }
    public Vector2 P2 { get; }

    public Edge(Vector2 p1, Vector2 p2)
    {
        // Enforce vertex ordering for equality checks
        if (p1.x < p2.x || (p1.x == p2.x && p1.y < p2.y))
        {
            P1 = p1;
            P2 = p2;
        }
        else
        {
            P1 = p2;
            P2 = p1;
        }
    }

    public override readonly bool Equals(object obj) => obj is Edge e && P1.Equals(e.P1) && P2.Equals(e.P2);

    public override readonly int GetHashCode() => HashCode.Combine(P1, P2);
}
