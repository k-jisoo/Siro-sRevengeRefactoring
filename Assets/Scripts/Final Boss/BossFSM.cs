using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossFSM
{


    public Define.BossState bossState;
    private Boss boss;
    public bool runDarkHeal = false;


    public BossFSM(Boss boss) 
    {
        this.boss = boss;
    }
    
    // Update is called once per frame
    public void Update()
    {
        switch (bossState)
        {
            case Define.BossState.MOVE_STATE: 
                boss.Move(); break;
            case Define.BossState.ATTACK_STATE:
                boss.Attack(); break;               
            case Define.BossState.HURT_STATE:
                boss.Hurt();  break;            
            case Define.BossState.CASTING_STATE:
               break;
            case Define.BossState.DEAD_STATE:
                boss.Die(); break;
            case Define.BossState.PATTERN_DARKHEAL_STATE:
                boss.Pattern_DarkHeal(); runDarkHeal = true; break;
            case Define.BossState.PATTERN_RUINSTK_STATE:
                boss.Pattern_RuinStk(); break;
            case Define.BossState.PATTERN_SUMNSKELETON_STATE:
                boss.Pattern_SummonSkeleton(); break;
            case Define.BossState.PATTERN_BIND_STATE:
                boss.Pattern_Bind(); break;
        }
      
    }
   
   
}

