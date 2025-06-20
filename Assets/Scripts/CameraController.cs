using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
[System.Obsolete]
public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    public int pixelsPerUnit = 32;  // 프로젝트 설정에 맞춰 조정

    private CinemachineVirtualCamera vCam;
    private Transform playerT;
    private float unitsPerPixel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            vCam = GetComponent<CinemachineVirtualCamera>();
            unitsPerPixel = 1f / pixelsPerUnit;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        AssignPlayer();
    }

    void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        AssignPlayer();
    }

    void AssignPlayer()
    {
        var p = GameObject.FindWithTag("Player");
        if (p)
        {
            playerT = p.transform;
            vCam.Follow = playerT;
            vCam.LookAt = playerT;
        }
    }

    void LateUpdate()
    {
        if (vCam.Follow == null) return;
        // 픽셀 단위 스냅으로 흐림 해소
        Vector3 pos = vCam.transform.position;
        pos.x = Mathf.Round(pos.x / unitsPerPixel) * unitsPerPixel;
        pos.y = Mathf.Round(pos.y / unitsPerPixel) * unitsPerPixel;
        vCam.transform.position = pos;
    }
}
