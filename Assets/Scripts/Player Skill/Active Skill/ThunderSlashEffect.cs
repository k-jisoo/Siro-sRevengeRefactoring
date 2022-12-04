using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderSlashEffect : MonoBehaviour
{

    private void Start()
    {
        Managers.Sound.PlaySFXAudio("Player/Active Skill/ThunderSlash_2");
    }
    private void OnDestroy()
    {
        Managers.Sound.PlaySFXAudio("Player/Active Skill/ThunderSlash_3");
        Destroy(gameObject);
    }
}
