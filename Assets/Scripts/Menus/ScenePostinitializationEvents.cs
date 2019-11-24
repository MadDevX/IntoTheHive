
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

/// <summary>
/// This class holds the events that are fired when a corresponding scene is initialized.
/// </summary>
public class ScenePostinitializationEvents: IInitializable
{
    private Scenes _scenes;
    private Dictionary<int, List<Action>> _scenePostInitEvents;
        
    public ScenePostinitializationEvents(Scenes scenes)
    {
        _scenes = scenes;
        _scenePostInitEvents = new Dictionary<int, List<Action>>();
    }

    public void Initialize()
    {
        InitializeAllScenesLists();
    }

    /// <summary>
    /// Subscribe an Action which will be executed after given scene's initialization.
    /// </summary>
    /// <param name="sceneBuildIndex">Build index of the scene which initialization will trigger the action.</param>
    /// <param name="postInitAction">Action which will be called after initialization.</param>
    public void Subscribe(int sceneBuildIndex, Action postInitAction)
    {
        List<Action> list;
        _scenePostInitEvents.TryGetValue(sceneBuildIndex, out list);
        list.Add(postInitAction);
    }

    /// <summary>
    /// Unsubscribe an Action which is currently executed after given scene's initialization.
    /// </summary>
    /// <param name="sceneBuildIndex">Build index of the scene which initialization triggers the action.</param>
    /// <param name="postInitAction">Action which is called after initialization.</param>
    public void Unsubscribe(int sceneBuildIndex, Action postInitAction)
    {
        List<Action> list;
        _scenePostInitEvents.TryGetValue(sceneBuildIndex, out list);
        list.Remove(postInitAction);
    }

    /// <summary>
    /// Initializes a list for each scene located in 'Scenes' class.
    /// </summary>
    private void InitializeAllScenesLists()
    {
        Type ScenesType = typeof(Scenes);
        var fieldsInfo = ScenesType.GetFields().Where( Type => Type.FieldType == typeof(SceneReference));

        // Use reflection to initialize all sceneEventLists in a generic way.
        foreach(FieldInfo field in fieldsInfo)
        {
            SceneReference sceneReference = field.GetValue(_scenes) as SceneReference;
            var sceneBuildIndex = SceneUtility.GetBuildIndexByScenePath(sceneReference.ScenePath);
            // Do not add lists for nonexisting scenes.
            if(sceneBuildIndex >= 0)
                _scenePostInitEvents.Add(sceneBuildIndex, new List<Action>());
        }
    }

    /// <summary>
    /// Invokes all events corresponding to the chosen scene initialization.
    /// </summary>
    /// <param name="sceneBuildIndex">Build index of the scene initialized.</param>
    public void InvokeEvents(int sceneBuildIndex)
    {

        //TODO MG : should this clear all events after invoking?
        //TODO MG : add a boolean isOneTimeOnly
        var events = _scenePostInitEvents[sceneBuildIndex];
        events?.ForEach(e => e?.Invoke());
    }

}

