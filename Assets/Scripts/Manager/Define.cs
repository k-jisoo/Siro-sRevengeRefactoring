public class Define 
{
    /// <summary>
    /// ����� ����� Ÿ�� (0 = BGM , 1 = ȿ����)
    /// </summary>
    public enum SoundType
    {
        BGM = 0,
        SFX = 1,
    }

    /// <summary>
    /// ������Ʈ Tag ǥ��
    /// </summary>
    public enum StringTag
    {
        Enemy = 0,
        Player = 1,
    }

    /// <summary>
    /// ��ų ���� ǥ��
    /// </summary>
    public enum CurrentSkillState
    {
        ACTIVE = 0,
        COOL_TIME = 1,
    }

    /// <summary>
    /// �������� ��Ȳ ǥ��
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
    /// ������ ������ Ÿ�� (Path)
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
    /// BOSS���� ����ϴ� STATE�� ������ ���� ����
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
