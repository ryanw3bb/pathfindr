using UnityEngine;

public class PFNode 
{
	// H - Heuristic (distance to target using Manhattan algorithm)
	// G - Movement Cost (the cost of getting to this node from the parent node)
	// F - H + G

	private int nodeRef;
	private float h, g, f;
	private IVector2 position;
	private PFNode parent;
	private bool open;
	private bool canMove;
	private bool target;

	public PFNode(int nodeRef, IVector2 position, bool open = true, bool target = false)
	{
		this.nodeRef = nodeRef;
		this.position = position;
		this.open = open;
		this.target = target;
	}

	public int NodeRef
	{
		get { return nodeRef; }
		set { nodeRef = value; }
	}

	public IVector2 Position
	{
		get { return position; }
		set { position = value; }
	}

	public bool Open
	{
		get { return open; }
		set { open = value; }
	}

	public bool Target
	{
		get { return target; }
		set { target = value; }
	}

	public float H
	{
		get { return h; }
		set { h = value; }
	}

	public float G
	{
		get { return g; }
		set { g = value; }
	}

	public float F
	{
		get { return f; }
		set { f = value; }
	}

	public PFNode Parent
	{
		get { return parent; }
		set { parent = value; }
	}
}