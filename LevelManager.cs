using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Настройки")]
    public AudioClip levelMusicClip;

    [Header("Зависимости")]
    [SerializeField] private NoteSpawner noteSpawner;

    [Header("UI")]
    public GameObject waitingUI;
    public GameObject gameUI;

    // Свойство для получения AudioSource
    private AudioSource _levelMusicSource;

    public bool IsPlaying { get; private set; } = false;

    // Безопасное получение времени музыки
    public float MusicTime => (_levelMusicSource != null) ? _levelMusicSource.time : 0f;
    public float MusicDuration => (levelMusicClip != null) ? levelMusicClip.length : 0f;

    private void Awake()
    {
        // Паттерн Синглтон
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Инициализация AudioSource
        _levelMusicSource = GetComponent<AudioSource>();
        _levelMusicSource.clip = levelMusicClip;
        _levelMusicSource.loop = false;
        _levelMusicSource.playOnAwake = false;
    }

    private void Start()
    {
        Debug.Log("[LevelManager] Попытка запуска уровня...");

    // 1. Проверяем, не запущено ли уже
    if (IsPlaying) 
    {
        Debug.Log("[LevelManager] Уровень уже запущен!"); 
        return;
    }
    
    // 2. Только теперь ставим флаг
    IsPlaying = true;

    if (AudioManager.Instance != null)
    {
        AudioManager.Instance.StopMusic();
        Debug.Log("[LevelManager] Музыка меню остановлена через AudioManager");
    }
    else
    {
        Debug.LogWarning("[LevelManager] AudioManager.Instance не найден! Музыка меню не остановлена.");
    }

    // 3. UI
    if (waitingUI != null) waitingUI.SetActive(false);
    if (gameUI != null) gameUI.SetActive(true);

    // 4. Музыка
    if (_levelMusicSource != null)
        _levelMusicSource.Play();

    // 5. Спавн
    if (noteSpawner != null)
        noteSpawner.StartSpawning();

    Debug.Log("[LevelManager] Уровень начат!");

    }

    public void StartLevel()
    {
        Debug.Log("[LevelManager] Попытка запуска уровня..."); // <--- ДОБАВЬ ЭТО

    if (IsPlaying) 
    {
        Debug.Log("[LevelManager] Уровень уже запущен!"); 
        return;
    }
    
    IsPlaying = true;
        if (IsPlaying) return;
        IsPlaying = true;

        if (waitingUI != null) waitingUI.SetActive(false);
        if (gameUI != null) gameUI.SetActive(true);

        _levelMusicSource.Play();

        if (noteSpawner != null)
            noteSpawner.StartSpawning();

        Debug.Log("[LevelManager] Уровень начат!");
    }
 
}
