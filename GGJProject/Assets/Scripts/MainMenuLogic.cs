using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{
    public void CallGame()
    {
        StaticData.Instance.CallLoadingScene("GameScreen");
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}