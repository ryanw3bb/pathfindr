using System.Collections.Generic;
using UnityEngine;

namespace Pathfindr 
{
	public class PFGridScene : PFGrid
	{
		public List<int> Evaluate(int gridSize, LayerMask obstacleLayers)
		{
			Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);

			foreach(Renderer r in FindObjectsOfType(typeof(Renderer))) 
			{
				bounds.Encapsulate(r.bounds);
			}

			return base.Evaluate(gridSize, bounds, obstacleLayers);
		}
	}
}