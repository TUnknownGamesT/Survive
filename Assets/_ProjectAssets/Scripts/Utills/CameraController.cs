using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    #region Singleton  

    public static CameraController instance;


    private void Awake()
    {

        instance = FindObjectOfType<CameraController>();
        if (instance == null)
        {
            instance = this;
        }

        _cameraTransform = gameObject.transform;
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachineCameraOffset = GetComponent<CinemachineCameraOffset>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }


    #endregion

    [Header("Shaking")]
    public static CinemachineVirtualCamera virtualCamera;
    public static float shakeDuration = 0.15f;
    public static float shakeAmplitude = 2f;
    public RawImage whiteEdge;
    public RawImage bloodyEdge;
    public Transform cameraTarget;
    public float smoothTime = 0.3f;

    private static float shakeTimeRemain;
    private static Transform _cameraTransform;
    private static CinemachineCameraOffset _cinemachineCameraOffset;
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private Vector3 velocity;


    private void OnEnable()
    {
        AIBrain.onEnemyDeath += KillEffect;
    }

    private void OnDisable()
    {
        AIBrain.onEnemyDeath -= KillEffect;
    }

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }


    private void Update()
    {
        if (shakeTimeRemain > 0)
        {
            shakeTimeRemain -= Time.deltaTime;
            if (shakeTimeRemain <= 0)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }

    }

    private void LateUpdate()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        Vector3 newPosition = GetCenterPosition().center;
        cameraTarget.position = Vector3.SmoothDamp(cameraTarget.position, newPosition, ref velocity, smoothTime);
    }


    private Bounds GetCenterPosition()
    {
        var playerPos = GameManager.playerRef.position;
        var crossHairPos = GameManager.crossHairRef.position;
        Bounds bounds = new Bounds(playerPos, Vector3.zero);
        
        const float diff = 20;
        bounds.Encapsulate(
            new Vector3(
                Mathf.Clamp(crossHairPos.x, playerPos.x - diff, playerPos.x + diff),
                crossHairPos.y,
                Mathf.Clamp(crossHairPos.z, playerPos.z - diff, playerPos.z + diff)
                )
            );

        return bounds;
    }

    public static void ShakeCamera(float shakeDuration, float shakeAmplitude)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = shakeAmplitude;
        shakeTimeRemain = shakeDuration;

    }

    public static void SlowMotion(float scaler)
    {
        UniTask.Void(async () =>
        {
            Time.timeScale = scaler;
            await UniTask.Delay(TimeSpan.FromSeconds(0.02f));
            Time.timeScale = 1f;
        });

    }

    public static void ExplosionCameraShake()
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 8f;
        shakeTimeRemain = 1f;
    }


    public void KillEffect()
    {
        LeanTween.value(0, 0.7f, 0.05f).setOnUpdate((float value) =>
        {
            Color c = whiteEdge.color;
            c.a = value;
            whiteEdge.color = c;
        }).setEaseInQuad().setOnComplete(() =>
        {
            LeanTween.value(0.7f, 0, 0.05f).setOnUpdate((float value) =>
            {
                Color c = whiteEdge.color;
                c.a = value;
                whiteEdge.color = c;
            }).setEaseInQuad();
        });
    }

    [ContextMenu("TakeDamageEffect")]
    public void TakeDamageEffect()
    {
        LeanTween.value(0, 0.3f, 0.1f).setOnUpdate((float value) =>
        {
            Color c = bloodyEdge.color;
            c.a = value;
            bloodyEdge.color = c;
        }).setEaseInQuad().setOnComplete(() =>
        {
            LeanTween.value(0.3f, 0, 0.1f).setOnUpdate((float value) =>
            {
                Color c = bloodyEdge.color;
                c.a = value;
                bloodyEdge.color = c;
            }).setEaseInQuad();
        });
    }
}
