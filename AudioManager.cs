using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public static AudioManager Instance;
    
    [Header("Audio Sources")]
    public AudioSource musicSource; // Сюда перетащим AudioSource для музыки
    
    void Awake()
    {
        // Паттерн Singleton: проверяем, нет ли уже такого менеджера
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Если дубликат — удаляем
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return; // Не перезапускаем, если играет то же самое
        musicSource.clip = clip;
        musicSource.Play();
    }

     public void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
            musicSource.Stop();
    }
}
