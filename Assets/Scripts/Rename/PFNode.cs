using UnityEngine;

namespace Pathfindr 
{
	public class PFNode 
	{
		// H - Heuristic (distance to target node using Manhattan algorithm)
		// G - Movement Cost (the cost of getting to this node from the start node)
		// F - H + G

		public int NodeRef { get; set; }
		public Vector2Int Position { get; set; }
		public Vector2Int ParentPosition { get; set; }
		public bool Open { get; set; }
		public bool Forbidden { get; set; }
		public bool Target { get; set; }
		public float H { get; set; }
		public float G { get; set; }
		public float F { get; set; }

		public PFNode(int nodeRef, Vector2Int position, bool open = true, bool target = false)
		{
			this.NodeRef = nodeRef;
			this.Position = position;
			this.Open = open;
			this.Target = target;
		}

		public void Reset()
		{
			H = 0;
			G = 0;
			F = 0;
			Target = false;
			Open = true;
		}

		public override string ToString()
		{
			return string.Format("[PFNode: NodeRef={0}, Position={1}, Open={2}, Target={3}, H={4}, G={5}, F={6}, ParentPosition={7}]", 
				NodeRef, Position, Open, Target, H, G, F, ParentPosition);
		}
	}
}