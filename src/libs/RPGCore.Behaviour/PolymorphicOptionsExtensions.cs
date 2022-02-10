using RPGCore.Data.Polymorphic;
using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.Polymorphic.Naming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RPGCore.Behaviour;

public static class PolymorphicOptionsBuilderExtensions
{
	public static PolymorphicOptionsBuilder UseGraphSerialization(this PolymorphicOptionsBuilder polymorphicOptions)
	{
		var assemblies = GetDependentAssemblies(AppDomain.CurrentDomain, typeof(SerializeBaseTypeAttribute).Assembly).ToList();

		polymorphicOptions.UseKnownBaseType(typeof(IOutputData), baseType =>
		{
			foreach (var assembly in assemblies)
			{
				var types = assembly.GetTypes();
				foreach (var type in types)
				{
					var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

					foreach (var property in properties)
					{
						if (property.PropertyType.IsGenericType)
						{
							var genericDefinition = property.PropertyType.GetGenericTypeDefinition();
							if (genericDefinition == typeof(Output<>))
							{
								var outputType = property.PropertyType.GetGenericArguments()[0];
								var outputDataTypeGenericDefinition = genericDefinition.GetNestedType("OutputData");
								var outputDataType = outputDataTypeGenericDefinition.MakeGenericType(outputType);

								baseType.UseSubType(outputDataType, subType =>
								{
									subType.Descriminator = GenericNames.ToGenericName(outputType);
								});
							}
						}

						var serialiseBaseTypeAttributes = type.GetCustomAttributes<SerializeBaseTypeAttribute>(false);
					}
				}
			}
		});

		return polymorphicOptions;
	}

	private static IEnumerable<Assembly> GetDependentAssemblies(AppDomain appDomain, Assembly sourceAssembly)
	{
		bool Predicate(Assembly assembly)
		{
			return IsDependentAssemblies(assembly, sourceAssembly);
		}

		return appDomain.GetAssemblies().Where(Predicate);
	}

	private static bool IsDependentAssemblies(Assembly otherAssembly, Assembly sourceAssembly)
	{
		return otherAssembly == sourceAssembly
			|| otherAssembly.GetReferencedAssemblies()
				.Select(assemblyName => assemblyName.FullName)
				.Contains(sourceAssembly.FullName);
	}

}
