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
public class ScenePostinitializationEvents
{
    private Scenes _scenes;
    private Dictionary<int, List<PostInitActionArgument>> _scenePostInitEvents;
        
    public ScenePostinitializationEvents(Scenes scenes)
    {
        _scenes = scenes;
        _scenePostInitEvents = new Dictionary<int, List<PostInitActionArgument>>();
        InitializeAllScenesLists();
    }

    /// <summary>
    /// Subscribe an Action which will be executed after given scene's initialization.
    /// </summary>
    /// <param name="sceneBuildIndex">Build index of the scene which initialization will trigger the action.</param>
    /// <param name="postInitAction">Action which will be called after initialization.</param>
    /// <param name="oneTimeOnly">States if the action will be called only one when the scene next loads or every time.</param>
    public void Subscribe(int sceneBuildIndex, Action postInitAction, bool oneTimeOnly = true)
    {
        Debug.Log("subscribe post init events");
        List<PostInitActionArgument> list;
        _scenePostInitEvents.TryGetValue(sceneBuildIndex, out list);        
        list.Add(new PostInitActionArgument(postInitAction, oneTimeOnly));
    }

    /// <summary>
    /// Unsubscribe an Action which is currently executed after given scene's initialization.
    /// </summary>
    /// <param name="sceneBuildIndex">Build index of the scene which initialization triggers the action.</param>
    /// <param name="postInitAction">Action which is called after initialization.</param>
    public void Unsubscribe(int sceneBuildIndex, Action postInitAction)
    {
        List<PostInitActionArgument> list;
        _scenePostInitEvents.TryGetValue(sceneBuildIndex, out list);
        list.RemoveAll(action => action.Action == postInitAction);
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
                _scenePostInitEvents.Add(sceneBuildIndex, new List<PostInitActionArgument>());
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
        Debug.Log("Invoke post init events");
        var events = _scenePostInitEvents[sceneBuildIndex];
        events?.ForEach(e => e?.Action.Invoke());
        events.RemoveAll(e => e.OneTimeOnly == true);
    }

    /// <summary>
    /// class which is purely internal to hold the information if action is one time only in a simply fashion
    /// </summary>
    private class PostInitActionArgument
    {
        public Action Action { get; set; }
        public bool OneTimeOnly { get; }

        public PostInitActionArgument(Action action, bool oneTimeOnly = true)
        {
            Action = action;
            OneTimeOnly = oneTimeOnly;
        }
    }
}



