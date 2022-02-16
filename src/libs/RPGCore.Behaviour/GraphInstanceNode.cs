namespace RPGCore.Behaviour;

public struct GraphInstanceNode
{
	public GraphInstance GraphInstance { get; }
	public LocalId Identifier { get; }

	public GraphInstanceNode(
		GraphInstance graphInstance,
		LocalId identifier)
	{
		GraphInstance = graphInstance;
		Identifier = identifier;
	}

	public void UseInput<TType>(Input<TType>? inputSocket, out InputInstance<TType> socket)
	{
		socket = default;
	}

	public void UseOutput<TType>(Output<TType>? outputSocket, out OutputInstance<TType> socket)
	{
		socket = default;
	}

	public ref TNodeData GetNodeInstanceData<TNodeData>(Node node)
		where TNodeData : struct, INodeData
	{
		var array = new TNodeData[1];

		return ref array[0];
	}
}

public struct GraphInstanceNode<TNode>
{
	public GraphInstance GraphInstance { get; }
	public TNode Node { get; }

	public GraphInstanceNode(
		GraphInstance graphInstance,
		TNode node)
	{
		GraphInstance = graphInstance;
		Node = node;
	}

	public void UseInput<TType>(Input<TType>? inputSocket, out InputInstance<TType> socket)
	{
		socket = default;
	}

	public void UseOutput<TType>(Output<TType>? outputSocket, out OutputInstance<TType> socket)
	{
		socket = default;
	}

	public ref TNodeData GetNodeInstanceData<TNodeData>(Node node)
		where TNodeData : struct, INodeData
	{
		var array = new TNodeData[1];

		return ref array[0];
	}
}
