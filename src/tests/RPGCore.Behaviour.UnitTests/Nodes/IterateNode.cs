using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour;

public class IterateNode : Node
{
	public struct IterateNodeData : INodeData
	{
		public List<GraphInstanceData> Instances { get; set; }
	}

	public Input<int> Iterations { get; set; }
	public Graph Graph { get; set; } = new Graph(Array.Empty<Node>());

	public override void OnEnable(GraphInstanceNode node)
	{
		UpdateIterations(node);
	}

	public override void OnInputChanged(GraphInstanceNode node)
	{
		UpdateIterations(node);
	}

	private void UpdateIterations(GraphInstanceNode node)
	{
		node.UseInput(Iterations, out var iterations);

		if (iterations.HasChanged)
		{
			// ref var data = ref node.GetNodeInstanceData<IterateNodeData>(this);
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
