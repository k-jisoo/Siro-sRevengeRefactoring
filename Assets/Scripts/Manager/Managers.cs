using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers instance;
    public static Managers Instance { get { return instance; } }

    private ResourceManager resource = new ResourceManager(); // °´Ã¼È­ »ý¼º
    public static ResourceManager Resource { get { return Instance.resource; } }

    private SoundManager sound;
    public static SoundManager Sound { get { return Instance.sound; } }

    private SkillEffectVolumeManager skillEffectVolume;
    public static SkillEffectVolumeManager SkillEffectVolume { get { return Instance.skillEffectVolume; } }

    private UIManager ui;
    public static UIManager UI { get { return Instance.ui; } }

    private CameraManager cameraManager;
    public static CameraManager CameraManager { get { return Instance.cameraManager; } }

    private StageManager stageManager;
    public static StageManager StageManager { get { return Instance.stageManager; } }



    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        MangersInit();
    }

    private void MangersInit()
    {
        stageManager = GetComponentInChildren<StageManager>();
        sound = GetComponentInChildren<SoundManager>();
        skillEffectVolume = GetComponentInChildren<SkillEffectVolumeManager>();
        cameraManager = GetComponentInChildren<CameraManager>();
        ui = GetComponentInChildren<UIManager>();
    }
}
