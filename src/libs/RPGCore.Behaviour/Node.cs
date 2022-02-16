using RPGCore.Data.Polymorphic.Inline;

namespace RPGCore.Behaviour;

[SerializeBaseType(TypeName.Name)]
public abstract class Node
{
	public LocalId Id { get; set; }

	public virtual void OnEnable(GraphInstanceNode node)
	{
	}

	public virtual void OnDisable(GraphInstanceNode node)
	{
	}

	public virtual void OnInputChanged(GraphInstanceNode node)
	{
	}
}
