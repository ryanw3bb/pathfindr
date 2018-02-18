using System;
using UnityEngine;

public static class Vector2IntExtensions 
{
	// return Manhattan distance between 2 Vector2Ints

	public static int ManhattanDistance(this Vector2Int p1, Vector2Int p2)
	{
		return Mathf.Abs((p1.x - p2.x) + (p1.y - p2.y));
	}


	// convert to standard Vector2

	public static Vector2 ToVector2(this Vector2Int v)
	{
		return new Vector2(Convert.ToSingle(v.x), Convert.ToSingle(v.y));
	}
}