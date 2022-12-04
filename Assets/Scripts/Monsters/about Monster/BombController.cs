using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    int damage = 3;
    float bombAttackRadius = 0.5f;
    public GameObject crab;
    public Animator anim;
    float shootSpeed=4f;
    Vector3 dir;
    float time;

    AudioSource bombAudio;
    float bombVolume;

    void OnEnable()
    {
        dir = (this.transform.position - crab.transform.position).normalized;
        anim = GetComponent<Animator>();
        bombAudio = GetComponent<AudioSource>();
        time = 0;
    }

    void Update()
    {
        if (time >= 0.5f)
        {
            anim.SetTrigger("bomb");
            Debug.Log("Set Bomb!");
            Debug.Log(name + "_Attack");
            Managers.Sound.PlaySFXAudio("Monster/" + "bomb_Attack", bombAudio, bombVolume, false);
            StartCoroutine(Bomb());
        }
        else
        {
            transform.Translate(dir.x * shootSpeed * Time.deltaTime, dir.y * shootSpeed * Time.deltaTime, 0, Space.World);
            time += Time.deltaTime;
        }
    }

    IEnumerator Bomb()
    {
        yield return new WaitForSeconds(1.2f);
        anim.SetTrigger("back");
        time = 0;
        gameObject.SetActive(false);
    }

    void AttackPlayer()
    {
        Collider2D target = Physics2D.OverlapCircle(this.transform.position, bombAttackRadius, 1 << 10);
        if (target != null)
            target.gameObject.GetComponent<Player>().TakeDamage(damage);
    }
}
