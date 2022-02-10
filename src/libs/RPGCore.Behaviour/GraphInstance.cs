using System.Collections.Generic;

namespace RPGCore.Behaviour;

public class GraphInstance
{
	public Graph Graph { get; set; }

	public Dictionary<string, INodeData> Nodes { get; set; } = new();

	public Dictionary<string, IOutputData> Outputs { get; set; } = new();

	public void UseInput<TType>(Input<TType>? inputSocket, out InputInstance<TType> socket)
	{
		socket = default;
	}
	public void UseOutput<TType>(Output<TType>? outputSocket, out OutputInstance<TType> socket)
	{
		socket = default;
	}

	public ref T GetNodeInstanceData<T>(Node node)
		where T : struct, INodeData
	{
		var array = new T[1];

		return ref array[0];
	}
}
