using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour;

public class IterateNode : Node
{
	public struct IterateNodeData : INodeData
	{
		public List<GraphInstance> Instances { get; set; }
	}

	public Input<int> Iterations { get; set; }
	public Graph Graph { get; set; } = new Graph(Array.Empty<Node>());

	public override void OnCreateInstance(GraphInstance graphInstance)
	{
		UpdateIterations(graphInstance);
	}

	public override void OnInputChanged(GraphInstance graphInstance)
	{
		UpdateIterations(graphInstance);
	}

	private void UpdateIterations(GraphInstance graphInstance)
	{
		graphInstance.UseInput(Iterations, out var iterations);

		if (iterations.HasChanged)
		{
			// ref var data = ref graphInstance.GetNodeInstanceData<IterateNodeData>(this);
			// 
			// // Remove unused instances
			// while (data.Instances.Count > iterations.Value)
			// {
			// 	data.Instances.RemoveAt(data.Instances.Count - 1);
			// }
			// 
			// // Add additional instances
			// while (data.Instances.Count < iterations.Value)
			// {
			// 	data.Instances.Add(Graph.CreateInstance());
			// }
		}
	}
}
