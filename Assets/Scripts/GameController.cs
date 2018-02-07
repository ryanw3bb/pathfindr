using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour 
{
	public LayerMask layerMask;

	private void Start () 
	{
		PFController pathfinder = gameObject.AddComponent<PFController>();
		//List<IVector2> obstacles = pathfinder.EvaluateScene(100, layerMask);
		pathfinder.InitGrid(6, 7, new List<int> () { 15, 25, 27, 33 });

		List<int> path = pathfinder.GetPath(new IVector2(1, 2), new IVector2(4, 5), true);
		foreach(int gridref in path) 
		{
			Debug.Log (gridref);
		}
	}
}