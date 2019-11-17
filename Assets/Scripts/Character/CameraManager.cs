using Cinemachine;
using UnityEngine;

public class CameraManager
{
    private Camera _camera;
    private CinemachineVirtualCamera _cinemachineCamera;
    private CharacterSpawner _characterSpawner;

    public CameraManager(
        Camera camera,
        CinemachineVirtualCamera cinemachineCamera,
        CharacterSpawner characterSpawner)
    {
        _camera = camera;
        _cinemachineCamera = cinemachineCamera;
        _characterSpawner = characterSpawner;
    }

    public void SetCameraToPlayerCharacter(CharacterFacade playerCharacterFacade)
    {
        _cinemachineCamera.Follow = playerCharacterFacade.transform;
    }


}