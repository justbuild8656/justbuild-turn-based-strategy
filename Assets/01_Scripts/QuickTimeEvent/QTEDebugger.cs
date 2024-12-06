using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QTEDebugger : MonoBehaviour
{
    public TMP_Text judgmentText;
    public TMP_Text reactionTimeText;
    
    public KeyCode reactionKey = KeyCode.A;
    public float qteTime = 3f;
    public float successMs = 80f;
    public float greatSuccessMs = 40f;

    private QuickTimeEvent _qte;

    public void TestQTE()
    {
        _qte = new QuickTimeEvent(new QTEData(reactionKey, qteTime, successMs / 1000, greatSuccessMs / 1000));
        _qte.OnTrigger.AddListener(EndEvent);
        _qte.Start();
    }

    private void Update()
    {
        if (_qte == null) return;
        reactionTimeText.text = $"Reaction Time: {_qte.CheckMs * 1000} (ms)";
    }

    private void EndEvent(QTEResult result)
    {
        judgmentText.text = $"Judgment: {result}";
    }
}
