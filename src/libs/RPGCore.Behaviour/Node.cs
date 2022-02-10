using RPGCore.Data.Polymorphic.Inline;

namespace RPGCore.Behaviour;

[SerializeBaseType(TypeName.Name)]
public abstract class Node
{
	internal Graph graph;

	public LocalId Id { get; set; }

	public virtual void OnCreateInstance(GraphInstance graphInstance)
	{

	}

	public virtual void OnInputChanged(GraphInstance graphInstance)
	{

	}
}
