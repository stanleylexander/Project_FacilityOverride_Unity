using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBlock : MonoBehaviour
{
    public Transform startPos;
    public Transform targetPos;

    public LayerMask wallMask;

    public Vector2 gridWorldSize;

    public float nodeRadius;
    float nodeDiameter;

    public float distanceBetweenNodes;

    Node[,] nodeGrid;
    public List<Node> finalPath;
    int gridSizeX, gridSizeY;

    // Start is called before the first frame update
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateGrid()
    {
        nodeGrid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position -
            Vector3.right * gridWorldSize.x / 2 -
            Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft +
                    Vector3.right * (x * nodeDiameter + nodeRadius) +
                    Vector3.forward * (y * nodeDiameter + nodeRadius);

                bool isWall = Physics.CheckSphere(worldPoint, nodeRadius, wallMask);

                nodeGrid[x, y] = new Node(isWall, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighboringNodes(Node neighbourNodeInp)
    {
        List<Node> neighborList = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = neighbourNodeInp.gridX + x;
                int checkY = neighbourNodeInp.gridY + y;

                if (checkX >= 0 && checkX < gridWorldSize.x && checkY >= 0 && checkY < gridWorldSize.y)
                {
                    neighborList.Add(nodeGrid[checkX, checkY]);
                }
            }
        }

        return neighborList;
    }

    public Node NodeFromWorldPoint(Vector3 vectorWorldPos)
    {
        float xPos = ((vectorWorldPos.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float yPos = ((vectorWorldPos.z + gridWorldSize.y / 2) / gridWorldSize.y);

        xPos = Mathf.Clamp01(xPos);
        yPos = Mathf.Clamp01(yPos);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPos);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPos);

        return nodeGrid[x, y];
    }

    private void OnDrawGizmos()
    {
        if (nodeGrid != null)
        {
            Node playerNode = NodeFromWorldPoint(startPos.position);
            Node targetNode = NodeFromWorldPoint(startPos.position);
            foreach (Node n in nodeGrid)
            {
                if (n.isWall)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.white;
                }
                if (finalPath != null)
                {
                    if (finalPath.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                if (playerNode == n)
                {
                    Gizmos.color = Color.green;
                }
                if (targetNode == n)
                {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - distanceBetweenNodes));
            }
        }
    }
}
