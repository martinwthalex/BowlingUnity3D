using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CM_vcam : MonoBehaviour
{
    #region Variables and initializing
    static CinemachineVirtualCamera vcam;
    static float noise_amount = 0.1f;
    static float freq_gain = 1f;
    private void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        Noise(false);
    }
    #endregion

    #region Add noise to the cam
    public static void Noise(bool noise)
    {
        if (noise)
        {
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = noise_amount;
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = freq_gain;
        }
        else
        {
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
        }
    }
    #endregion
}
