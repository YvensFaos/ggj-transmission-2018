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
                instance = FindObjectOfType<GameObject>().AddComponent<StaticData>();
            }
            return instance;
        }
    }

    /// <summary>
    /// Liga e desliga as configurações de DEBUG do projeto, como logs e visualizações.
    /// </summary>
    public bool DEBUG;

    /// <summary>
    /// Guarda a referência para o mecanismo de interação do player. Esse mecanismo é acionado pelos scripts dos objetos interativos.
    /// </summary>
    //public BaseInteractScript PlayerInteract;

    /// <summary>
    /// Guarda a referência para o GameObject do Player.
    /// </summary>
    public GameObject PlayerRef;

    public Material debugMaterial;

    /// <summary>
    /// Determina qual será a próxima cena a ser chamada pela LoadingScene
    /// </summary>
    public string NextScene;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //PlayerRef = GameObject.FindGameObjectsWithTag("Player")[1];
        //PlayerInteract = PlayerRef.GetComponentInChildren<BaseInteractScript>();
        //TODO
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// Método para invocar a tela de carregamento de qualquer ponto do projeto.
    /// </summary>
    /// <param name="NextScene">Nova cena a ser chamada.</param>
    public void CallLoadingScene(string NextScene)
    {
        this.NextScene = NextScene;
        SceneManager.LoadScene("LoadingScreen");
    }
}