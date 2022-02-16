using RPGCore.Behaviour;

namespace RPGCore.Documentation.Samples.RPGCore.Behaviour.AddNodeSample;

public class AddNode : Node
{
	public Input<float> ValueA;
	public Input<float> ValueB;

	public Output<float> Output;

	public override void OnInputChanged(GraphInstanceData graphInstance)
	{
		graphInstance.UseInput(ValueA, out var valueA);
		graphInstance.UseInput(ValueB, out var valueB);
		graphInstance.UseOutput(Output, out var output);

		output.Value = valueA.Value + valueB.Value;
	}
}
