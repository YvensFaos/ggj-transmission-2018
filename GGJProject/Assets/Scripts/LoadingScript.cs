using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScript : MonoBehaviour
{
    public static string StaticNextScene;

    public void Start()
    {
        //VSSoundManager.Instance.NormalizeSources();
        if (StaticNextScene == null || StaticNextScene.Equals(""))
        {
            StaticNextScene = "MainMenu";
        }
        StartCoroutine("WaitTimer");
    }

    /// <summary>
    /// Método básico de espera, para que o Loading não aconteça rápido demais.
    /// </summary>
    /// <returns>Espera por 1.0 segundos.</returns>
    private IEnumerator WaitTimer()
    {
        yield return new WaitForSeconds(1.0f);
        StartCoroutine("LoadMethod");
    }

    /// <summary>
    /// Método para carregar a nova cena de forma assíncrona.
    /// A cena a ser carregada deve ser indicada no atributo NextScene ou setada no VSStaticData
    /// </summary>
    /// <returns>Espera carregar a nova cena.</returns>
    private IEnumerator LoadMethod()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(StaticNextScene, LoadSceneMode.Single);
        yield return async;
    }
}