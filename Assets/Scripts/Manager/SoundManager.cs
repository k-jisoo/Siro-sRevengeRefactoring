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
    /// BGM 또는 SFX(효과음) 클립을 찾아옴
    /// </summary>
    /// <param name="path">가져올 오디오 클립 이름</param>
    /// <param name="soundType">오디오 타입 (BGM 또는 SFX)</param>
    /// <returns></returns>
    private AudioClip SearchingAudioClip(string path, Define.SoundType soundType)
    {
        AudioClip clip;
        if (soundType == Define.SoundType.BGM) // 클립 타입이 BGM 이면
        {
            clip = Managers.Resource.GetAudioClip(BGM_CLIP_DEFAULT_PATH + path); // BGM은 자주 이용되지 않게 때문에 따로 저장하지 않고 빼오는게 이득인거 같음
        }
        else // 클립 타입이 효과음이면
        {
            if(!sfxClips.TryGetValue(path, out clip)) // 효과음(SFX)는 자주 호출되기 때문에 따로 저장하고 불러오는 형태로 진행
            {
                clip = Managers.Resource.GetAudioClip(SFX_CLIP_DEFAULT_PATH + path); 
                sfxClips.Add(path, clip);
            }
        }

        return clip;
    }

    /// <summary>
    /// BGM 오디오 재생
    /// </summary>
    /// <param name="bgmClipName">재생할 BGM 클립 이름</param>
    /// <param name="bgmAudio">BGM을 재생할 오디오 (기본 값 Null)</param>
    /// <param name="vol">볼륨 (기본 값 0.2)</param>
    /// <param name="isloop">BGM 반복 (기본 값 True)</param>
    public void PlayBGMAudio(string bgmClipName, AudioSource bgmAudio = null, float vol = 0.5f, bool isloop = true)
    {
        if (bgmAudio == null) bgmAudio = bgmAduioSource;

        bgmAudio.clip = SearchingAudioClip(bgmClipName, Define.SoundType.BGM);

        if(bgmAudio.clip == null)
        {
            Debug.Log($"{bgmClipName} 을 재생하지 못했습니다.");
            return;
        }

        bgmAudio.volume = vol;
        bgmAudio.loop = isloop;

        bgmAudio.Play();
    }

    /// <summary>
    /// SFX 오디오 재생
    /// </summary>
    /// <param name="sfxClipName">재생할 SFX 클립 이름</param>
    /// <param name="sfxAudio">SFX을 재생할 오디오 (기본 값 Null)</param>
    /// <param name="vol">볼륨 (기본 값 1.0)</param>
    /// <param name="isloop">BGM 반복 (기본 값 false)</param>
    public void PlaySFXAudio(string sfxClipName, AudioSource sfxAudio = null, float vol = 1.0f, bool isloop = false)
    {
        if (sfxAudio == null) sfxAudio = sfxAudioSource;

        sfxAudio.clip = SearchingAudioClip(sfxClipName, Define.SoundType.SFX);

        if (sfxAudio.clip == null)
        {
            Debug.Log($"{sfxClipName} 을 재생하지 못했습니다.");
            return;
        }

        sfxAudio.volume = vol;
        sfxAudio.loop = isloop;

        sfxAudio.PlayOneShot(sfxAudio.clip);
    }

    /// <summary>
    /// SFX 오디오 딜레이 재생
    /// </summary>
    /// <param name="delay">딜레이 시간</param>
    /// <param name="sfxClipName">재생할 SFX 클립 이름</param>
    /// <param name="sfxAudio">SFX을 재생할 오디오 (기본 값 Null)</param>
    /// <param name="vol">볼륨 (기본 값 1.0)</param>
    /// <param name="isloop">SFX 반복 (기본 값 false)</param>
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
    /// BGM 오디오 재생
    /// </summary>
    /// <param name="delay">딜레이 시간</param>
    /// <param name="bgmClipName">재생할 BGM 클립 이름</param>
    /// <param name="bgmAudio">BGM을 재생할 오디오 (기본 값 Null)</param>
    /// <param name="vol">볼륨 (기본 값 0.2)</param>
    /// <param name="isloop">BGM 반복 (기본 값 True)</param>
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
