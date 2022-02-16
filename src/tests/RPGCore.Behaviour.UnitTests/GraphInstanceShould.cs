﻿using NUnit.Framework;
using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.Polymorphic.SystemTextJson;
using System.Collections.Generic;
using System.Text.Json;

namespace RPGCore.Behaviour.UnitTests;

[TestFixture(TestOf = typeof(GraphInstanceData))]
public class GraphInstanceShould
{
	[Test, Parallelizable]
	public void SerializeAndDeserialize()
	{
		var jsonSerializerOptions = new JsonSerializerOptions()
			.UsePolymorphicSerialization(options =>
			{
				options.UseGraphSerialization();
				options.UseInline();
			});
		jsonSerializerOptions.WriteIndented = true;

		var graph = new Graph(new Node[]
		{
			new AddNode() {
				Id = LocalId.NewShortId()
			},
			new IterateNode() {
				Id = LocalId.NewShortId(),
				Graph = new Graph(new Node[]
				{
					new AddNode() {
						Id = LocalId.NewShortId()
					},
					new AddNode() {
						Id = LocalId.NewShortId()
					}
				})
			}
		});

		var graphInstance = graph.CreateInstanceData();
		graphInstance.Nodes["123"] = new IterateNode.IterateNodeData()
		{
			Instances = new List<GraphInstanceData>()
		};
		graphInstance.Outputs["123"] = new Output<float>.OutputData()
		{
			Value = 10.0f
		};

		string serializedGraphInstance = JsonSerializer.Serialize(graphInstance, jsonSerializerOptions);

		var deserializedGraphInstance = JsonSerializer.Deserialize<GraphInstanceData>(serializedGraphInstance, jsonSerializerOptions);

		string reserializedGraphInstance = JsonSerializer.Serialize(deserializedGraphInstance, jsonSerializerOptions);

		Assert.That(serializedGraphInstance, Is.EqualTo(reserializedGraphInstance));

		TestContext.Out.WriteLine(reserializedGraphInstance);
	}
}
