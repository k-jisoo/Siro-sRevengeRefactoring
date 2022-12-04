using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{ 
    private readonly string AUDIO_CLIP_DEFAULT_PATH = "Sounds/";
    private readonly string GAME_OBJECT_DEFAULT_PATH = "Prefabs/";

    public T Load<T>(string resoucrePath) where T : Object
    {
        return Resources.Load<T>(resoucrePath);
    }

    /// <summary>
    /// 리소스 폴더에 있는 오디오 클립을 가져옴
    /// </summary>
    /// <param name="path">가져올 오디오 클립 경로</param>
    /// <returns>오디오 클립 반환</returns>
    public AudioClip GetAudioClip (string path)
    {
        AudioClip clip = Load<AudioClip>(AUDIO_CLIP_DEFAULT_PATH + path);

        if(clip == null)
        {
            Debug.Log($"{path} 오디오 클립을 찾을 수 가 없습니다.");
            return null;
        }

        return clip;
    }

    /// <summary>
    /// 리소스 폴더에 있는 게임 오브젝트를 가져옴
    /// </summary>
    /// <param name="path">가져올 게임 오브젝트 경로</param>
    /// <returns>게임 오브젝트 반환</returns>
    public GameObject GetPerfabGameObject(string path)
    {
        GameObject gameObject = Load<GameObject>(GAME_OBJECT_DEFAULT_PATH + path);

        if (gameObject == null)
        {
            Debug.Log($"{gameObject} 게임 오브젝트를 찾을 수 가 없습니다.");
            return null;
        }

        return gameObject;
    }
}
