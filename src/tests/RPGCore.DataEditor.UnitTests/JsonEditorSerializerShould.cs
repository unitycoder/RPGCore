﻿using NUnit.Framework;
using RPGCore.DataEditor.CSharp;
using RPGCore.DataEditor.Manifest;
using RPGCore.DataEditor.UnitTests.Utility;

namespace RPGCore.DataEditor.UnitTests
{
	[TestFixture(TestOf = typeof(JsonEditorSerializer))]
	public class JsonEditorSerializerShould
	{
		[EditorType]
		private class SerializerBaseObject
		{
			public string ChildFieldA { get; set; } = "Child Value A";
		}

		[EditorType]
		private class SerializerChildObject : SerializerBaseObject
		{
			public string ChildFieldB { get; set; } = "Child Value B";
		}

		private readonly ProjectManifest schema;
		private readonly EditorSession session;

		public JsonEditorSerializerShould()
		{
			schema = ProjectManifestBuilder.Create()
				.UseFrameworkTypes()
				.UseType(typeof(SerializerBaseObject))
				.UseType(typeof(SerializerChildObject))
				.Build();

			session = new EditorSession(schema, new JsonEditorSerializer());
		}

		[Test, Parallelizable]
		public void LoadCommentsFromJson()
		{
			string testData = "/* This is a file comment */{/* This is a comment, please keep. */ /* There's another comment here too. */ \"ChildFieldA\": /* this is a value comment */\"This is a value\"}";

			var file = session.EditFile()
				.WithType(new TypeName("SerializerChildObject"))
				.LoadFrom(new StringContentLoader(testData))
				.SaveTo(new ConsoleLogSaver())
				.Build();

			file.Save();

			AssertUtility.AssertThatTypeIsEqualTo(file.Root, out EditorObject rootObject);

			Assert.That(rootObject.Comments, Has.Count.EqualTo(1));
			Assert.That(rootObject.Comments[0], Is.EqualTo(" This is a file comment "));
		}

		[Test, Parallelizable]
		public void LoadWithAbstractBaseTypes()
		{
			var file = session.EditFile()
				.WithType(new TypeName("SerializerBaseObject"))
				.LoadFrom(new StringContentLoader(@"
				{
					""$type"": ""SerializerChildObject"",
					""ChildFieldA"": ""Loaded 1"",
					""ChildFieldB"": ""Loaded 2""
				}"))
				.SaveTo(new ConsoleLogSaver())
				.Build();

			file.Save();
		}

		[Test, Parallelizable]
		public void LoadWithUnknownBaseTypes()
		{
			var file = session.EditFile()
				.WithType(TypeName.Unknown)
				.LoadFrom(new StringContentLoader(@"
				{
					""$type"": ""SerializerChildObject"",
					""ChildFieldA"": ""Loaded 1"",
					""ChildFieldB"": ""Loaded 2""
				}"))
				.SaveTo(new ConsoleLogSaver())
				.Build();

			file.Save();
		}
	}
}
