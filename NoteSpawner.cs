using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [System.Serializable]
    public class NoteData
    {
        public float time;          // Секунда музыки, когда нота должна ПОПАСТЬ на кнопку
        [Range(0, 3)]
        public int   laneIndex;     // 0=Left 1=Down 2=Up 3=Right
    }
 
    [Header("Спавн")]
    public GameObject notePrefab;           // Назначь префаб Note
    public Transform[] spawnPoints;         // 4 точки сверху (по одной на дорожку)
    public LaneButton[] laneButtons;        // 4 кнопки внизу
 
    [Header("Физика нот")]
    public float fallDuration = 1.5f;       // Сколько секунд нота летит (подбери под экран)
    public float missY        = -6f;        // Y ниже которого нота = Miss
 
    [Header("Битмап")]
    public List<NoteData> beatMap = new();
 
    [Header("Зависимости")]
    public LevelManager levelManager;
 
    // ─────────────────────────────────────────────────────
    private bool      _active;
    private float     _noteSpeed;    // единиц/сек — рассчитывается из fallDuration
    private Coroutine _spawnRoutine;
 
    private void Awake()
    {
        if (beatMap.Count == 0)
            beatMap = BuildDefaultBeatMap();
    }
 
    public void StartSpawning()
    {
Debug.Log("[NoteSpawner] Метод StartSpawning вызван!");

        // Расстояние между SpawnPoint и LaneButton по Y
        if (spawnPoints != null && spawnPoints.Length > 0 &&
            laneButtons  != null && laneButtons.Length  > 0)
        {
            float dist = spawnPoints[0].position.y - laneButtons[0].transform.position.y;
            _noteSpeed = dist / fallDuration;
        }
        else
        {
            _noteSpeed = 8f; // Запасное значение
        }
 
        _active       = true;
        _spawnRoutine = StartCoroutine(SpawnRoutine());
    }
 
    public void StopSpawning()
    {
        _active = false;
        if (_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);
    }
 
    // ─────────────────────────────────────────────────────
    private IEnumerator SpawnRoutine()
    {
        Debug.Log("[NoteSpawner] Coroutine запущена!"); // Добавь это
    int index = 0;

    while (_active && index < beatMap.Count)
    {
 
        while (_active && index < beatMap.Count)
        {
            NoteData note      = beatMap[index];
            float    spawnTime = note.time - fallDuration;    // Когда нужно заспавнить
            float    musicTime = levelManager != null ? levelManager.MusicTime : 0f;
 
            // Ждём момента спавна
            float wait = spawnTime - musicTime;
            if (wait > 0f)
                yield return new WaitForSeconds(wait);
 
            if (_active)
                SpawnNote(note);
 
            index++;
        }
    }
    }
 
    private void SpawnNote(NoteData data)
    {
        if (notePrefab == null) return;
        if (data.laneIndex >= spawnPoints.Length || data.laneIndex >= laneButtons.Length) return;
 
        Transform  sp   = spawnPoints[data.laneIndex];
        LaneButton btn  = laneButtons[data.laneIndex];
 
        GameObject obj  = Instantiate(notePrefab, sp.position, Quaternion.identity);
        NoteObject note = obj.GetComponent<NoteObject>();
 
        note.Initialize(data.laneIndex, _noteSpeed, missY, btn);
    }
 
    // ─────────────────────────────────────────────────────
    // Битмап под music.mp3 (~13 сек).
    // Замени тайминги на реальные когда будешь слушать трек.
    // ─────────────────────────────────────────────────────
    private static List<NoteData> BuildDefaultBeatMap()
    {
        return new List<NoteData>
        {
            new NoteData { time = 1.0f,  laneIndex = 0 },
            new NoteData { time = 1.5f,  laneIndex = 2 },
            new NoteData { time = 2.0f,  laneIndex = 1 },
            new NoteData { time = 2.5f,  laneIndex = 3 },
            new NoteData { time = 3.0f,  laneIndex = 0 },
            new NoteData { time = 3.5f,  laneIndex = 2 },
            new NoteData { time = 4.0f,  laneIndex = 1 },
            new NoteData { time = 4.25f, laneIndex = 3 },
            new NoteData { time = 4.5f,  laneIndex = 0 },
            new NoteData { time = 5.0f,  laneIndex = 2 },
            new NoteData { time = 5.5f,  laneIndex = 1 },
            new NoteData { time = 6.0f,  laneIndex = 3 },
            new NoteData { time = 6.5f,  laneIndex = 0 },
            new NoteData { time = 6.75f, laneIndex = 1 },
            new NoteData { time = 7.0f,  laneIndex = 2 },
            new NoteData { time = 7.5f,  laneIndex = 3 },
            new NoteData { time = 8.0f,  laneIndex = 0 },
            new NoteData { time = 8.5f,  laneIndex = 2 },
            new NoteData { time = 9.0f,  laneIndex = 1 },
            new NoteData { time = 9.25f, laneIndex = 3 },
            new NoteData { time = 9.5f,  laneIndex = 0 },
            new NoteData { time = 9.75f, laneIndex = 2 },
            new NoteData { time = 10.0f, laneIndex = 1 },
            new NoteData { time = 10.5f, laneIndex = 3 },
            new NoteData { time = 11.0f, laneIndex = 0 },
            new NoteData { time = 11.5f, laneIndex = 2 },
            new NoteData { time = 12.0f, laneIndex = 1 },
            new NoteData { time = 12.5f, laneIndex = 3 },
        };
    }
}
