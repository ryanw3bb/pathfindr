using UnityEngine;

public class Player : MonoBehaviour
{
	public Vector2Int GridPosition;
	public Vector3 WorldPosition;
	public bool AllowDiagonal = false;
	public float MoveSpeed = 1f;

	public override string ToString()
	{
		return string.Format("[Player: GridPosition={0}, WorldPosition={1}]", GridPosition, WorldPosition);
	}
}