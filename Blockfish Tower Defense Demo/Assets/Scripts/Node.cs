using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Note Credit to: https://github.com/SebLague/Pathfinding

//Class for Nodes to make a grid of Nodes for my A* pathfinding to navigate
public class Node
{

	//We need to see where the node is in the world
	//Whether its walkable
	//And where it is on the grid of nodes
	public bool walkable;
	public Vector3 vectPos;
	public int gridX;
	public int gridY;

	//The gCost, hCost, and Parent are used in the A* algorithm
	public int gCost;
	public int hCost;
	public Node parent;

	public Node(bool _walkable, Vector3 _vectPos, int _gridX, int _gridY)
	{
		walkable = _walkable;
		vectPos = _vectPos;
		gridX = _gridX;
		gridY = _gridY;
	}

	public int fCost
	{
		get
		{
			return gCost + hCost;
		}
	}
}
