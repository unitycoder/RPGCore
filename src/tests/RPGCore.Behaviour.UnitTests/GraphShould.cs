using NUnit.Framework;
using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.Polymorphic.SystemTextJson;
using System.Text.Json;

namespace RPGCore.Behaviour.UnitTests;

[TestFixture(TestOf = typeof(Graph))]
public class GraphShould
{
	[Test, Parallelizable]
	public void SerializeAndDeserialize()
	{
		var jsonSerializerOptions = new JsonSerializerOptions()
			.UsePolymorphicSerialization(options =>
			{
				options.UseInline();
			});
		jsonSerializerOptions.WriteIndented = true;

		var graph = new Graph(new Node[]
		{
			new AddNode()
			{
				Id = LocalId.NewShortId()
			},
			new IterateNode()
			{
				Id = LocalId.NewShortId(),
				Graph = new Graph(new Node[]
				{
					new AddNode()
					{
						Id = LocalId.NewShortId()
					},
					new AddNode()
					{
						Id = LocalId.NewShortId()
					}
				})
			}
		});

		string serializedGraph = JsonSerializer.Serialize(graph, jsonSerializerOptions);

		var deserializedGraph = JsonSerializer.Deserialize<Graph>(serializedGraph, jsonSerializerOptions);

		string reserializedGraph = JsonSerializer.Serialize(deserializedGraph, jsonSerializerOptions);

		Assert.That(serializedGraph, Is.EqualTo(reserializedGraph));

		TestContext.Out.WriteLine(reserializedGraph);
	}
}
