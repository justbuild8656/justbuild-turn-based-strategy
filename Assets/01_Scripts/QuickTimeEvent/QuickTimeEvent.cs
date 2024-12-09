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
    private readonly KeyCode _qteKey;
    
    /// <summary>
    /// 해당 QTE가 진행되는데 걸리는 시간.
    /// </summary>
    public readonly float QTEDuration;
    
    /// <summary>
    /// 현재 QTE가 진행된 시간
    /// </summary>
    public float CurrentTime { get; private set; }

    /// <summary>
    /// 판정이 나오기 위해 필요한 반응 속도 (ms).
    /// 반드시 음수가 아닌 양수로만 기입할 것.
    /// 단 Fail 값의 경우 -1f 로 기입할 것.
    /// </summary>
    private readonly Dictionary<QTEJudgment, float> _qteJudgmentMs = new()
    {
        { QTEJudgment.GreatSuccess, 80f },
        { QTEJudgment.Success, 120f },
        { QTEJudgment.Fail, -1f },
    };

    private readonly List<QTEListener> _listeners;
    
    /// <summary>
    /// 이벤트가 완료되었는지 확인할 수 있는 변수.
    /// </summary>
    public bool IsCompleted { get; private set; }
    
    /// <param name="qteKey"> QTE가 진행중일 떄 눌러야 하는 키. </param>
    /// <param name="qteDuration"> QTE 가 진행되는 시간. </param>
    /// <param name="qteJudgmentMs">
    /// 판정이 나오기 위해 요구되는 반응 속도 (ms) 설정.
    /// Fail 값만 -1f 로 기입하고 나머지 값은 양수로 기입할 것.
    /// </param>
    public QuickTimeEvent(KeyCode qteKey, float qteDuration, Dictionary<QTEJudgment, float> qteJudgmentMs = null)
    {
        _qteKey = qteKey;
        QTEDuration = qteDuration;

        if (qteJudgmentMs != null)
        {
            _qteJudgmentMs = qteJudgmentMs;
        }
        
        _listeners = new List<QTEListener>();
    }

    /// <summary>
    /// QTE 이벤트 시작 함수
    /// </summary>
    public void Start()
    {
        if (QTEManager.Instance == null)
        {
            Debug.LogError("QTE Manager 인스턴스가 존재하지 않습니다!");
            return;
        }

        QTEManager.Instance.StartQTE(this);
    }

    public IEnumerator EventCoroutine()
    {
        float checkMS;
        
        while (true)
        {
            checkMS = QTEDuration / 2 - CurrentTime;
             
            if (CurrentTime > QTEDuration) break;

            if (Input.GetKeyDown(_qteKey))
            {
                CheckJudgment(checkMS);
                IsCompleted = true;
                yield break;
            }

            CurrentTime += Time.deltaTime;
            yield return null;
        }

        CallListeners(new QTEResult(checkMS, QTEJudgment.Fail));
        IsCompleted = true;
    }

    /// <summary>
    /// QTE 판정 체크 함수
    /// </summary>
    /// <param name="checkMs"> 판정 체크를 할 지점으로부터 현재 누른 지점의 거리 </param>
    private void CheckJudgment(float checkMs)
    {
        // 밀리초 단위로 변경 ((s)/1000 = (ms))
        var greatSuccessMs = _qteJudgmentMs[QTEJudgment.GreatSuccess] / 1000;
        var successMs = _qteJudgmentMs[QTEJudgment.Success] / 1000;
        
        if (checkMs < greatSuccessMs && checkMs >= -greatSuccessMs)
        {
            CallListeners(new QTEResult(checkMs, QTEJudgment.GreatSuccess));
        }
        else if (checkMs <= successMs && checkMs >= -successMs)
        {
            CallListeners(new QTEResult(checkMs, QTEJudgment.Success));
        }
        else
        {
            CallListeners(new QTEResult(checkMs, QTEJudgment.Fail));
        }
    }

    public void AddListener(QTEListener listener)
    {
        _listeners.Add(listener);
    }

    private void CallListeners(QTEResult result)
    {
        foreach (var listener in _listeners)
        {
            listener.Invoke(result);
        }
    }
}
