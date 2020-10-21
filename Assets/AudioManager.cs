using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager music;

    [FMODUnity.EventRef]
    public string fmodEvent;
    [FMODUnity.EventRef]
    public string fmodEventA;
    [FMODUnity.EventRef]
    public string fmodEventB;
    [FMODUnity.EventRef]
    public string fmodEventC;
    [FMODUnity.EventRef]
    public string fmodEventD;
    [FMODUnity.EventRef]
    public string fmodEventE;
    [FMODUnity.EventRef]
    public string fmodEventF;
    [FMODUnity.EventRef]
    public string fmodEventG;
    [FMODUnity.EventRef]
    public string fmodEventH;

    [SerializeField]
    public float segmentCode = 0.0f;

    private float oldSegmentCode = 0.0f;
    private FMOD.Studio.EventInstance musicLevel0000;
    private FMOD.Studio.PARAMETER_ID segmentCodeParameterID;
    private EventInstance internalEvent;

    private void Awake()
    {
        if (music != null)
        {
            GameObject.Destroy(music);
        }
        else
        {
            music = this;
        }
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        musicLevel0000 = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);

        FMOD.Studio.EventDescription segmentCodeEventDescription;
        musicLevel0000.getDescription(out segmentCodeEventDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION segmentCodeParameterDescription;
        segmentCodeEventDescription.getParameterDescriptionByName("segmentCode", out segmentCodeParameterDescription);
        segmentCodeParameterID = segmentCodeParameterDescription.id;

        musicLevel0000.start();
    }

    void Update()
    {
        if(oldSegmentCode!= segmentCode)
        {
            musicLevel0000.setParameterByID(segmentCodeParameterID, segmentCode);
            oldSegmentCode = segmentCode;
        }
    }

    void OnAttack()
    {
        internalEvent = FMODUnity.RuntimeManager.CreateInstance(fmodEventA);
        internalEvent.start();
    }

    void comboAttack()
    {
        internalEvent = FMODUnity.RuntimeManager.CreateInstance(fmodEventB);
        internalEvent.start();
    }

    void bloodyThrust()
    {
        internalEvent = FMODUnity.RuntimeManager.CreateInstance(fmodEventC);
        internalEvent.start();
    }

    void hatefulArrow()
    {
        internalEvent = FMODUnity.RuntimeManager.CreateInstance(fmodEventD);
        internalEvent.start();
    }

    void usePotion()
    {
        internalEvent = FMODUnity.RuntimeManager.CreateInstance(fmodEventE);
        internalEvent.start();
    }
}
