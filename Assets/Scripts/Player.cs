using UnityEngine;

public class Player : MonoBehaviour
{
	[HideInInspector] public Vector2Int GridPosition;
	[HideInInspector] public Vector3 WorldPosition;
	[HideInInspector] public bool AllowDiagonal = false;
	[HideInInspector] public float MoveSpeed = 1f;

	public override string ToString()
	{
		return string.Format("[Player: GridPosition={0}, WorldPosition={1}]", GridPosition, WorldPosition);
	}
}