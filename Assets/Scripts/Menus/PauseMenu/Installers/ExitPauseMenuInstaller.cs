using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ExitPauseMenuInstaller : MonoInstaller
{
    [SerializeField] private Button _exitButton;    

    public override void InstallBindings()
    {
        InstallLogic();
        InstallElements();
    }

    public void InstallLogic()
    {
        Container.BindInterfacesAndSelfTo<ExitPauseMenuManager>().AsSingle();
    }
    public void InstallElements()
    {
        Container.BindInstance(_exitButton).WithId(Identifiers.PauseExitButton).AsCached();
    }
}
