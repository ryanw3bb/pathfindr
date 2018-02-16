using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PFController : MonoBehaviour 
{
	private const float ADJACENT_MOVE_COST = 1;
	private const float DIAGONAL_MOVE_COST = 1.4f;
	private const int MAX_ITERATIONS = 5000;

	private PFNode[,] nodes;
	private List<PFNode> openNodes;
	private int xNodes, yNodes;
	
	public void InitGrid(int gridXSize, int gridYSize, List<int> closedNodes) 
	{
		int nodeRef = 0;
		
		xNodes = gridXSize;
		yNodes = gridYSize;
		
		nodes = new PFNode[xNodes, yNodes];
		openNodes = new List<PFNode>();
		
		for(int i = 0; i < yNodes; i++)
		{
			for(int j = 0; j < xNodes; j++)
			{
				PFNode node = new PFNode(nodeRef, new Vector2Int(j, i));
				nodes[j, i] = node;
				
				if(closedNodes.Contains(nodeRef)) 
				{
					CloseNode(node);
				}
				
				nodeRef++;
			}
		}
	}

	public List<int> GetPath(Vector2Int startPos, Vector2Int targetPos, bool allowDiagonal = true)
	{
		// find the distance of each node from target grid ref
		foreach(PFNode node in nodes) 
		{
			if(node.Open) 
			{
				node.H = GetManhattanDistance(node.Position, targetPos);
			}
		}
		
		nodes[targetPos.x, targetPos.y].Target = true;

		Debug.Log(nodes[targetPos.x, targetPos.y].ToString());
		
		bool solved = false;
		float moveCost;
		PFNode parentNode = nodes[startPos.x, startPos.y];
		PFNode nextNode = null;
		PFNode currentNode = null;
		int attempts = 0;
		
		while(!solved)
		{
			CloseNode(parentNode);
			
			for(int i = parentNode.Position.y-1; i <= parentNode.Position.y+1; i++)
			{
				// check bounds y
				if(i < 0 || i >= yNodes) { continue; }
				
				for(int j = parentNode.Position.x-1; j <= parentNode.Position.x+1; j++)
				{
					// check bounds x
					if(j < 0 || j >= xNodes) { continue; }

					// diagonal node check
					if(!allowDiagonal && j != parentNode.Position.x && i != parentNode.Position.y) { continue; }
					
					currentNode = nodes[j, i];
					
					if(!currentNode.Open) { continue; }
					
					openNodes.Add(currentNode);
					
					if(currentNode.Target) { solved = true; }
					
					if(j != parentNode.Position.x && i != parentNode.Position.y)
					{
						moveCost = DIAGONAL_MOVE_COST;
					}
					else
					{
						moveCost = ADJACENT_MOVE_COST;
					}
						
					if(currentNode.G == 0 || (parentNode.G + moveCost) < currentNode.G)
					{
						currentNode.ParentPosition = parentNode.Position;
						currentNode.G = parentNode.G + moveCost;
						currentNode.F = currentNode.H + currentNode.G;

						Debug.Log(currentNode.ToString());
					}
				}
			}
			
			if(!solved)
			{
				foreach(PFNode node in openNodes)
				{
					if(node.F != 0)
					{
						if(nextNode == null || node.F <= nextNode.F)
						{
							nextNode = node;
						}
					}
				}

				Debug.Log("next node: " + nextNode.NodeRef);
				
				parentNode = nextNode;
				nextNode = null;
			}

			attempts++;

			if(attempts > MAX_ITERATIONS)
			{
				break;
			}
		}
		
		List<int> path = new List<int>();
		currentNode = nodes[targetPos.x, targetPos.y];
		
		do
		{
			path.Add(currentNode.NodeRef);
			currentNode = nodes[currentNode.ParentPosition.x, currentNode.ParentPosition.y];
		} 
		while(currentNode != nodes[startPos.x, startPos.y]);
		
		path.Add(nodes[startPos.x, startPos.y].NodeRef);
		path.Reverse();
		
		return path;
	}

	private int GetManhattanDistance(Vector2Int p1, Vector2Int p2)
	{
		return Mathf.Abs((p1.x - p2.x) + (p1.y - p2.y));
	}
	
	private void CloseNode(PFNode node)
	{
		node.Open = false;
		openNodes.Remove(node);
	}
}