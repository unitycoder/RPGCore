namespace RPGCore.Behaviour;

public class Graph
{
	public Node[] Nodes { get; }

	public Graph(Node[] nodes)
	{
		Nodes = nodes;
	}

	public GraphInstance CreateInstance()
	{
		return new GraphInstance();
	}
}
