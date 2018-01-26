using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashLogic : MonoBehaviour {

    /// <summary>
    /// Esse método é chamado pelo evento ao final da animação da SplashScreen
    /// </summary>
    public void CallLoadingScreen()
    {
        SceneManager.LoadScene("LoadingScene");
    }
}
