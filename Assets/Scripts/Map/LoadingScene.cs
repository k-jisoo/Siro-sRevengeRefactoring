using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    static string nextScene;
    public Image progressBar;
    float time;
    bool onFadeEffect = false;

    public static void LoadScene(string sceneName)
    {
        Managers.Sound.StopBGMAudio();
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    
    void Start()
    {
        StartCoroutine(LoadSceneProgress());
    }
    IEnumerator LoadSceneProgress()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        //float timer = 0f;
        time = 0;
        while (!op.isDone)
        {
            time += Time.unscaledDeltaTime;
            progressBar.fillAmount = time/2f;
            if (progressBar.fillAmount >= 0.9f && !onFadeEffect)
            {
                Managers.StageManager.SenecFadeEffect();
                onFadeEffect = true;
            }

            if (progressBar.fillAmount >= 1f)
            {
                op.allowSceneActivation = true;
                Managers.StageManager.SetStageKillCount();
                Managers.StageManager.isBossSpawn = false;
                yield break;
            }
            yield return null;
        }
    }

  
}
