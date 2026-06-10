using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
 public int LaneIndex { get; private set; }
 
    private float      _speed;
    private float      _missY;
    private float      _hitWindowY;
    private bool       _isDead;
    private bool       _inRange;
    private LaneButton _laneButton;
 
    public void Initialize(int laneIndex, float speed, float missY, LaneButton laneButton)
    {
        LaneIndex   = laneIndex;
        _speed      = speed;
        _missY      = missY;
        _laneButton = laneButton;
        _hitWindowY = laneButton != null ? laneButton.hitWindowY : 0.8f;
    }
 
    private void Update()
    {
        if (_isDead) return;
 
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
 
        float distToButton = _laneButton != null
            ? transform.position.y - _laneButton.transform.position.y
            : float.MaxValue;
 
        // Входим в окно попадания
        if (!_inRange && Mathf.Abs(distToButton) < _hitWindowY)
        {
            _inRange = true;
            _laneButton?.RegisterNote(this);
        }
        // Выходим из окна снизу
        else if (_inRange && distToButton < -_hitWindowY)
        {
            _inRange = false;
            _laneButton?.UnregisterNote(this);
        }
 
        // Вышла за экран — Miss
        if (transform.position.y < _missY)
        {
            _laneButton?.OnNoteMissed(this);
            Die();
        }
    }
 
    public void OnHit()
    {
        if (_inRange) _laneButton?.UnregisterNote(this);
        Die();
    }
 
    private void Die()
    {
        _isDead = true;
        Destroy(gameObject);
    }
}
