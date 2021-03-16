using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dfs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    static void DepthFirstSearch(Node CurrentNode)
    {
        CurrentNode.visited = true;
        Debug.Log(CurrentNode.nom);
        for (int i = 0; i < CurrentNode.children.Count; i++)
        {
            if (!CurrentNode.children[i].visited)
            {
                DepthFirstSearch(CurrentNode.children[i]);
            }
        }
    }
}
