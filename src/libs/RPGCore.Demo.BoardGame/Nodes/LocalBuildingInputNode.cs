using RPGCore.Behaviour;

namespace RPGCore.Demo.BoardGame;

public class LocalBuildingInputNode : NodeTemplate<LocalBuildingInputNode>
{
	public Output Owner;
	public Output Building;

	public override Instance Create()
	{
		return new LocalBuildingInputInstance();
	}

	public class LocalBuildingInputInstance : Instance
	{
		public Output<LobbyPlayer> Owner;
		public Output<Building> Building;

		public override InputMap[] Inputs(ConnectionMapper connections) => null;

		public override OutputMap[] Outputs(ConnectionMapper connections) => new[]
		{
			connections.Connect(ref Template.Owner, ref Owner),
			connections.Connect(ref Template.Building, ref Building)
		};

		public override void Setup()
		{
		}

		public override void Remove()
		{
		}
	}
}
