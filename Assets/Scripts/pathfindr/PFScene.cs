using System.Collections.Generic;
using UnityEngine;

public class PFScene : PFArea 
{
	public List<int> ObstaclePositions;
	public int SampleResolution;
	public Bounds SceneBounds;

	public List<int> Evaluate(int sampleResolution, LayerMask obstacleLayers)
	{
		SceneBounds = new Bounds(Vector3.zero, Vector3.zero);
		ObstaclePositions = new List<int>();
		SampleResolution = sampleResolution - 1;

		foreach(Renderer r in FindObjectsOfType(typeof(Renderer))) 
		{
			SceneBounds.Encapsulate(r.bounds);
		}

		RaycastHit hit;
		Vector3 rayPos;
		float cellSizeX = (SceneBounds.size.x / SampleResolution);
		float cellSizeZ = (SceneBounds.size.z / SampleResolution);
		float offsetX = -(SceneBounds.size.x / 2f);
		float offsetZ = -(SceneBounds.size.z / 2f);

		for(int i = 0; i < SampleResolution; i++)
		{
			for(int j = 0; j < SampleResolution; j++)
			{
				rayPos = new Vector3(offsetX + cellSizeX * (i + .5f), SceneBounds.max.y + 1, offsetZ + cellSizeZ * (j + .5f));

				if(Physics.Raycast(rayPos, -Vector3.up, out hit, Mathf.Infinity, obstacleLayers))
				{
					int node = i * SampleResolution + j;
					ObstaclePositions.Add(node);
				}
			}
		}

		return ObstaclePositions;
	}

	public Vector2Int? CheckHit(Vector3 mousePosition, string groundLayer)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit))
		{
			if(hit.collider.tag == groundLayer)
			{
				return new Vector2Int(Mathf.RoundToInt(((SceneBounds.max.x - hit.point.x) / (SceneBounds.max.x - SceneBounds.min.x) * SampleResolution)),
					Mathf.RoundToInt(((SceneBounds.max.z - hit.point.z) / (SceneBounds.max.z - SceneBounds.min.z)) * SampleResolution));
			}
		}

		return null;
	}
}