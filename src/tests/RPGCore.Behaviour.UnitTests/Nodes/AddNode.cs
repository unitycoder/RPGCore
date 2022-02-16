namespace RPGCore.Behaviour;

public class AddNode : Node
{
	public Input<float> ValueA { get; set; }
	public Input<float> ValueB { get; set; }
	public Output<float> Output { get; set; }

	public override void OnInputChanged(GraphInstanceNode node)
	{
		node.UseInput(ValueA, out var valueA);
		node.UseInput(ValueB, out var valueB);
		node.UseOutput(Output, out var output);

		output.Value = valueA.Value + valueB.Value;
	}
}
