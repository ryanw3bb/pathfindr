using System.Collections.Generic;
using UnityEngine;

namespace Pathfindr 
{
	public class PFGrid : MonoBehaviour 
	{
		protected List<int> obstaclePositions;
		protected int gridResolution;
		protected Bounds bounds;
		protected Vector2 cellSize;
		protected Vector2 gridOffset;

		public List<int> Evaluate(int gridResolution, Bounds bounds, LayerMask obstacleLayers)
		{
			this.bounds = bounds;
			this.gridResolution = gridResolution;
			obstaclePositions = new List<int>();

			RaycastHit hit;
			Vector3 rayPos;
			cellSize = new Vector2(bounds.size.x / gridResolution, bounds.size.z / gridResolution);
			gridOffset = new Vector2(bounds.center.x + bounds.extents.x, bounds.center.z + bounds.extents.z);

			for(int i = 0; i < gridResolution; i++)
			{
				for(int j = 0; j < gridResolution; j++)
				{
					rayPos = GridToWorldPosition(new Vector2Int(i, j));
					rayPos.y = bounds.max.y + 1;

					if(Physics.Raycast(rayPos, -Vector3.up, out hit, Mathf.Infinity, obstacleLayers))
					{
						int node = (j * gridResolution) + i;
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
			return new Vector2Int(Mathf.FloorToInt(((bounds.max.x - worldPos.x) / (bounds.max.x - bounds.min.x) * gridResolution)),
				Mathf.FloorToInt(((bounds.max.z - worldPos.z) / (bounds.max.z - bounds.min.z)) * gridResolution));
		}

		public Vector3 GridToWorldPosition(Vector2 gridPos)
		{
			return new Vector3(gridOffset.x - cellSize.x * (gridPos.x + .5f), 0, gridOffset.y - cellSize.y * (gridPos.y + .5f));
		}
	}
}