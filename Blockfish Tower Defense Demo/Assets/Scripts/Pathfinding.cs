using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Note Credit to: https://github.com/SebLague/Pathfinding

//Pathfinding Uses the Grid made in Grid which comprises of Nodes
//Using this grid of Nodes we'll be using an A* algorithm 
//Which is adjusted to constantly be checking the current state 
//of the Nodes which changes to determine the shortest route
//To the goal while navigating around the turrets
//That spawn and despawn while the game is running

//This object is attached to each enemy
public class Pathfinding : MonoBehaviour
{
    public Transform destination;
    public GameObject gridContainer;
    public float speed;

    //Path of the object
    public List<Node> path;

    private Grid grid;
    
    //To start we get the grid from the A* object
    void Start()
    {
        grid = gridContainer.GetComponent<Grid>();
    }

    //During the update frame the algorithm determines the current shortest path
    //And then follows that path
    void Update()
    {
        FindPath(transform.position, destination.position);
        FollowPath();
    }

    //Find path is simply the A* algorithm of pathfinding
    void FindPath(Vector3 startPos, Vector3 goalPos)
    {
        //Find the start position of the node grid
        Node startNode = grid.WorldPosToNode(startPos);
        Node goalNode = grid.WorldPosToNode(goalPos);

        //Get an empty lists for the openNodes to be traversed
        //And the closed Nodes that have been traversed
        List<Node> openNodes = new List<Node>();
        List<Node> closedNodes = new List<Node>();

        //Add the starting position to the open Nodes
        openNodes.Add(startNode);

        while (openNodes.Count > 0)
        {
            Node currentNode = openNodes[0];

            //Find the node with lowest fcost
            for (int i = 1; i < openNodes.Count; i++)
            {
                if (openNodes[i].fCost < currentNode.fCost || openNodes[i].fCost == currentNode.fCost)
                {
                    if (openNodes[i].hCost < currentNode.hCost)
                        currentNode = openNodes[i];
                }
            }

            //remove node from open set
            openNodes.Remove(currentNode);

            //Add to close set
            closedNodes.Add(currentNode);

            //If the goal is found retrace the path and set the path
            if(currentNode == goalNode)
            {
                RetracePath(startNode, goalNode);
                return;
            }

            //For each neighbor of the current node
            //if the neighbor is traversible continue the loop
            //if the new path to neighbor is shorter or neighbor is not in closed
            //set fcost, set parentNode, add neighbor to open if not in open

            foreach (Node neighbour in grid.GetNeighborNodes(currentNode))
            {
                //Check to see if neighbor is traverible or in closedset
                if (!neighbour.walkable || closedNodes.Contains(neighbour))
                {
                    continue;
                }

                //Check to see if the neighbor is shorter to get to and/or not in open set
                int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openNodes.Contains(neighbour))
                {
                    //Set gCost, hCost, and parent Node
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, goalNode);
                    neighbour.parent = currentNode;

                    if (!openNodes.Contains(neighbour))
                        openNodes.Add(neighbour);
                }
            }

        }
    }

    void RetracePath(Node start, Node goal)
    {
        List<Node> rPath = new List<Node>();
        Node currentNode = goal;

        while (currentNode != start)
        {
            rPath.Add(currentNode);
            currentNode = currentNode.parent;
        }
        rPath.Reverse();

        path = rPath;
    }

    int GetDistance(Node n, Node g)
    {
        return Mathf.RoundToInt(Vector3.Distance(n.vectPos, g.vectPos));
    }

    //FollowPath simply navigates the enemy to the next neighboring node at a given speed
    void FollowPath()
    {
        if(path.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[0].vectPos, speed);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, destination.position, speed);
        }
    }
}
