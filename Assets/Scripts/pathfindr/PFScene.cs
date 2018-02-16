using System.Collections.Generic;
using UnityEngine;

public class PFScene : MonoBehaviour 
{
	public static List<int> Evaluate(int sampleResolution, LayerMask obstacleLayers)
	{
		List<int> obstaclePositions = new List<int>();

		Bounds b = new Bounds(Vector3.zero, Vector3.zero);
		foreach(Renderer r in FindObjectsOfType(typeof(Renderer))) 
		{
			b.Encapsulate(r.bounds);
		}

		RaycastHit hit;
		Vector3 pos;
		float cellSizeX = (b.size.x / sampleResolution);
		float cellSizeZ = (b.size.z / sampleResolution);
		float offsetX = -(b.size.x / 2f);
		float offsetZ = -(b.size.z / 2f);

		for(int i = 0; i < sampleResolution; i++)
		{
			for(int j = 0; j < sampleResolution; j++)
			{
				pos = new Vector3(offsetX + cellSizeX * (i + .5f), b.max.y + 1, offsetZ + cellSizeZ * (j + .5f));

				//Debug.Log(pos);

				if(Physics.Raycast(pos, -Vector3.up, out hit, Mathf.Infinity, obstacleLayers))
				{
					int node = i * sampleResolution + j;
					//Debug.Log(hit.collider.name + " " + node);
					obstaclePositions.Add(node);
				}
			}
		}

		//Debug.Log(obstaclePositions.Count);

		return obstaclePositions;
	}
}