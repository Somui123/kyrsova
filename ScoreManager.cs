using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
 
    [Header("UI (опционально)")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
 
    [Header("Очки")]
    public int scorePerHit  = 100;
    public int comboBonus   = 10;   // Доп. очки за каждые 10 комбо
 
    public int Score    { get; private set; }
    public int Combo    { get; private set; }
    public int MaxCombo { get; private set; }
    public int Hits     { get; private set; }
    public int Misses   { get; private set; }
 
    private void Awake()
    {
        Instance = this;
    }
 
    public void RegisterHit()
    {
        Combo++;
        Hits++;
        if (Combo > MaxCombo) MaxCombo = Combo;
 
        int bonus = (Combo / 10) * comboBonus;
        Score += scorePerHit + bonus;
 
        UpdateUI();
    }
 
    public void RegisterMiss()
    {
        Combo = 0;
        Misses++;
        UpdateUI();
    }
 
    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = Score.ToString("N0");
 
        if (comboText != null)
        {
            comboText.gameObject.SetActive(Combo > 1);
            comboText.text = $"x{Combo}";
        }
    }
}
