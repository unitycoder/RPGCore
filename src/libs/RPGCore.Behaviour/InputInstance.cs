namespace RPGCore.Behaviour;

public struct InputInstance<TType>
{
	internal GraphInstance graphInstance;
	internal Input<TType> input;

	public TType Value { get; set; }
	public bool HasChanged { get; }
}
