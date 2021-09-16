using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    HexGrid grid;

    private void Awake()
    {
        grid = GetComponentInChildren<HexGrid>();
    }

    // class that finds a path from a seeker to a target
    public List<Tile> FindPath(Tile startNode, Tile targetNode)
    {
        List<Tile> openSet = new List<Tile>();
        HashSet<Tile> closedSet = new HashSet<Tile>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Tile currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                //output the path to draw
                return RetracePath(startNode, targetNode);
            }

            foreach (Tile neighbor in grid.GetNeighbors(currentNode))
            {
                if (!neighbor.GetTileTraversable() || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        return null;
    }

    List<Tile> RetracePath(Tile startNode, Tile endNode)
    {
        List<Tile> path = new List<Tile>();
        Tile currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    int GetDistance(Tile nodeA, Tile nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridPos.x - nodeB.gridPos.x);
        int dstY = Mathf.Abs(nodeA.gridPos.y - nodeB.gridPos.y);
        int dstZ = Mathf.Abs(nodeA.gridPos.z - nodeB.gridPos.z);

        return 10 * (dstX + dstY + dstZ);
    }
}
