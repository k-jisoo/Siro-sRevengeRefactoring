using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pingpong : MonoBehaviour
{
    private void Start()
    {
        OnPingpongEffect();
    }

    private void OnPingpongEffect()
    {LeanTween.moveY(gameObject,580,1.5f).setEaseOutQuad().setLoopPingPong();}
}
