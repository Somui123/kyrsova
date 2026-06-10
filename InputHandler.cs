using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header("Кнопки дорожек")]
    public LaneButton laneLeft;     // ← LeftArrow
    public LaneButton laneDown;     // ↓ DownArrow
    public LaneButton laneUp;       // ↑ UpArrow
    public LaneButton laneRight;    // → RightArrow
 
    private bool _levelStarted = false;
 
    private void Update()
    {
        bool leftPressed  = Input.GetKeyDown(KeyCode.LeftArrow);
        bool downPressed  = Input.GetKeyDown(KeyCode.DownArrow);
        bool upPressed    = Input.GetKeyDown(KeyCode.UpArrow);
        bool rightPressed = Input.GetKeyDown(KeyCode.RightArrow);
 
        bool anyPressed = leftPressed || downPressed || upPressed || rightPressed;
 
        // Первое нажатие — стартуем
        if (!_levelStarted && anyPressed)
        {
            _levelStarted = true;
            LevelManager.Instance?.StartLevel();
        }
 
        // Передаём нажатия кнопкам
        if (_levelStarted)
        {
            if (leftPressed  && laneLeft  != null) laneLeft.OnPressed();
            if (downPressed  && laneDown  != null) laneDown.OnPressed();
            if (upPressed    && laneUp    != null) laneUp.OnPressed();
            if (rightPressed && laneRight != null) laneRight.OnPressed();
        }
    }
}
