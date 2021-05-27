﻿namespace RPGCore.Data.Polymorphic
{
	public class InlinePolymorphicOptions
	{
		/// <summary>
		/// Determines the default type name to use when none is supplied in <see cref="SerializeTypeAttribute.NamingConvention"/>.
		/// <para>This property has a default value of <see cref="TypeName.FullName"/>.</para>
		/// </summary>
		public TypeName DefaultNamingConvention { get; set; } = TypeName.FullName;

		/// <summary>
		/// Determines the default type alias convention to use when none is supplied in <see cref="SerializeTypeAttribute.AliasConventions"/>.
		/// <para>This property has a default value of <see cref="TypeName.None"/>.</para>
		/// </summary>
		public TypeName DefaultAliasConventions { get; set; } = TypeName.None;

		public InlinePolymorphicOptions()
		{
		}
	}
}
