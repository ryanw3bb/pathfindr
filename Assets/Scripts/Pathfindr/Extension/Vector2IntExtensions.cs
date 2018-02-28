using System;
using UnityEngine;

public static class Vector2IntExtensions 
{
	// return Manhattan distance between 2 Vector2Ints

	public static int ManhattanDistance(this Vector2Int p1, Vector2Int p2)
	{
		return Mathf.Abs(p1.x - p2.x) + Mathf.Abs(p1.y - p2.y);
	}

	// return Euclidean (straight line) distance between 2 Vector2Ints

	public static float EuclideanDistance(this Vector2Int p1, Vector2Int p2)
	{
		return (float)Math.Sqrt(Math.Pow((p1.x - p2.x), 2) + Math.Pow((p1.y - p2.y), 2));
	}

	// convert to standard Vector2

	public static Vector2 ToVector2(this Vector2Int v)
	{
		return new Vector2(Convert.ToSingle(v.x), Convert.ToSingle(v.y));
	}
}