using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShakeOnActive : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _virtCam;
    [SerializeField] float _camShakeMagnitude = 1f;
    [SerializeField] float _shakeDuration = .5f;

    CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    // Start is called before the first frame update
    void Start()
    {

        _cinemachineBasicMultiChannelPerlin = _virtCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _camShakeMagnitude;
    }
    private void OnEnable()
    {
        StartCoroutine(ShakeCam());
    }

    private void OnDisable()
    {
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;

    }

    IEnumerator ShakeCam()
    {

        if (_cinemachineBasicMultiChannelPerlin != null)
        {
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _camShakeMagnitude;
            yield return new WaitForSeconds(_shakeDuration);
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        }

    }
}
