using UnityEngine;
using System.Collections.Generic;
using Pathfindr;

public class GameController : MonoBehaviour 
{
	private const bool ALLOW_DIAGONAL = false;
	private const int GRID_SIZE = 10;
	private const string GROUND_LAYER = "Ground";

	private LayerMask ObstacleLayer = 1 << 8;
	private PFScene scene;
	private PFEngine pathfinder;
	private Vector2Int currentPos;

	private void Start() 
	{
		pathfinder = new PFEngine();
		scene = gameObject.AddComponent<PFScene>();

		List<int> obstacles = scene.Evaluate(GRID_SIZE, ObstacleLayer);

		currentPos = new Vector2Int(0, 0);

		pathfinder.InitGrid(GRID_SIZE, GRID_SIZE, obstacles);

		/*List<int> path = pathfinder.GetPath(currentPos, new Vector2Int(8, 8), ALLOW_DIAGONAL);
		foreach(int gridref in path) 
		{
			Debug.Log(gridref);
		}*/
	}

	private void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Vector2Int? newPos = scene.CheckHit(Input.mousePosition, GROUND_LAYER);

			if(newPos != null)
			{
				List<Vector2Int> path = pathfinder.GetPath(currentPos, newPos.Value, ALLOW_DIAGONAL);
				if(path != null)
				{
					foreach(Vector2Int gridref in path)
					{
						Debug.Log(gridref);
					}

					currentPos = path[path.Count - 1];
				}
			}
		}
	}
}