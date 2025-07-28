using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FixedCameraWidth : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    public float targetWidth = 19f;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        AdjustCameraOrthographicSize();
    }

    void AdjustCameraOrthographicSize()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        virtualCamera.m_Lens.OrthographicSize = (targetWidth / 2f) / screenAspect;
    }
}
