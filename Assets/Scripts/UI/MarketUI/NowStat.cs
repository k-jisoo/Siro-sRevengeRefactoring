using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NowStat : MonoBehaviour
{
    public Text speed;
    public Text sheld;
    public Text hp;
    public Text attack;
    public Text money;

    // Start is called before the first frame update
    void Start()
    {

        //�̵��ӵ� , ü�� , ����, ���ݷ� 
        speed.text = Managers.StageManager.Player.MoveSpeed.ToString();
        sheld.text = Managers.StageManager.Player.Armor.ToString();
        hp.text = Managers.StageManager.Player.MaxHp.ToString();
        attack.text = Managers.StageManager.Player.DefaultAttackDamage.ToString();
        money.text = Managers.StageManager.Player.PlayerGold.ToString();

    }
}

    