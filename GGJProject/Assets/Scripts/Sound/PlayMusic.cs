using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour {

    [Header("Music Info")]
    public string MusicName;

    public float Volume = 1f;
    public float Pitch = 1f;

    public bool DestroyAfter;

    private void Start()
    {
        if (!SoundManager.Instance.GetActualMusic().Equals(MusicName))
        {
            SoundManager.Instance.PlayMusic(MusicName);
        }

        if (SoundManager.Instance.MusicSource.volume != Volume)
        {
            SoundManager.Instance.MusicSource.volume = Volume;
        }

        //if (SoundManager.Instance.MusicSource.dopplerLevel != DopplerLevel)
        //{
        //    SoundManager.Instance.MusicSource.dopplerLevel = DopplerLevel;
        //}

        if (SoundManager.Instance.MusicSource.pitch != Pitch)
        {
            SoundManager.Instance.MusicSource.pitch = Pitch;
        }

        if (DestroyAfter)
        {
            Destroy(this);
        }
    }
}
