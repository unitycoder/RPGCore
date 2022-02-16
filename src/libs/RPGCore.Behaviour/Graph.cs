using System.Collections.Generic;

namespace RPGCore.Behaviour;

public class Graph
{
	private readonly Node[] nodes;

	public IReadOnlyList<Node> Nodes => nodes;

	public Graph(Node[] nodes)
	{
		this.nodes = nodes;
	}

	public GraphInstanceData CreateInstanceData()
	{
		return new GraphInstanceData();
	}
}
