public class Define 
{
    /// <summary>
    /// 재상할 오디오 타입 (0 = BGM , 1 = 효과음)
    /// </summary>
    public enum SoundType
    {
        BGM = 0,
        SFX = 1,
    }

    /// <summary>
    /// 오브젝트 Tag 표시
    /// </summary>
    public enum StringTag
    {
        Enemy = 0,
        Player = 1,
    }

    /// <summary>
    /// 스킬 상태 표시
    /// </summary>
    public enum CurrentSkillState
    {
        ACTIVE = 0,
        COOL_TIME = 1,
    }

    /// <summary>
    /// 스테이지 상황 표시
    /// </summary>
    public enum Stage
    {
        STAGE1,
        STORE1,
        STAGE2,
        STORE2,
        STAGE3,
        STORE3,
        STAGE4,
        STORE4,
        Boss,
    }

    /// <summary>
    /// 생성힐 프리팹 타입 (Path)
    /// </summary>
    public enum PrefabType
    {
        Player_Skill,
        Final_Boss_Skill,
        Monsters,
        UI,
        SubBoss,
    }
    /// <summary>
    /// BOSS에서 사용하는 STATE의 열거형 변수 집합
    /// </summary>
    public enum BossState
    {
        MOVE_STATE,
        ATTACK_STATE,
        HURT_STATE,
        CASTING_STATE,
        DEAD_STATE,
        PATTERN_DARKHEAL_STATE,
        PATTERN_RUINSTK_STATE,
        PATTERN_SUMNSKELETON_STATE,
        PATTERN_BIND_STATE
    };
}
