using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Golem2Attack : BasicMonsterController
{
    public GameObject beam;
    public Sprite laser;
    public Sprite square;
    float attackTime = 2.0f, warningTime = 4.0f;
    SpriteRenderer beamRenderer;
    public Collider2D beamCollider;
    Vector3 dir;


    /*private new void Start()
    {
        base.Start();
        beamRenderer = beam.GetComponent<SpriteRenderer>();
        beamRenderer.sprite = square;
        state = State.Attack;
        base.coolTime = 0;
        StartCoroutine("Beam");
    }*/

    private void OnEnable()
    {
        beam.SetActive(false);
        beamRenderer = beam.GetComponent<SpriteRenderer>();
        beamRenderer.sprite = square;
        state = State.Attack;
        base.coolTime = 0;
        StartCoroutine("Beam");
    }

    protected override void Attack()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }


    IEnumerator Beam()
    {
        beamRenderer.sprite = square;
        beamRenderer.color = new Color(255, 0, 0, 50);
        state = State.Run;
        yield return new WaitForSeconds(base.skillTime);
        state = State.Attack;
        beam.transform.position = this.transform.position;
        dir = transform.position - playerTarget.transform.position;
        beam.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        beam.transform.Rotate(0, 0, 90);
        beam.SetActive(true);

        yield return new WaitForSeconds(warningTime);

        string name = base.attackAudio.ToString().Substring(0, base.attackAudio.ToString().Length - 33);
        Debug.Log(name + "_Attack");
        Managers.Sound.PlaySFXAudio("Monster/" + name + "_Attack", base.attackAudio, base.volume, false);

        beamRenderer.color = new Color(225, 225, 225, 225);
        beamRenderer.sprite = laser;
        beamCollider.enabled = true;//공격
        yield return new WaitForSeconds(attackTime);
        beamCollider.enabled = false; // 비활성화
        beam.SetActive(false);

        StartCoroutine("Beam");
    }
}