using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEManager : MonoBehaviour
{
    public static QTEManager Instance { get; private set; }
    
    private readonly List<QuickTimeEvent> _quickTimeEvents = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_quickTimeEvents.Count == 0) return;

        for (var index = 0; index < _quickTimeEvents.Count; index++)
        {
            var qte = _quickTimeEvents[index];
            if (qte.IsCompleted)
            {
                _quickTimeEvents.RemoveAt(index);
            }
        }
    }

    public void StartQTE(QuickTimeEvent qte)
    {
        StartCoroutine(qte.EventCoroutine());
        _quickTimeEvents.Add(qte);
    }
}
