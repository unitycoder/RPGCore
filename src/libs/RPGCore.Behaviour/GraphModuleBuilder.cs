using System.Collections.Generic;

namespace RPGCore.Behaviour;

public class GraphModuleBuilder
{
	private readonly Dictionary<string, Graph> graphs = new();

	public GraphModuleBuilder UseGraph(
		string identifier,
		Graph graph)
	{
		graphs.Add(identifier, graph);
		return this;
	}

	public GraphModule Build()
	{
		return new GraphModule(graphs);
	}
}
