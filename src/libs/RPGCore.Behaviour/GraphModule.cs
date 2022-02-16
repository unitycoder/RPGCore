using System.Collections.Generic;

namespace RPGCore.Behaviour;

public class GraphModule
{
	private readonly Dictionary<string, Graph> graphs;

	public IReadOnlyDictionary<string, Graph> Graphs => graphs;

	internal GraphModule(Dictionary<string, Graph> graphs)
	{
		this.graphs = graphs;
	}

	public static GraphModuleBuilder Create()
	{
		return new GraphModuleBuilder();
	}
}
