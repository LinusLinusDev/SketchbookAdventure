using UnityEngine;
using Cinemachine;

public class CameraEffects : MonoBehaviour
{
    public Vector3 cameraWorldSize;
    public CinemachineFramingTransposer cinemachineFramingTransposer;
    [SerializeField] private CinemachineBasicMultiChannelPerlin multiChannelPerlin;
    public float screenYDefault;
    public float screenYTalking;
    [Range(0, 10)]
    [System.NonSerialized] public float ShakeLength = 10;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        cinemachineFramingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        screenYDefault = cinemachineFramingTransposer.m_ScreenX;
        NewPlayer.Instance.cameraEffects = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        multiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        virtualCamera.Follow = NewPlayer.Instance.transform;
    }

    void Update()
    {
        multiChannelPerlin.m_FrequencyGain += (0 - multiChannelPerlin.m_FrequencyGain) * Time.deltaTime * (10 - ShakeLength);
    }

    public void Shake(float shake, float length)
    {
        ShakeLength = length;
        multiChannelPerlin.m_FrequencyGain = shake;
    }
}
