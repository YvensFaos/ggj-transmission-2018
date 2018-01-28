using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Essa classe é responsável por manter todas as instâncias e valores fixos e compartilhados entre as diferentes classes do projeto.
/// Ela deve ser acessada através do Instance.
/// </summary>
public class StaticData : MonoBehaviour
{
    private static StaticData instance;

    public static StaticData Instance
    {
        get {
            if(instance == null)
            {
                GameObject gameObject = FindObjectOfType<GameObject>();
                if(gameObject != null)
                {
                    instance = gameObject.AddComponent<StaticData>();
                }
            }
            return instance;
        }
    }

    /// <summary>
    /// Liga e desliga as configurações de DEBUG do projeto, como logs e visualizações.
    /// </summary>
    public bool DEBUG;

    public GameGraph gameGraph;
    public CoreLogic coreLogic;

    /// <summary>
    /// Determina qual será a próxima cena a ser chamada pela LoadingScene
    /// </summary>
    public string NextScene;

    private void Awake()
    {
        Debug.Log("Eu!");
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (DEBUG)
        {
            Random.InitState(666);
        }
    }

    /// <summary>
    /// Método para invocar a tela de carregamento de qualquer ponto do projeto.
    /// </summary>
    /// <param name="NextScene">Nova cena a ser chamada.</param>
    public void CallLoadingScene(string NextScene)
    {
        this.NextScene = NextScene;
        LoadingScript.StaticNextScene = NextScene;

        Log("Call " + NextScene);
        SceneManager.LoadScene("LoadingScreen");
    }

    /// <summary>
    /// Método para printar uma frase no console da Unity.
    /// Depende da variável DEBUG.
    /// </summary>
    /// <param name="debugPhrase"></param>
    public void Log(string debugPhrase)
    {
        if(Instance.DEBUG)
        {
            Debug.Log(debugPhrase);
        }
    }
}