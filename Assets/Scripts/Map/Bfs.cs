using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Bfs
{
    static Dictionary<Vector3, Node> nodeDic = new Dictionary<Vector3, Node>();
    static Queue<Vector3> nodeQueue = null;


    public static void InitGraph()
    {
        Door[] doors = GameObject.FindObjectsOfType<Door>();
        for (int i = 0; i < doors.Length; i++) //met une node pour chaque porte
        {
            nodeDic[doors[i].transform.position] = new Node(doors[i].transform.position);
        }
        for (int i = 0; i < doors.Length; i++)
        {
            nodeDic[doors[i].transform.position].children.Add(nodeDic[doors[i].targetDoor.transform.position]); //link la porte d'en face qui fait parti de l'autre chambre
            Door[] roomDoors = doors[i].room.doors;
            for (int j = 0; j < roomDoors.Length; j++)
            {
                if (doors[i] != roomDoors[j])
                {
                    nodeDic[doors[i].transform.position].children.Add(nodeDic[roomDoors[j].transform.position]); // link les portes de la chambre à la porte actuelle
                }
            }
        }
    }
    public static Node GetNextNode(Vector3 from, Vector3 to)
    {
        Node startNode = GetNode(from);
        Node endNode = GetNode(to);
        if (startNode == null || endNode == null)
        {
            return null;
        }
        List<Node> path;
        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(startNode);
        startNode.Parent = null;
        startNode.Depth = 0;

        // iterate over every node once by propagating to neighboring nodes
        while (queue.Count > 0)
        {
            Node node = queue.Dequeue();
            foreach (Node n in node.children)
            {
                if (n.Depth < 0)
                {
                    queue.Enqueue(n);
                    n.Depth = node.Depth + 1;
                    n.Parent = node;
                    if (n == endNode)
                    {
                        path = GetParentPath(n);
                        path.RemoveAt(path.Count - 1);
                        path.Reverse();
                        CleanNodes();
                        return path[0];
                    }
                }
            }
        }
        return null;
    }
    static List<Node> GetParentPath(Node node, List<Node> path = null)
    {
        if (path == null) { path = new List<Node>(); }
        if (node == null)
        {
            return path;
        }
        path.Add(node);
        return GetParentPath(node.Parent, path);
    }
    static void CleanNodes()
    {
        // Clean the nodes before next time
        List<Node> nodes = new List<Node>(nodeDic.Values);
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Clean();
        }
    }
    static Node GetNode(Vector3 nodePos)
    {
        if (!nodeDic.ContainsKey(nodePos))
        {
            return null;
        }
        return nodeDic[nodePos];
    }
}
