using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashLogic : MonoBehaviour {

    private void Start()
    {
        CallLoadingScreen();
    }

    /// <summary>
    /// Esse método é chamado pelo evento ao final da animação da SplashScreen
    /// </summary>
    public void CallLoadingScreen()
    {
        SceneManager.LoadScene("LoadingScreen");
    }
}
