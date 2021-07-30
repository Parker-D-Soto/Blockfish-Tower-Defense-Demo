using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Note Credit to: https://github.com/SebLague/Pathfinding
public class Grid : MonoBehaviour
{
    //This will be attached to an empty object to make a grid of Nodes for the A* to navigate

    //Note the object attached to Grid is at the center of the Grid
    
    //We need to know if the player can walk on the area
    public LayerMask unwalkableLayer;
    //We need a graph of vector2
    public Vector2 gridWorld;
    //We need to make an array for a representation of the graph of nodes and their sizes
    public float nodeRadius;
    Node[,] nodeGrid;

    private float nodeDia;
    private int gridNodesX, gridNodesY;

    private void Start()
    {
        //Using the nodes Diameter we can learn the number of nodes on the x and y
        nodeDia = nodeRadius * 2;
        gridNodesX = Mathf.RoundToInt(gridWorld.x / nodeDia);
        gridNodesY = Mathf.RoundToInt(gridWorld.y / nodeDia);

        //After figuring that out we can create a grid
        CreateGrid();
    }

    //Uses what we found out to actually make the grid of nodes and
    //Whether they can be traversed or not

    //I allowed CreateGrid to be public to be used in TurretSpawn
    //Check that script for more info
    public void CreateGrid()
    {
        nodeGrid = new Node[gridNodesX, gridNodesY];

        //Start at bottom left and make each node from there
        Vector3 bottomLeftGrid = (transform.position) - (Vector3.right * gridWorld.x / 2) - (Vector3.forward * gridWorld.y / 2);

        //Go through each node on grid making an array of Nodes that determine if the space is walkable

        for(int x = 0; x < gridNodesX; x++)
        {
            for(int y = 0; y < gridNodesY; y++)
            {
                //Find the center of each Node location
                Vector3 currentPoint = bottomLeftGrid + (Vector3.right * (x * nodeDia + nodeRadius)) + (Vector3.forward * (y * nodeDia + nodeRadius));
                bool walkable = !(Physics.CheckSphere(currentPoint, nodeRadius, unwalkableLayer));
                nodeGrid[x, y] = new Node(walkable, currentPoint, x, y);
            }
        }
    }

    //This takes in a worldCoordinate and translates it to node coordinates
    public Node WorldPosToNode(Vector3 currentPos)
    {
        Node closestNode = nodeGrid[0, 0];

        for (int x = 0; x < gridNodesX; x++)
        {
            for(int y = 0; y < gridNodesY; y++)
            {
                if(Vector3.Distance(currentPos, nodeGrid[x, y].vectPos) < Vector3.Distance(currentPos, closestNode.vectPos))
                {
                    closestNode = nodeGrid[x, y];
                }
            }
        }

        //Returns the appropriate node grid coordinates
        return nodeGrid[closestNode.gridX, closestNode.gridY];
    }

    //Gets the neighbors of Node within the grid
    public List<Node> GetNeighborNodes(Node startNode)
    {
        List<Node> neighbors = new List<Node>();

        //Goes through every available neighbor around the node
        for(int x=-1; x<=1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //Skips the starter node which is in the center
                if(x==0 && y == 0)
                {
                    continue;
                }

                //Gets the exact Node coordinates of the neighbor Node
                int neighborX = startNode.gridX + x;
                int neighborY = startNode.gridY + y;

                //Checks to see if the neighbor is within the grid
                if (neighborX >= 0 && neighborX < gridNodesX && neighborY >= 0 && neighborY < gridNodesY)
                {
                    //Adds the neighbor to the list of neighbors
                    neighbors.Add(nodeGrid[neighborX, neighborY]);
                }
            }
        }

        //Returns neighbors
        return neighbors;
    }

    //This lets me view the size of the grid in the editor to learn how big it needs to be
    //To match the game area
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorld.x, 1, gridWorld.y));

        if(nodeGrid != null)
        {
            //Color whether the current node is walkable
            //Also leave a tiny space to avoid overlap
            foreach (Node n in nodeGrid)
            {
                Gizmos.color = (n.walkable) ? Color.blue : Color.red;
                Gizmos.DrawCube(n.vectPos, Vector3.one * (nodeDia - 0.1f));
            }
        }
    }
}
