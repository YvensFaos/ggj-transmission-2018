using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    #region Singleton

    public static SoundManager Instance {
        get;
        private set;
    }

    /// <summary>
    /// Cria uma versão estática e globalmente acessável de um VSSoundManager.
    /// Caso já exista um valor para o singleton, o novo valor é deletado e apenas o valor antigo é mantido.
    /// </summary>
    /// <param name="instance">Instância do VSSoundManager.</param>
    /// <returns>Se foi criado o primeiro e único singleton. Caso já exista um singleton regitrado, o retorno é falso.</returns>
    private static bool Singleton(SoundManager instance)
    {
        if (Instance)
        {
            Destroy(instance);
            return false;
        }
        else
        {
            Instance = instance;
            return true;
        }
    }

    #endregion Singleton

    [Header("Audio Lists")]
    public AudioClip[] SFXList;

    public AudioClip[] MusicList;

    public AudioSource LongSFXSource;
    public AudioSource SFXSource;
    public AudioSource MusicSource;

    private Dictionary<string, AudioClip> sfxHash;
    private Dictionary<string, AudioClip> musicHash;

    [Header("Volume Variables")]
    [Range(0, 1)]
    public float gameVolume = 1.0f;

    private void Awake()
    {
        Singleton(this);

        sfxHash = new Dictionary<string, AudioClip>();
        StaticData.Instance.Log(SFXList + " " + SFXList.Length);
        if (SFXList != null && SFXList.Length != 0)
        {
            foreach (AudioClip source in SFXList)
            {
                StaticData.Instance.Log("Adicionando sfx: " + source.name);
                sfxHash.Add(source.name, source);
            }
        }
        musicHash = new Dictionary<string, AudioClip>();
        if (MusicList != null && MusicList.Length != 0)
        {
            foreach (AudioClip source in MusicList)
            {
                StaticData.Instance.Log("Adicionando música: " + source.name);
                musicHash.Add(source.name, source);
            }
        }
    }

    public void PlaySFXAudio(string identifier)
    {
        StaticData.Instance.Log("Play SFX - " + identifier);
        if (sfxHash[identifier] != null)
        {
            SFXSource.clip = sfxHash[identifier];
            SFXSource.Play();
        }
    }

    /// <summary>
    /// Toca um sfx longo em um soundsource a parte, para não ser sobrescrito por outros sfx.
    /// Usado principalmente para os soundfx de onibus e de metro.
    /// </summary>
    /// <param name="identifier"></param>
    public void PlayLongSFXAudio(string identifier)
    {
        StaticData.Instance.Log("Play SFX - " + identifier);
        if (sfxHash[identifier] != null)
        {
            LongSFXSource.clip = sfxHash[identifier];
            LongSFXSource.Play();
        }
    }

    public void PlayMusic(string identifier, bool loop = true)
    {
        StaticData.Instance.Log("Play Music - " + identifier);
        if (musicHash[identifier] != null)
        {
            MusicSource.clip = musicHash[identifier];
            MusicSource.Play();
            MusicSource.loop = loop;
        }
    }

    public void PauseMusic()
    {
        MusicSource.Pause();
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }

    /// <summary>
    /// Retorna o nome da música tocando atualmente pelo MusicSource.
    /// </summary>
    /// <returns>O nome da música tocando atualmente pelo MusicSource.</returns>
    public string GetActualMusic()
    {
        if (MusicSource.clip != null)
        {
            return MusicSource.clip.name;
        }

        return "";
    }

    /// <summary>
    /// Normaliza os parâmetros do VSSoundManager para os valores default:
    /// Pitch = 1.0f;
    /// Volume = 1.0f;
    /// Mute = false;
    /// </summary>
    public void NormalizeSources()
    {
        SFXSource.pitch = 1.0f;
        SFXSource.volume = 1.0f;
        SFXSource.mute = false;

        MusicSource.pitch = 1.0f;
        MusicSource.volume = 1.0f;
        MusicSource.mute = false;
    }
}
