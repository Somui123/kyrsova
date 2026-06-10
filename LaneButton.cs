using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaneButton : MonoBehaviour
{
     [Header("Настройки дорожки")]
    public int   laneIndex;          // 0=Left 1=Down 2=Up 3=Right
    public float hitWindowY = 0.8f;  // ±0.8 единиц вокруг кнопки = окно попадания
 
    [Header("Визуал кнопки")]
    public SpriteRenderer buttonSprite;   // Спрайт кнопки
    public Color idleColor    = new Color(0.25f, 0.25f, 0.35f, 1f);
    public Color pressedColor = new Color(1f, 0.92f, 0.2f, 1f);
    public Color hitColor     = new Color(0.2f, 1f, 0.4f, 1f);
    public float flashTime    = 0.12f;
 
    [Header("Звук ноты")]
    public AudioSource noteAudioSource;  // AudioSource на этом же объекте
    public AudioClip   noteHitSound;     // Назначь звук ноты

    [Header("Feedback Effects")]
public GameObject particlePrefab; // Перетащи сюда HitParticle
public GameObject textPrefab;     // Перетащи сюда HitTextPrefab
 
    // ─────────────────────────────────────────────────────
    private readonly List<NoteObject> _notesInRange = new();
    private Coroutine                 _flashRoutine;
 
    // ─────────────────────────────────────────────────────
    private void Awake()
    {
        if (buttonSprite != null)
            buttonSprite.color = idleColor;
 
        if (noteAudioSource == null)
            noteAudioSource = GetComponent<AudioSource>();
    }
 
    // ─────────────────────────────────────────────────────
    // Вызывается из InputHandler при нажатии клавиши
    // ─────────────────────────────────────────────────────
    public void OnPressed()
    {
        FlashButton(pressedColor);
 
        NoteObject bestNote = GetBestNote();
        if (bestNote != null)
        {
            _notesInRange.Remove(bestNote);
            bestNote.OnHit();
            PlayNoteSound();
            FlashButton(hitColor);
            ScoreManager.Instance?.RegisterHit();
            PlayHitEffect();
        }
        else
        {
            // Нажали мимо — пустое нажатие (не штрафуем, просто нет очков)
        }
    }
 
    // Вызывается из NoteObject когда нота вышла за экран
    public void OnNoteMissed(NoteObject note)
    {
        _notesInRange.Remove(note);
        ScoreManager.Instance?.RegisterMiss();
    }
 
    // ─────────────────────────────────────────────────────
    // NoteObject регистрирует себя, когда входит в окно
    // ─────────────────────────────────────────────────────
    public void RegisterNote(NoteObject note)
    {
        if (!_notesInRange.Contains(note))
            _notesInRange.Add(note);
    }
 
    public void UnregisterNote(NoteObject note)
    {
        _notesInRange.Remove(note);
    }
 
    // ─────────────────────────────────────────────────────
    private NoteObject GetBestNote()
    {
        _notesInRange.RemoveAll(n => n == null);
 
        NoteObject best      = null;
        float      bestDist  = float.MaxValue;
 
        foreach (var note in _notesInRange)
        {
            float dist = Mathf.Abs(note.transform.position.y - transform.position.y);
            if (dist < bestDist)
            {
                bestDist = dist;
                best     = note;
            }
        }
        return best;
    }
 
    private void PlayNoteSound()
    {
        if (noteAudioSource != null && noteHitSound != null)
            noteAudioSource.PlayOneShot(noteHitSound);
    }
 
    private void FlashButton(Color color)
    {
        if (_flashRoutine != null) StopCoroutine(_flashRoutine);
        _flashRoutine = StartCoroutine(FlashRoutine(color));
    }
 
    private IEnumerator FlashRoutine(Color flashCol)
    {
        if (buttonSprite != null) buttonSprite.color = flashCol;
        yield return new WaitForSeconds(flashTime);
        if (buttonSprite != null) buttonSprite.color = idleColor;
    }

    public void PlayHitEffect()
{
    if (particlePrefab != null)
    {
        Instantiate(particlePrefab, transform.position, Quaternion.identity);
    }
    
    if (textPrefab != null)
    {
        // Создаем текст чуть выше кнопки
        Vector3 spawnPos = transform.position + new Vector3(0, 0.5f, 0);
        GameObject textObj = Instantiate(textPrefab, spawnPos, Quaternion.identity);
        
        // Если хочешь менять текст динамически (например, Perfect/Good/Miss):
        // textObj.GetComponent<TextMeshProUGUI>().text = "Good job!"; 
    }
}
}
