using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour 
{
	public LayerMask ObstacleLayer;

	private const int GRID_SIZE = 10;

	private void Start () 
	{
		PFController pathfinder = gameObject.AddComponent<PFController>();

		List<int> obstacles = PFTools.EvaluateScene(GRID_SIZE, ObstacleLayer);

		pathfinder.InitGrid(GRID_SIZE, GRID_SIZE, new List<int>());

		List<int> path = pathfinder.GetPath(new Vector2Int(0, 0), new Vector2Int(8, 8), false);
		foreach(int gridref in path) 
		{
			Debug.Log(gridref);
		}
	}
}