namespace RPGCore.Behaviour;

public class AddNode : Node
{
	public Input<float> ValueA { get; set; }
	public Input<float> ValueB { get; set; }
	public Output<float> Output { get; set; }

	public override void OnInputChanged(GraphInstance graphInstance)
	{
		graphInstance.UseInput(ValueA, out var valueA);
		graphInstance.UseInput(ValueB, out var valueB);
		graphInstance.UseOutput(Output, out var output);

		output.Value = valueA.Value + valueB.Value;
	}
}
