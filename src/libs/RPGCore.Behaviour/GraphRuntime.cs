using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour;

public class GraphRuntime
{
	private readonly List<GraphModule> modules = new();

	public IReadOnlyList<GraphModule> LoadedModules => modules;

	public void LoadModule(GraphModule graphModule)
	{
		modules.Add(graphModule);
	}

	public void UnloadModule(GraphModule graphModule)
	{
		if (!modules.Remove(graphModule))
		{
			throw new InvalidOperationException($"Unable to unload module that is not in use.");
		}
	}

	public GraphInstance CreateInstance(Graph graph, GraphInstanceData graphInstance)
	{
		return new GraphInstance(this, graph, graphInstance);
	}
}
