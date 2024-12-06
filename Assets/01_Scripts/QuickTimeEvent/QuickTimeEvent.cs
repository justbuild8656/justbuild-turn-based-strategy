using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum QTEResult
{
    Fail,
    Success,
    GreatSuccess
}

public struct QTEData
{
    public readonly KeyCode QTEKey; // QTE 에 사용될 Key
    public readonly float QTETime; // QTE 진행시간
    public readonly float SuccessMs; // 성공 판정이 나오는 시간 (ms 단위)
    public readonly float GreatSuccessMs; // 대성공 판정이 나오는 시간 (ms 단위)

    public QTEData(KeyCode qteKey, float qteTime, float successMs, float greatSuccessMs)
    {
        QTEKey = qteKey;
        QTETime = qteTime;
        SuccessMs = successMs;
        GreatSuccessMs = greatSuccessMs;
    }
}

public class QuickTimeEvent
{
    private readonly QTEData _qteData;
    private float _curTime; // QTE 이벤트 시작후 걸린 시간

    public float CheckMs { get; private set; }
    public bool IsEnded { get; private set; } // 이벤트 종료 여부
    public readonly UnityEvent<QTEResult> OnTrigger; // 이벤트 종료후 호출되는 이벤트

    public QuickTimeEvent(QTEData qteData)
    {
        _qteData = qteData;
        _curTime = 0;
        IsEnded = false;
        OnTrigger = new UnityEvent<QTEResult>();
    }

    // 이벤트 시작
    public void Start()
    {
        QTEManager.Instance.StartQTE(this);
    }
    
    public IEnumerator EventCoroutine()
    {
        while (true)
        {
            CheckMs = _qteData.QTETime / 2 - _curTime;
            
            if (_curTime > _qteData.QTETime) break;

            if (Input.GetKeyDown(_qteData.QTEKey))
            {
                CheckJudgment(CheckMs);
                IsEnded = true;
                yield break;
            }

            _curTime += Time.deltaTime;
            yield return null;
        }
        
        OnTrigger.Invoke(QTEResult.Fail);
        IsEnded = true;
    }

    // QTE 판정 체크 (checkMs = 판정 체크를 할 지점으로부터 현재 누른 지점의 거리)
    private void CheckJudgment(float checkMs)
    {
        if (checkMs < _qteData.GreatSuccessMs && checkMs >= -_qteData.GreatSuccessMs)
        {
            OnTrigger.Invoke(QTEResult.GreatSuccess);
        }
        else if (checkMs <= _qteData.SuccessMs && checkMs >= -_qteData.SuccessMs)
        {
            OnTrigger.Invoke(QTEResult.Success);
        }
        else
        {
            OnTrigger.Invoke(QTEResult.Fail);
        }
    }
}
