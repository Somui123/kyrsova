using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FeedbackText : MonoBehaviour
{
    public float speed = 2f;
    public float lifetime = 0.5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }
}
