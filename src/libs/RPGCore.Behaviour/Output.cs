using System.Text.Json.Serialization;

namespace RPGCore.Behaviour;

public sealed class Output<TType>
{
	public struct OutputData : IOutputData
	{
		[JsonPropertyName("value")]
		public TType Value { get; set; }
	}

	internal int lookupIndex = -1;
}
