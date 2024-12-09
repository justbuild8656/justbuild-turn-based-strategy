public struct QTEResult
{
    /// <summary>
    /// 해당 이벤트를 플레이어가 반응한 속도
    /// </summary>
    public readonly float ReactionTime;
    
    /// <summary>
    /// 플레이어 반응 속도를 기반으로 계산한 QTE의 판정
    /// </summary>
    public readonly QTEJudgment Judgment;

    public QTEResult(float reactionTime, QTEJudgment judgment)
    {
        ReactionTime = reactionTime;
        Judgment = judgment;
    }
}
