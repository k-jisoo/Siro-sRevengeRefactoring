using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Prologue : MonoBehaviour
{
    /// <summary>
    /// 애니메이션에서 호출
    /// </summary>
    public void NextSecen()
    {
        SceneManager.LoadScene("Stage1");
    }
}
