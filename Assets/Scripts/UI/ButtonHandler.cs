using UnityEngine;
public class ButtonHandler : MonoBehaviour
{
    public void OnStartGameButton()
    {
        MySceneManager.Instance.LoadNextScene();
    }
    //게임 재개
    public void OnResum()
    {
        PauseManager.Instance.Resume();
    }

    public void OnSettingsButton()
    {
        PauseManager.Instance.OpenSettings();
    }

    public void OnQuitButton()
    {
        MySceneManager.Instance.QuitGame();
    }

    public void TitleLoad()
    {
        MySceneManager.Instance.LoadNextScene();
        Time.timeScale = 1f;   
    }
}
