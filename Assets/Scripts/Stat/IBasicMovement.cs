/// <summary>
/// Enemy, Player의 필수 인터페이스
/// </summary>
public interface IBasicMovement
{
    /// <summary>
    /// 이동 함수
    /// </summary>
    void Move();

    /// <summary>
    /// 기본 공격 함수
    /// </summary>
    void DefaultAttack();
}
