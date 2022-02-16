using System.Collections.Generic;

namespace RPGCore.Behaviour;

public class GraphInstanceData
{
	public Dictionary<string, INodeData> Nodes { get; set; } = new();

	public Dictionary<string, IOutputData> Outputs { get; set; } = new();
}
