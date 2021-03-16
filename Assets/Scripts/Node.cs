using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int nom;
    public bool visited;
    public List<Node> children;
    public int depth = -1;

    public Node(int name, bool visited, int depth)
    {
        children = new List<Node>();
        nom = name;
        this.visited = visited;
        this.depth = depth;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
