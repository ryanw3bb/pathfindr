using UnityEngine;
using System.Collections.Generic;
using Pathfindr;

public class GameController : MonoBehaviour 
{
	public Player Player1;

	private const int GRID_SIZE = 10;
	private const string GROUND_LAYER = "Ground";

	private LayerMask obstacleLayer = 1 << 8;
	private PFGridScene scene;
	private PFEngine pathfinder;
	private List<Vector2Int> waypoints;
	private Vector3 nextWaypointWorldPos;
	private float moveTimer;

	private void Start() 
	{
		scene = gameObject.AddComponent<PFGridScene>();

		List<int> obstacles = scene.Evaluate(GRID_SIZE, obstacleLayer);

		Player1.WorldPosition = nextWaypointWorldPos = Player1.transform.position;
		Player1.GridPosition = scene.WorldPositionToGrid(Player1.WorldPosition);

		pathfinder = new PFEngine(GRID_SIZE, obstacles);
	}

	private void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Vector2Int? newPos = scene.CheckHit(Input.mousePosition, GROUND_LAYER);

			if(newPos != null)
			{
				if(!waypoints.IsNullOrEmpty())
				{
					waypoints = pathfinder.GetPath(waypoints[0], newPos.Value, Player1.AllowDiagonal);
				}
				else
				{
					waypoints = pathfinder.GetPath(Player1.GridPosition, newPos.Value, Player1.AllowDiagonal);
				}
			}
		}

		if(!waypoints.IsNullOrEmpty())
		{
			MovePlayer();
		}
	}

	private void MovePlayer()
	{
		if(Player1.transform.position == nextWaypointWorldPos)
		{
			moveTimer = 0;

			Player1.GridPosition = waypoints[0];
			Player1.WorldPosition = nextWaypointWorldPos;

			waypoints.RemoveAt(0);

			if(!waypoints.IsNullOrEmpty())
			{
				nextWaypointWorldPos = scene.GridToWorldPosition(waypoints[0]);
			}
		}
		else
		{
			moveTimer += Mathf.Pow(Player1.MoveSpeed, Time.deltaTime) / 10f;

			Player1.transform.position = Vector3.Lerp(Player1.WorldPosition, nextWaypointWorldPos, moveTimer);
		}
	}
}