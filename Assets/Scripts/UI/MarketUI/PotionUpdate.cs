using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionUpdate : MonoBehaviour
{
    AudioSource audioSoure;
    public Button btn;
    public Text hp;
    public Text potion;
    public Text money; 

    // Start is called before the first frame update
    void Start()
    {
        audioSoure = GetComponent<AudioSource>();
        btn.onClick.AddListener(Potion);
       
    }

    // Update is called once per frame
    public void Potion()
    {
       
        if (Managers.StageManager.Player.PlayerGold >= 150)
            
        {
            audioSoure.Play();

            Managers.StageManager.Player.PlayerGold -= 150;
           
            Managers.StageManager.Player.Hp += 10;
            potion.text = "체력 회복!";
            Managers.UI.UpdatePlayerHpSlider(Managers.StageManager.Player.Hp, Managers.StageManager.Player.MaxHp);


        }
        else { potion.text = "구매 불가! "; }


        
        hp.text = Managers.StageManager.Player.MaxHp.ToString();
        money.text = Managers.StageManager.Player.PlayerGold.ToString();
    }
}
