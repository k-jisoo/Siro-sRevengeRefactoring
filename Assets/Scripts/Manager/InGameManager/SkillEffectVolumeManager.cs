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
    /// ��ų URP Volume ����
    /// </summary>
    /// <param name="postProcessProfile">��ų URP Volume ������</param>
    public void ChagnePostProcessProfile(VolumeProfile postProcessProfile)
    {
        volueEffect.profile = postProcessProfile;
    }
}
