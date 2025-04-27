using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public GridBlock gridRef;
    public Transform startPos;
    public Transform targetPos;

    private void Awake()
    {
        gridRef = GetComponent<GridBlock>();
    }

    // Update is called once per frame
    void Update()
    {
        FindPath(startPos.position, targetPos.position);
    }

    void FindPath(Vector3 startPosInp, Vector3 targetPosInp)
    {
        Node startNode = gridRef.NodeFromWorldPoint(startPosInp);
        Node targetNode = gridRef.NodeFromWorldPoint(targetPosInp);

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].TotalCost < currentNode.TotalCost || openList[i].TotalCost == currentNode.TotalCost)
                {
                    if (openList[i].heuristicCost < currentNode.heuristicCost)
                    {
                        currentNode = openList[i];
                    }
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                GetFinalPath(startNode, targetNode);
                return;
            }

            foreach (Node neighbourNode in gridRef.GetNeighboringNodes(currentNode))
            {
                if (neighbourNode.isWall || closedList.Contains(neighbourNode))
                {
                    continue;
                }
                int costToNeighbour = currentNode.moveCost + GetManhattanDistance(currentNode, neighbourNode);

                if (costToNeighbour < neighbourNode.moveCost || !openList.Contains(neighbourNode))
                {
                    neighbourNode.moveCost = costToNeighbour;
                    neighbourNode.heuristicCost = GetManhattanDistance(neighbourNode, targetNode);
                    neighbourNode.ParentNode = currentNode;

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
    }

    void GetFinalPath(Node startNodeInp, Node endNodeInp)
    {
        List<Node> finalPath = new List<Node>();
        Node currentNode = endNodeInp;

        while (currentNode != startNodeInp)
        {
            finalPath.Add(currentNode);
            currentNode = currentNode.ParentNode;
        }

        finalPath.Reverse();

        gridRef.finalPath = finalPath;
    }

    int GetManhattanDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return distanceX + distanceY;
    }
}
