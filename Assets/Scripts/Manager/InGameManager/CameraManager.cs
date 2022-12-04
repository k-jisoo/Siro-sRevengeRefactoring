using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera bossCamera;
    public CinemachineVirtualCamera playerCamera;

    #region 보스 카메라 설정
    public void SetFollow(Transform Boss)   //보스 등장시 본인의 Transform을 전달해주어야 함.
    {
        bossCamera.Follow = Boss;
        Managers.CameraManager.playerCamera.Priority = 9;
        Managers.CameraManager.bossCamera.Priority = 11;
    }

    public void SetPriority(int priority)
    {
        print("setpriority");
        bossCamera.Priority = priority;
    }
    #endregion
}
