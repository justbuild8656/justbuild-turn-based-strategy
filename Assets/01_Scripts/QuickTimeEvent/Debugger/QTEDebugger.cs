using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QTEDebugger : MonoBehaviour
{
    public TMP_Text judgmentText;
    public TMP_Text reactionTimeText;
    
    public KeyCode reactionKey = KeyCode.A;
    public float qteDuration = 1f;

    private QuickTimeEvent _qte;

    public void TestQTE()
    {
        _qte = new QuickTimeEvent(reactionKey, qteDuration);
        _qte.AddListener(EndEvent);
        _qte.Start(this);
    }

    private void Update()
    {
        if (_qte == null) return;
        reactionTimeText.text = $"Reaction Time: {(_qte.JudgeTime - _qte.QTETimer) * 1000} (ms)";
    }

    private void EndEvent(QTEResult result)
    {
        reactionTimeText.text = $"Reaction Time: {result.ReactionTime} (ms)";
        judgmentText.text = $"Judgment: {result.Judgment}";
    }
}
