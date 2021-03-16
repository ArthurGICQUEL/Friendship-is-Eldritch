using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool visited;
    public List<Node> children;
    Vector3 nodePos;
    public Node Parent;
    public int Depth;

    public Node(Vector3 pos)
    {
        children = new List<Node>();
        visited = false;
        nodePos = pos;
        Clean();
    }
    public void Clean()
    {
        Parent = null;
        Depth = -1;
    }
}
