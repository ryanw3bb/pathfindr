using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PFController : MonoBehaviour 
{
	private PFNode[,] nodes;
	private List<PFNode> openNodes;
	private int xNodes, yNodes;
	private float stdMoveCost = 10;
	private float diagMoveCost = 14;

	public List<IVector2> EvaluateScene(int sampleResolution, LayerMask obstacleLayers)
	{
		List<IVector2> obstaclePositions = new List<IVector2>();
		
		Bounds b = new Bounds(Vector3.zero, Vector3.zero);
		foreach(Renderer r in FindObjectsOfType(typeof(Renderer))) 
		{
			b.Encapsulate (r.bounds);
		}

		Debug.Log (b.center + " " + b.size);
		
		RaycastHit hit;
		Vector3 pos;
		
		for(int i = 0; i < sampleResolution; i++)
		{
			for(int j = 0; i < sampleResolution; j++)
			{
				pos = new Vector3((b.size.x / sampleResolution) * i, 100, (b.size.y / sampleResolution) * j);

				if(Physics.Raycast(pos, -Vector3.up, out hit, obstacleLayers))
				{
					obstaclePositions.Add(new IVector2(j, i));
				}
			}
		}

		Debug.Log (obstaclePositions.Count);
		
		return obstaclePositions;
	}
	
	public void InitGrid(int gridXSize, int gridYSize, List<int> closedGridRefs) 
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
				PFNode node = new PFNode(nodeRef, new IVector2(j, i));
				nodes[j, i] = node;
				
				if(closedGridRefs.Contains(nodeRef)) 
				{
					CloseNode(node);
				}
				
				nodeRef++;
			}
		}
	}

	public List<int> GetPath(IVector2 startPos, IVector2 targetPos, bool allowDiagonal = true)
	{
		foreach(PFNode node in nodes) 
		{
			if(node.Open) 
			{
				node.H = Mathf.Abs((node.Position.x - targetPos.x) + (node.Position.y - targetPos.y));
			}
		}
		
		nodes [targetPos.x, targetPos.y].Target = true;
		
		bool solved = false;
		float moveCost;
		PFNode parentNode = nodes [startPos.x, startPos.y];
		PFNode nextNode = null;
		PFNode currentNode = null;
		
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
					
					if(j != parentNode.Position.x && i != parentNode.Position.y)
					{
						moveCost = diagMoveCost;
					}
					else
					{
						moveCost = stdMoveCost;
					}
					
					if(currentNode.G == 0 || (parentNode.G + moveCost) < currentNode.G)
					{
						currentNode.Parent = parentNode;
						currentNode.G = parentNode.G + moveCost;
						currentNode.F = currentNode.H + currentNode.G;
					}
				}
			}
			
			if(!solved)
			{
				/*if(nextNode == null)
				{
					Debug.Log ("can't reach target");
					break; 	
				}*/
				
				foreach(PFNode node in openNodes)
				{
					if(node.F != 0)
					{
						if(nextNode == null || node.F < nextNode.F)
						{
							nextNode = node;
						}
					}
				}

				//Debug.Log ("next node: " + nextNode.NodeRef);
				
				parentNode = nextNode;
				nextNode = null;
			}
		}
		
		List<int> path = new List<int> ();
		currentNode = nodes [targetPos.x, targetPos.y];
		
		do
		{
			path.Add(currentNode.NodeRef);
			currentNode = currentNode.Parent;
		} 
		while (currentNode != nodes [startPos.x, startPos.y]);
		
		path.Add (nodes [startPos.x, startPos.y].NodeRef);
		path.Reverse ();
		
		return path;
	}
	
	private void CloseNode(PFNode node)
	{
		node.Open = false;
		openNodes.Remove (node);
	}
}