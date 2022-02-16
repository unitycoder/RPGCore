namespace RPGCore.Behaviour;

public struct GraphInstance
{
	public GraphRuntime Runtime { get; }
	public Graph Graph { get; }
	public GraphInstanceData Data { get; }

	public GraphInstance(
		GraphRuntime runtime,
		Graph graph,
		GraphInstanceData data)
	{
		Runtime = runtime;
		Graph = graph;
		Data = data;
	}

	public void Enable()
	{
		for (int i = 0; i < Graph.Nodes.Count; i++)
		{
			var node = Graph.Nodes[i];
			node.OnEnable(new GraphInstanceNode(this, node.Id));
		}
	}

	public void Disable()
	{
		for (int i = 0; i < Graph.Nodes.Count; i++)
		{
			var node = Graph.Nodes[i];
			node.OnDisable(new GraphInstanceNode(this, node.Id));
		}
	}

	public GraphInstanceNode<TNode> GetNode<TNode>()
		where TNode : Node
	{
		foreach (var node in Graph.Nodes)
		{
			if (node is TNode typedNode)
			{
				return new GraphInstanceNode<TNode>(this, node.Id, typedNode);
			}
		}
		return new GraphInstanceNode<TNode>(this, typedNode);
	}
}
