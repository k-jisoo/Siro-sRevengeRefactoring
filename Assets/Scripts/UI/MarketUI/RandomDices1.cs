using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomDices1 : MonoBehaviour
{
    public Text statResult;
    public Image skill;
    public Sprite[]simage;
    public Button btn;

    AudioSource audioSoure;
    int RandomInt;

    public Text money;
    public Text speed;
    public Text sheld;
    public Text hp;
    public Text attack;

    private void Start()
    {
        audioSoure = GetComponent<AudioSource>();
        btn.onClick.AddListener(Dice);

        money.text = Managers.StageManager.Player.PlayerGold.ToString();
        sheld.text = Managers.StageManager.Player.Armor.ToString();
        attack.text = Managers.StageManager.Player.DefaultAttackDamage.ToString();
        speed.text = Managers.StageManager.Player.MoveSpeed.ToString();
        hp.text = Managers.StageManager.Player.MaxHp.ToString();
    }

    public void Dice()


    {
       

        RandomInt = Random.Range(1, 8);
   

        if (Managers.StageManager.Player.PlayerGold >= 150)

        {    Managers.StageManager.Player.PlayerGold -= 150;
            audioSoure.Play();

            if (RandomInt == 1)
            {
                skill.sprite = simage[0];
                statResult.text = "공격력 증가!";
                Managers.StageManager.Player.DefaultAttackDamage += 3;
               

            }

            else if (RandomInt == 2)
            {
                skill.sprite = simage[1];
                statResult.text = "방어력 증가!";
                Managers.StageManager.Player.Armor += 2;
          
            }

            else if (RandomInt == 3)
            {
                skill.sprite = simage[2];
                statResult.text = "이동속도 증가!";
                Managers.StageManager.Player.MoveSpeed += 0.25f;
               
            }

            else if (RandomInt == 4)
            {
                skill.sprite = simage[3];
                statResult.text = "최대체력 증가!";
                Managers.StageManager.Player.MaxHp += 15;
               
            }

            else if (RandomInt == 5)
            {
                

                if (Managers.StageManager.Player.DefaultAttackDamage >= 50)
                {
                    
                    skill.sprite = simage[4];
                    statResult.text = "공격력 감소!";
                    Managers.StageManager.Player.DefaultAttackDamage -= 3;
                }
                else //if (Managers.StageManager.Player.DefaultAttackDamage <= 50)
                {
                    
                    skill.sprite = simage[0];
                     statResult.text = "공격력 증가!";
                     Managers.StageManager.Player.DefaultAttackDamage += 3;
                }

            }

            else if (RandomInt == 6)
            {
               

                if (Managers.StageManager.Player.Armor >= 50)
                {
                   
                    skill.sprite = simage[5];
                    statResult.text = "방어력 감소!";
                    Managers.StageManager.Player.Armor -= 2;
                }
                else // if (Managers.StageManager.Player.Armor <= 50)
                {
                    
                    skill.sprite = simage[1];
                     statResult.text = "방어력 증가!";
                     Managers.StageManager.Player.Armor += 2;
                }
            }

            else if (RandomInt == 7)
            {
               

                if (Managers.StageManager.Player.MoveSpeed >= 50)
                {
                    skill.sprite = simage[6];
                    statResult.text = "이동속도 감소!";
                    Managers.StageManager.Player.MoveSpeed -= 0.25f;
                }
                else //if (Managers.StageManager.Player.MoveSpeed <= 50)
                {
                   
                     skill.sprite = simage[2];
                     statResult.text = "이동속도 증가!";
                     Managers.StageManager.Player.MoveSpeed += 0.25f;
                }
            }

            else if (RandomInt == 8)
            {
                skill.sprite = simage[7];


                if (Managers.StageManager.Player.MaxHp >= 50) 
                {
                   
                    skill.sprite = simage[7];
                    statResult.text = "최대체력 감소!";
                    Managers.StageManager.Player.MaxHp -= 15;
                }
                else //if (Managers.StageManager.Player.MaxHp <= 50)
                {
                    {
                      
                        skill.sprite = simage[3];
                         statResult.text = "최대체력 증가!";
                         Managers.StageManager.Player.MaxHp += 15;

                    }
                }
            }




            money.text = Managers.StageManager.Player.PlayerGold.ToString();
            sheld.text = Managers.StageManager.Player.Armor.ToString();
            attack.text = Managers.StageManager.Player.DefaultAttackDamage.ToString();
            speed.text = Managers.StageManager.Player.MoveSpeed.ToString();
            hp.text = Managers.StageManager.Player.MaxHp.ToString();



        }
        else
        {
            statResult.text = " 돈이 부족합니다."; }
    }




}



