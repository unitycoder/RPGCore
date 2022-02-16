using NUnit.Framework;
using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.Polymorphic.SystemTextJson;
using System.Collections.Generic;
using System.Text.Json;

namespace RPGCore.Behaviour.UnitTests;

[TestFixture(TestOf = typeof(GraphRuntime))]
public class GraphRuntimeShould
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

		var graphRuntime = new GraphRuntime();

		var swordItemGraph = new Graph(new Node[]
		{
			new AddNode() {
				Id = LocalId.NewShortId()
			},
			new DurabilityNode() {
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

		var mainModule = GraphModule.Create()
			.UseGraph("graph-1", swordItemGraph)
			.Build();

		graphRuntime.LoadModule(mainModule);
		var weaponGraph = mainModule.Graphs["graph-1"];

		var graphInstanceData = weaponGraph.CreateInstanceData();
		{
			graphInstanceData.Nodes["123"] = new IterateNode.IterateNodeData()
			{
				Instances = new List<GraphInstanceData>()
			};
			graphInstanceData.Outputs["123"] = new Output<float>.OutputData()
			{
				Value = 10.0f
			};
		}

		var graphInstance = graphRuntime.CreateInstance(weaponGraph, graphInstanceData);
		
		graphInstance.Enable();
		var durabilityNode = graphInstance.GetNode<DurabilityNode>();
		if (durabilityNode.Node != null)
		{
			durabilityNode.UseOutput(durabilityNode.Node.Output, out var output);
			output.Value += 5.0f;
		}
		graphInstance.Disable();

		string serializedGraphInstance = JsonSerializer.Serialize(graphInstanceData, jsonSerializerOptions);
		var deserializedGraphInstance = JsonSerializer.Deserialize<GraphInstanceData>(serializedGraphInstance, jsonSerializerOptions);
		string reserializedGraphInstance = JsonSerializer.Serialize(deserializedGraphInstance, jsonSerializerOptions);

		Assert.That(serializedGraphInstance, Is.EqualTo(reserializedGraphInstance));

		TestContext.Out.WriteLine(reserializedGraphInstance);
	}
}
