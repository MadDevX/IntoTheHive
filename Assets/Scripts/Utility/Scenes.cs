using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameResources/Scenes")]
public class Scenes : ScriptableObject
{
    public SceneReference Menu;
    public SceneReference ConnectionScreen;
    public SceneReference LobbyClient;
    public SceneReference LobbyHost;
    public SceneReference Level;
    public SceneReference Hub;
}
