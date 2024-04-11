using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    #region CLIPS
    public AudioClip BranchBreak;
    public AudioClip Button;
    public AudioClip Coin;
    public AudioClip Explosion;
    public AudioClip Heart;
    public AudioClip Highscore;
    public AudioClip Jump;
    public AudioClip LevelUp;
    public AudioClip PlayerDeath;
    public AudioClip ShieldDamage;
    public AudioClip ShieldUp;
    public AudioClip Wind;
    #endregion

    private GameObject singleSoundGameObject;
    private int NUMBER_OF_SOUND_SOURCES = 25;
    private int lastSourceIndex = 0;
    private List<AudioSource> audioSourcesPool;

    private void Start()
    {
        singleSoundGameObject = new GameObject("SingleSound");


        foreach (var item in FindObjectsOfType<Button>(true))
        {
            if (item.gameObject.tag == TagsDictionary.BUTTON)
            {
                item.GetComponent<Button>().onClick.AddListener(delegate { PlaySingleSound(Button); });
            }
        }
        audioSourcesPool = new List<AudioSource>();

        //fill pool with AudioSources
        for(int i = 0; i < NUMBER_OF_SOUND_SOURCES; i++)
        {
            var source = singleSoundGameObject.AddComponent<AudioSource>();
            audioSourcesPool.Add(source);
        }
        Unmute();
    }

    public void PlaySingleSound(AudioClip sound)
    {
        AudioSource audioSource = audioSourcesPool[GetPoolIndex()];
        if(audioSource == null || sound == null)
        {
            return;
        }
        audioSource.PlayOneShot(sound);
    }

    public void Mute()
    {
        AudioListener.volume = 0;
    }

    public void Unmute()
    {
        AudioListener.volume = Helper.GAME_DEFAULT_VOLUME;
    }

    public void PlaySingleSoundAfterDelay(AudioClip sound, float delayInSecs)
    {
        AudioSource audioSource = audioSourcesPool[GetPoolIndex()];
        audioSource.clip = sound;
        audioSource.PlayDelayed(delayInSecs);
    }

    public void PlayLoopedSound(AudioClip sound)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = sound;
        audioSource.Play();
        audioSource.loop = true;
    }

    private int GetPoolIndex()
    {
        lastSourceIndex++;
        if (lastSourceIndex >= NUMBER_OF_SOUND_SOURCES)
        {
            lastSourceIndex = 0;
        }
        return lastSourceIndex;
    }
}
