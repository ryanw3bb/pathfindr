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
					CloseNode(node, true);
				}
				
				nodeRef++;
			}
		}
	}

	public List<Vector2Int> GetPath(Vector2Int startPos, Vector2Int targetPos, bool allowDiagonal = true)
	{
		if(startPos == targetPos) { return null; }

		openNodes = new List<PFNode>();
		bool solved = false;
		float moveCost;
		PFNode parentNode = nodes[startPos.x, startPos.y];
		PFNode nextNode = null;
		PFNode currentNode = null;
		int iterations = 0;

		foreach(PFNode node in nodes) 
		{
			if(!node.Forbidden) 
			{
				node.Reset();
				node.H = GetManhattanDistance(node.Position, targetPos);
			}
		}

		nodes[targetPos.x, targetPos.y].Target = true;

		Debug.Log(">>> CURRENT : " + nodes[startPos.x, startPos.y].ToString());
		Debug.Log(">>> TARGET : " + nodes[targetPos.x, targetPos.y].ToString());
		
		while(!solved)
		{
			CloseNode(parentNode);
			
			for(int i = parentNode.Position.y-1; i <= parentNode.Position.y+1; i++)
			{
				if(i < 0 || i >= yNodes) { continue; }
				
				for(int j = parentNode.Position.x-1; j <= parentNode.Position.x+1; j++)
				{
					if(j < 0 || j >= xNodes) { continue; }

					if(!allowDiagonal && j != parentNode.Position.x && i != parentNode.Position.y) { continue; }
					
					currentNode = nodes[j, i];
					
					if(!currentNode.Open) { continue; }
					
					openNodes.Add(currentNode);
					
					if(currentNode.Target) { solved = true; }
					
					moveCost = (j != parentNode.Position.x && i != parentNode.Position.y) ? DIAGONAL_MOVE_COST : ADJACENT_MOVE_COST;
						
					if(currentNode.G == 0 || (parentNode.G + moveCost) < currentNode.G)
					{
						currentNode.ParentPosition = parentNode.Position;
						currentNode.G = parentNode.G + moveCost;
						currentNode.F = currentNode.H + currentNode.G;
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

				parentNode = nextNode;
				nextNode = null;
			}

			iterations++;

			if(iterations > MAX_ITERATIONS)
			{
				break;
			}
		}
		
		List<Vector2Int> path = new List<Vector2Int>();
		currentNode = nodes[targetPos.x, targetPos.y];
		
		do
		{
			path.Add(currentNode.Position);
			currentNode = nodes[currentNode.ParentPosition.x, currentNode.ParentPosition.y];
		} 
		while(currentNode != nodes[startPos.x, startPos.y]);
		
		path.Add(nodes[startPos.x, startPos.y].Position);
		path.Reverse();
		
		return path;
	}

	private int GetManhattanDistance(Vector2Int p1, Vector2Int p2)
	{
		return Mathf.Abs((p1.x - p2.x) + (p1.y - p2.y));
	}
	
	private void CloseNode(PFNode node, bool forbidden = false)
	{
		node.Open = false;
		openNodes.Remove(node);

		if(forbidden)
		{
			node.Forbidden = true;
		}
	}
}