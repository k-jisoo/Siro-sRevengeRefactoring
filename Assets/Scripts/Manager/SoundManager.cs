using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{


    [SerializeField] AudioSource bgmAduioSource;
    [SerializeField] AudioSource sfxAudioSource;

    private Dictionary<string, AudioClip> bgmClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();

    private readonly string BGM_CLIP_DEFAULT_PATH = "BGM/";
    private readonly string SFX_CLIP_DEFAULT_PATH = "SFX/";

    private Dictionary<float, WaitForSeconds> delayTime = new Dictionary<float, WaitForSeconds>();

    /// <summary>
    /// BGM �Ǵ� SFX(ȿ����) Ŭ���� ã�ƿ�
    /// </summary>
    /// <param name="path">������ ����� Ŭ�� �̸�</param>
    /// <param name="soundType">����� Ÿ�� (BGM �Ǵ� SFX)</param>
    /// <returns></returns>
    private AudioClip SearchingAudioClip(string path, Define.SoundType soundType)
    {
        AudioClip clip;
        if (soundType == Define.SoundType.BGM) // Ŭ�� Ÿ���� BGM �̸�
        {
            clip = Managers.Resource.GetAudioClip(BGM_CLIP_DEFAULT_PATH + path); // BGM�� ���� �̿���� �ʰ� ������ ���� �������� �ʰ� �����°� �̵��ΰ� ����
        }
        else // Ŭ�� Ÿ���� ȿ�����̸�
        {
            if(!sfxClips.TryGetValue(path, out clip)) // ȿ����(SFX)�� ���� ȣ��Ǳ� ������ ���� �����ϰ� �ҷ����� ���·� ����
            {
                clip = Managers.Resource.GetAudioClip(SFX_CLIP_DEFAULT_PATH + path); 
                sfxClips.Add(path, clip);
            }
        }

        return clip;
    }

    /// <summary>
    /// BGM ����� ���
    /// </summary>
    /// <param name="bgmClipName">����� BGM Ŭ�� �̸�</param>
    /// <param name="bgmAudio">BGM�� ����� ����� (�⺻ �� Null)</param>
    /// <param name="vol">���� (�⺻ �� 0.2)</param>
    /// <param name="isloop">BGM �ݺ� (�⺻ �� True)</param>
    public void PlayBGMAudio(string bgmClipName, AudioSource bgmAudio = null, float vol = 0.5f, bool isloop = true)
    {
        if (bgmAudio == null) bgmAudio = bgmAduioSource;

        bgmAudio.clip = SearchingAudioClip(bgmClipName, Define.SoundType.BGM);

        if(bgmAudio.clip == null)
        {
            Debug.Log($"{bgmClipName} �� ������� ���߽��ϴ�.");
            return;
        }

        bgmAudio.volume = vol;
        bgmAudio.loop = isloop;

        bgmAudio.Play();
    }

    /// <summary>
    /// SFX ����� ���
    /// </summary>
    /// <param name="sfxClipName">����� SFX Ŭ�� �̸�</param>
    /// <param name="sfxAudio">SFX�� ����� ����� (�⺻ �� Null)</param>
    /// <param name="vol">���� (�⺻ �� 1.0)</param>
    /// <param name="isloop">BGM �ݺ� (�⺻ �� false)</param>
    public void PlaySFXAudio(string sfxClipName, AudioSource sfxAudio = null, float vol = 1.0f, bool isloop = false)
    {
        if (sfxAudio == null) sfxAudio = sfxAudioSource;

        sfxAudio.clip = SearchingAudioClip(sfxClipName, Define.SoundType.SFX);

        if (sfxAudio.clip == null)
        {
            Debug.Log($"{sfxClipName} �� ������� ���߽��ϴ�.");
            return;
        }

        sfxAudio.volume = vol;
        sfxAudio.loop = isloop;

        sfxAudio.PlayOneShot(sfxAudio.clip);
    }

    /// <summary>
    /// SFX ����� ������ ���
    /// </summary>
    /// <param name="delay">������ �ð�</param>
    /// <param name="sfxClipName">����� SFX Ŭ�� �̸�</param>
    /// <param name="sfxAudio">SFX�� ����� ����� (�⺻ �� Null)</param>
    /// <param name="vol">���� (�⺻ �� 1.0)</param>
    /// <param name="isloop">SFX �ݺ� (�⺻ �� false)</param>
    public void PlayDelaySFXAudio(string sfxClipName, float delay, AudioSource sfxAudio = null, float vol = 1.0f, bool isloop = false)
    {
        StartCoroutine(DelaySFX(sfxClipName, delay, sfxAudio, vol, isloop));
    }

    private IEnumerator DelaySFX(string sfxClipName, float delay, AudioSource sfxAudio = null, float vol = 1.0f, bool isloop = false)
    {
        PlaySFXAudio(sfxClipName, sfxAudio, vol, isloop);

        yield return WaitForSeconds(delay);
    }

    /// <summary>
    /// BGM ����� ���
    /// </summary>
    /// <param name="delay">������ �ð�</param>
    /// <param name="bgmClipName">����� BGM Ŭ�� �̸�</param>
    /// <param name="bgmAudio">BGM�� ����� ����� (�⺻ �� Null)</param>
    /// <param name="vol">���� (�⺻ �� 0.2)</param>
    /// <param name="isloop">BGM �ݺ� (�⺻ �� True)</param>
    public void PlayDelayBGMAudio(string bgmClipName, float delay, AudioSource bgmAudio = null, float vol = 0.2f, bool isloop = true)
    {
        StartCoroutine(DelayBGM(bgmClipName, delay, bgmAudio, vol, isloop));
    }

    private IEnumerator DelayBGM(string bgmClipName, float delay, AudioSource bgmAudio = null, float vol = 0.2f, bool isloop = true)
    {
        PlayBGMAudio(bgmClipName, bgmAudio, vol, isloop);

        yield return WaitForSeconds(delay);
    }

    private WaitForSeconds WaitForSeconds(float delay)
    {
        WaitForSeconds wfs;
        if (!delayTime.TryGetValue(delay, out wfs))
            delayTime.Add(delay, wfs = new WaitForSeconds(delay));
        return wfs;
    }

    public void StopBGMAudio(AudioSource bgmAudio = null)
    {
        if (bgmAudio == null) bgmAudio = bgmAduioSource;

        if(bgmAudio.isPlaying) bgmAudio.Stop();
    }


}
