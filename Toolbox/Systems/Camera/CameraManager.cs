using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    private CinemachineBrain brain;
    private CinemachineCamera currentCam;
    private float defaultBlendTime;

    private void Start()
    {
        RefreshReferences();
        SceneInfoManager.Instance.OnSceneInitialized += RefreshReferences;
        //SceneChanger.OnSceneLoaded += RefreshReferences;
    }

    //private void OnDestroy() => SceneChanger.OnSceneLoaded -= RefreshReferences;
    private void OnDestroy() => SceneInfoManager.Instance.OnSceneInitialized -= RefreshReferences;

    private void RefreshReferences()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
        if (!brain) return;
        currentCam = brain.ActiveVirtualCamera as CinemachineCamera;
        defaultBlendTime = brain.DefaultBlend.Time;
    }

    public void SetCamera(CinemachineCamera vcam, float blendTime = -1)
    {
        if (currentCam == vcam) return;

        if (blendTime == -1) blendTime = defaultBlendTime;
        brain.DefaultBlend.Time = blendTime;

        if (currentCam) currentCam.enabled = false;
        currentCam = vcam;
        currentCam.enabled = true;
    }

    public CinemachineCamera GetCurrentCamera() => currentCam;
}
