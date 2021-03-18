using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool visited;
    public List<Node> children;
    public Vector3 position;
    public Node Parent;
    public int Depth;

    public Node(Vector3 pos)
    {
        children = new List<Node>();
        visited = false;
        position = pos;
        Clean();
    }
    public void Clean()
    {
        Parent = null;
        Depth = -1;
    }

    public override string ToString() {
        return $"Node({position})";
    }
}
