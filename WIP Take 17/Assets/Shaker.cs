using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Shaker : MonoBehaviour
{
    public static Shaker Instance { get; private set;}

    CinemachineVirtualCamera cinemachineVirtualCamera;
    float shakeTimer;

    void Awake() {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time) {
        CinemachineBasicMultiChannelPerlin cinemachineBasic = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasic.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    void Update() {
        if (shakeTimer > 0) {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f) {
                CinemachineBasicMultiChannelPerlin cinemachineBasic = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasic.m_AmplitudeGain = 0f;
            }
        }
    }
}
