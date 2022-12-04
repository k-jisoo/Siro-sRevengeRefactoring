using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomDices : MonoBehaviour
{
    public Text statResult;
    public Image skill;
    public Sprite[]simage;
    int RandomInt;
    public Text money;


    // Start is called before the first frame update
    void Start()
    {
        RandomInt = Random.Range(1, 9);
        money.text = Managers.StageManager.Player.PlayerGold.ToString();

        if (Managers.StageManager.Player.PlayerGold >= 150)
            Managers.StageManager.Player.PlayerGold -= 150;
        {



            if (RandomInt == 1)
            {
                skill.sprite = simage[0];
                statResult.text = "공격력 100 증가!";
                //스텟 +=100; 
                Managers.StageManager.Player.DefaultAttackDamage += 100;

            }

            else if (RandomInt == 2)
            {
                skill.sprite = simage[1];
                statResult.text = "방어력 100 증가!";
                Managers.StageManager.Player.Armor += 100;
            }

            else if (RandomInt == 3)
            {
                skill.sprite = simage[2];
                statResult.text = "이동속도 100 증가!";
                Managers.StageManager.Player.MoveSpeed += 100;
            }

            else if (RandomInt == 4)
            {
                skill.sprite = simage[3];
                statResult.text = "체력 100 증가!";
                Managers.StageManager.Player.MaxHp += 100;
            }

            else if (RandomInt == 5)
            {

                if (Managers.StageManager.Player.DefaultAttackDamage >= 50)
                {
                    Managers.StageManager.Player.DefaultAttackDamage -= 50;
                    skill.sprite = simage[4];
                    statResult.text = "공격력 50 감소!";
                }
                else
                    statResult.text = "구매 불가능합니다. ";
            }

            else if (RandomInt == 6)
            {

                if (Managers.StageManager.Player.Armor >= 50)
                {
                    Managers.StageManager.Player.Armor -= 50;
                    skill.sprite = simage[5];
                    statResult.text = "방어력 50 감소!";
                }
                else
                    statResult.text = "구매 불가능합니다. ";
            }

            else if (RandomInt == 7)
            {

                if (Managers.StageManager.Player.MoveSpeed >= 50)
                { skill.sprite = simage[6];
                statResult.text = "이동속도 50 감소!";
                Managers.StageManager.Player.MoveSpeed -= 50;
            }
                else
                statResult.text = "구매 불가능합니다. ";
            }

            else if (RandomInt == 8)
            {
                skill.sprite = simage[7];
                statResult.text = "체력 50 증가!";
                if (Managers.StageManager.Player.MaxHp >= 50) {
                    Managers.StageManager.Player.MaxHp -= 50;
                    skill.sprite = simage[7];
                    statResult.text = "체력 50 증가!";
                             
                }
               else
                statResult.text = "구매 불가능합니다. ";
            }


        }
    }




}



