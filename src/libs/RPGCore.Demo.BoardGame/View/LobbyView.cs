﻿using Newtonsoft.Json;
using RPGCore.Demo.BoardGame.Models;
using RPGCore.Packages;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace RPGCore.Demo.BoardGame;

public class LobbyView
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)] private LobbyPlayerCollection players;
	[DebuggerBrowsable(DebuggerBrowsableState.Never)] private GameplayView gameplay;

	public LobbyPlayerCollection Players
	{
		get => HierachyHelper.Get(this, ref players);
		set => HierachyHelper.Set(this, ref players, value);
	}

	public GameplayView Gameplay
	{
		get => HierachyHelper.Get(this, ref gameplay);
		set => HierachyHelper.Set(this, ref gameplay, value);
	}

	public IExplorer GameData { get; private set; }
	public Dictionary<string, BuildingTemplate> BuildingTemplates { get; private set; }

	public LobbyView()
	{
		Players = new LobbyPlayerCollection();
		Gameplay = new GameplayView();
	}

	public void SetupDependancies(IExplorer gameData)
	{
		GameData = gameData;
		var loaded = LoadAll<BuildingTemplate>(GameData.Tags["type-building"]);

		BuildingTemplates = loaded.ToDictionary(template => template.Identifier);
	}

	public static T[] LoadAll<T>(IResourceCollection resources)
		where T : IResourceModel
	{
		var deserializedResources = new T[resources.Count];

		int index = 0;
		foreach (var resource in resources)
		{
			deserializedResources[index] = Load<T>(resource);
			index++;
		}
		return deserializedResources;
	}

	public static T Load<T>(IResource resource)
		where T : IResourceModel
	{
		var serializer = new JsonSerializer();

		using var data = resource.Content.OpenRead();
		using var sr = new StreamReader(data);
		using var reader = new JsonTextReader(sr);

		var model = serializer.Deserialize<T>(reader);
		model.Identifier = resource.FullName;

		return model;
	}

	public void Apply(LobbyViewProcedure action)
	{
		action.Apply(this);
	}
}
