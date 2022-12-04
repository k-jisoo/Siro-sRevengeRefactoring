using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SkillEffectVolumeManager : MonoBehaviour
{
    private Volume volueEffect;

    private void Start()
    {
        volueEffect = GetComponent<Volume>();   
    }

    /// <summary>
    /// 스킬 URP Volume 변경
    /// </summary>
    /// <param name="postProcessProfile">스킬 URP Volume 프로필</param>
    public void ChagnePostProcessProfile(VolumeProfile postProcessProfile)
    {
        volueEffect.profile = postProcessProfile;
    }
}
