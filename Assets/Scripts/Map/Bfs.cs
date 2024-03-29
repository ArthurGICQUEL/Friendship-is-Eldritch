﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Bfs
{
    static Dictionary<Vector3, Node> nodeDic = new Dictionary<Vector3, Node>();

    public static void InitGraph()
    {
        Door[] doors = GameObject.FindObjectsOfType<Door>();
        if (doors.Length == 0) { return; }
        for (int i = 0; i < doors.Length; i++) //met une node pour chaque porte
        {
            nodeDic[doors[i].floorPos] = new Node(doors[i].floorPos);
            Vector3 roomPos = doors[i].room.GetMiddleFloor();
            //Debug.LogWarning(roomPos);
            if (!nodeDic.ContainsKey(roomPos)) {
                nodeDic[roomPos] = new Node(roomPos);
            }
        }
        Node nodeDoor, nodeRoom, nodeTargetDoor;
        for (int i = 0; i < doors.Length; i++)
        {
            nodeDoor = nodeDic[doors[i].floorPos];
            nodeRoom = nodeDic[doors[i].room.GetMiddleFloor()];
            nodeRoom.children.Add(nodeDoor); //link la room and cette door
            nodeDoor.children.Add(nodeRoom); //link la door avec sa room
            Debug.DrawLine(nodeRoom.position, nodeDoor.position, Color.green, 10f);
            nodeTargetDoor = nodeDic[doors[i].targetDoor.floorPos];
            nodeDoor.children.Add(nodeTargetDoor); //link la porte d'en face qui fait parti de l'autre chambre
            Debug.DrawLine(nodeTargetDoor.position, nodeDoor.position, Color.green, 10f);
            Door[] roomDoors = doors[i].room.doors;
            Node nodeRoomDoor;
            for (int j = 0; j < roomDoors.Length; j++)
            {
                if (doors[i] != roomDoors[j])
                {
                    nodeRoomDoor = nodeDic[roomDoors[j].floorPos];
                    nodeDoor.children.Add(nodeRoomDoor); // link les portes de la chambre à la porte actuelle
                    Debug.DrawLine(nodeDoor.position, nodeRoomDoor.position, Color.green, 10f);
                }
            }
        }
    }

    public static Node GetNextNode(Vector3 from, Vector3 to) => GetNextNode(GetNode(from), GetNode(to));
    public static Node GetNextNode(Node startNode, Node endNode)
    {
        //Node startNode = GetNode(from);
       // Node endNode = GetNode(to);
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
        CleanNodes();
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
    public static Node GetNode(Vector3 nodePos)
    {
        if (!nodeDic.ContainsKey(nodePos))
        {
            return null;
        }
        return nodeDic[nodePos];
    }
}
