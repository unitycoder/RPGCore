﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RPGCore.Data.Polymorphic.Internal
{
	/// <inheritdoc/>
	internal class PolymorphicConverter : JsonConverter<object>
	{
		private readonly PolymorphicConverterFactoryOptions polymorphicConverterFactoryOptions;
		private readonly Type converterType;
		private readonly TypeNameInformation[] validTypes;

		internal PolymorphicConverter(PolymorphicConverterFactoryOptions polymorphicConverterFactoryOptions, Type converterType)
		{
			this.polymorphicConverterFactoryOptions = polymorphicConverterFactoryOptions;
			this.converterType = converterType;
			validTypes = TypeNameInformation
				.GetUserDefinedOptions(converterType,
					polymorphicConverterFactoryOptions.DefaultNamingConvention,
					polymorphicConverterFactoryOptions.DefaultAliasConventions)
				.ToArray();
		}

		/// <inheritdoc/>
		public override object? Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Null)
			{
				return null;
			}

			var peek = reader;

			if (peek.TokenType != JsonTokenType.StartObject)
			{
				throw new JsonException("JsonTokenType.StartObject not found.");
			}

			if (!peek.Read()
				|| peek.TokenType != JsonTokenType.PropertyName
				|| peek.GetString() != "$type")
			{
				throw new JsonException("Property \"$type\" not found.");
			}

			if (!peek.Read()
				|| peek.TokenType != JsonTokenType.String)
			{
				throw new JsonException("Value at \"$type\" is invalid.");
			}

			string? typeName = peek.GetString();

			if (typeName == null)
			{
				throw CreateInvalidTypeException(typeName);
			}

			Type? type = null;
			foreach (var option in validTypes)
			{
				if (option.DoesNameMatch(typeName, polymorphicConverterFactoryOptions.CaseInsensitive))
				{
					type = option.Type;
				}
			}
			if (type == null)
			{
				throw CreateInvalidTypeException(typeName);
			}

			return JsonSerializer.Deserialize(ref reader, type, options);
		}

		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();

			var valueType = value.GetType();
			var typeInformation = GetTypeInformation(valueType);

			if (typeInformation == null)
			{
				throw new InvalidOperationException($"Cannot serialize value of type '{valueType.FullName}' as it's not one of the allowed types.");
			}

			writer.WriteString("$type", typeInformation.Name);

			var buffer = new MemoryStream();
			using (var bufferWriter = new Utf8JsonWriter(buffer, new JsonWriterOptions()
			{
				Encoder = options.Encoder,
				Indented = options.WriteIndented
			}))
			{
				JsonSerializer.Serialize(bufferWriter, value, value.GetType(), options);
			}
			buffer.Seek(0, SeekOrigin.Begin);

			using (var document = JsonDocument.Parse(buffer, new JsonDocumentOptions
			{
				AllowTrailingCommas = options.AllowTrailingCommas,
				MaxDepth = options.MaxDepth,
				CommentHandling = options.ReadCommentHandling
			}))
			{
				foreach (var jsonProperty in document.RootElement.EnumerateObject())
				{
					jsonProperty.WriteTo(writer);
				}
			}

			writer.WriteEndObject();
		}

		private TypeNameInformation? GetTypeInformation(Type valueType)
		{
			TypeNameInformation? typeInformation = null;
			foreach (var polymorphicType in validTypes)
			{
				if (polymorphicType.Type == valueType)
				{
					typeInformation = polymorphicType;
				}
			}
			return typeInformation;
		}

		private JsonException CreateInvalidTypeException(string? typeName)
		{
			var sb = new StringBuilder();
			sb.Append($"\"$type\" value of \"{typeName}\" is invalid.\nValid options for \"{converterType.FullName}\" are:");

			foreach (var validOption in validTypes)
			{
				sb.Append("\n- '");
				sb.Append(validOption.Name);
				sb.Append("'");

				if (validOption.Aliases != null)
				{
					sb.Append(", also known as '");
					sb.Append(string.Join("', '", validOption.Aliases));
				}
				sb.Append("'");
			}

			return new JsonException(sb.ToString());
		}
	}
}
