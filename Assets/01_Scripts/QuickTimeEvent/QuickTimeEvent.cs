using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public enum QTEJudgment
{
    GreatSuccess,
    Success,
    Fail
}

public delegate void QTEListener(QTEResult result);

public class QuickTimeEvent
{
    public readonly KeyCode TriggerKey;
    public readonly float CastTime;
    public readonly float JudgeFactor;
    public readonly float JudgeTime;

    public float QTETimer { get; private set; }
    public bool IsCompleted { get; private set; }
    
    private readonly List<QTEListener> _listeners;
    
    // 대성공이 나오는 판정 범위 (ms)
    private const float JudgeMs = 0.08f;

    /// <summary>
    /// Quick Time Event 생성
    /// </summary>
    /// <param name="triggerKey"> QTE에 사용될 키 </param>
    /// <param name="castTime"> QTE 시간 </param>
    /// <param name="judgeTimePer"> 판정 시간 (QTE 시간의 몇 퍼센트) </param>
    /// <param name="judgeFactor"> 판정 배율 (기본 80ms) </param>
    public QuickTimeEvent(KeyCode triggerKey, float castTime, float judgeFactor = 1f, float judgeTimePer = 80f)
    {
        TriggerKey = triggerKey;
        CastTime = castTime;

        if (judgeFactor <= 0f)
        {
            Debug.LogWarning("QTE 판정 배율이 0과 같거나 0보다 작습니다");
        }
        
        JudgeFactor = Mathf.Max(0.01f, judgeFactor);
        
        if (judgeTimePer is > 100f or < 0f)
        {
            Debug.LogWarning("QTE 판정 시간 (퍼센트) 이(가) 0보다 작거나 100보다 큽니다");
        }

        var clampedJudgeTime = Mathf.Clamp(judgeTimePer, 0f, 100f);
        JudgeTime = castTime * clampedJudgeTime / 100f;
        
        _listeners = new List<QTEListener>();
    }

    public void Start(MonoBehaviour mono)
    {
        mono.StartCoroutine(QTECoroutine());
    }

    private IEnumerator QTECoroutine()
    {
        do
        {
            var checkMs = JudgeTime - QTETimer;

            if (Input.GetKey(TriggerKey))
            {
                CheckJudgment(checkMs);
                IsCompleted = true;
                yield break;
            }
            
            QTETimer += Time.deltaTime;

            yield return null;
        } 
        while (QTETimer < CastTime);
        
        Invoke(new QTEResult(CastTime, QTEJudgment.Fail));
        IsCompleted = true;
    }

    public void AddListener(QTEListener listener)
    {
        _listeners.Add(listener);
    }

    private void Invoke(QTEResult result)
    {
        foreach (var listener in _listeners)
        {
            listener.Invoke(result);
        }
    }
    
    private void CheckJudgment(float checkMs)
    {
        var greatSuccessMs = JudgeMs * JudgeFactor;
        var successMs = greatSuccessMs * 2;
        
        if (checkMs < greatSuccessMs && checkMs >= -greatSuccessMs)
        {
            Invoke(new QTEResult(checkMs, QTEJudgment.GreatSuccess));
        }
        else if (checkMs <= successMs && checkMs >= -successMs)
        {
            Invoke(new QTEResult(checkMs, QTEJudgment.Success));
        }
        else
        {
            Invoke(new QTEResult(checkMs, QTEJudgment.Fail));
        }
    }
}
