using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnter;

    private void OnTriggerEnter(Collider other)
    {
        onEnter.Invoke();
    }
}
