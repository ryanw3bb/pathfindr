using System.Collections.Generic;
using UnityEngine;

namespace Pathfindr 
{
	public class PFScene : PFArea 
	{
		private List<int> obstaclePositions;
		private int gridResolution;
		private Bounds sceneBounds;
		private Vector2 cellSize;
		private Vector2 gridOffset;

		public List<int> Evaluate(int gridSize, LayerMask obstacleLayers)
		{
			sceneBounds = new Bounds(Vector3.zero, Vector3.zero);
			obstaclePositions = new List<int>();
			gridResolution = gridSize;

			foreach(Renderer r in FindObjectsOfType(typeof(Renderer))) 
			{
				sceneBounds.Encapsulate(r.bounds);
			}

			RaycastHit hit;
			Vector3 rayPos;
			cellSize = new Vector2(sceneBounds.size.x / gridSize, sceneBounds.size.z / gridSize);
			gridOffset = new Vector2(sceneBounds.size.x / 2f, sceneBounds.size.z / 2f);

			for(int i = 0; i < gridSize; i++)
			{
				for(int j = 0; j < gridSize; j++)
				{
					rayPos = GridToWorldPosition(new Vector2Int(i, j));
					rayPos.y = sceneBounds.max.y + 1;

					if(Physics.Raycast(rayPos, -Vector3.up, out hit, Mathf.Infinity, obstacleLayers))
					{
						int node = (j * gridSize) + i;
						obstaclePositions.Add(node);
					}
				}
			}

			return obstaclePositions;
		}

		public Vector2Int? CheckHit(Vector3 screenPoint, string groundLayer)
		{
			Ray ray = Camera.main.ScreenPointToRay(screenPoint);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit))
			{
				if(hit.collider.tag == groundLayer)
				{
					return WorldPositionToGrid(hit.point);
				}
			}

			return null;
		}

		public Vector2Int WorldPositionToGrid(Vector3 worldPos)
		{
			return new Vector2Int(Mathf.FloorToInt(((sceneBounds.max.x - worldPos.x) / (sceneBounds.max.x - sceneBounds.min.x) * gridResolution)),
				Mathf.FloorToInt(((sceneBounds.max.z - worldPos.z) / (sceneBounds.max.z - sceneBounds.min.z)) * gridResolution));
		}

		public Vector3 GridToWorldPosition(Vector2 gridPos)
		{
			return new Vector3(gridOffset.x - cellSize.x * (gridPos.x + .5f), 0, gridOffset.y - cellSize.y * (gridPos.y + .5f));
		}
	}
}