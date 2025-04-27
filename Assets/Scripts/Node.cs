using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int gridX;
    public int gridY;

    public bool isWall;

    public Vector3 worldPosition;

    public Node ParentNode;

    public int moveCost;
    public int heuristicCost;

    public int TotalCost
    {
        get
        {
            return moveCost + heuristicCost;
        }
    }

    public Node(bool IsWallInp, Vector3 worldPositionInp, int gridXInp, int gridYInp)
    {
        isWall = IsWallInp;
        worldPosition = worldPositionInp;
        gridX = gridXInp;
        gridY = gridYInp;
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
