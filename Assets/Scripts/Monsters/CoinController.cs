using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] int coinValue;
    private readonly string coinSFX = "Player/Get Gold/GetGold";


    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 1, 0) * 180 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.CompareTag(Define.StringTag.Player.ToString()))
        {
            Managers.StageManager.Player.PlayerGold += coinValue;
            Managers.Sound.PlaySFXAudio(coinSFX, null, 0.25f);
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        MemoryPoolManager.GetInstance().InputGameObject(gameObject);
    }
}
