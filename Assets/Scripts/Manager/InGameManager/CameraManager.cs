using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera bossCamera;
    public CinemachineVirtualCamera playerCamera;

    #region ���� ī�޶� ����
    public void SetFollow(Transform Boss)   //���� ����� ������ Transform�� �������־�� ��.
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
