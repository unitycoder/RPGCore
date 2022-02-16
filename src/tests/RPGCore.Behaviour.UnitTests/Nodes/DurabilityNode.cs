namespace RPGCore.Behaviour;

public class DurabilityNode : Node
{
	public struct DurabilityNodeData : INodeData
	{
		public int MaxDurability { get; set; } 
	}

	public Input<float> Input { get; set; }
	public Output<float> Output { get; set; }

	public override void OnInputChanged(GraphInstanceNode node)
	{
		node.UseInput(Input, out var input);
		node.UseOutput(Output, out var output);

		output.Value += input.Value;

		ref var data = ref node.GetNodeInstanceData<DurabilityNodeData>(this);

		data.MaxDurability += 1;
	}
}
