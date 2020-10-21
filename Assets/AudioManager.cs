using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager music;

    [FMODUnity.EventRef]
    public string fmodEvent;

    [SerializeField]
    public float segmentCode = 0.0f;

    private float oldSegmentCode = 0.0f;
    private FMOD.Studio.EventInstance musicLevel0000;
    private FMOD.Studio.PARAMETER_ID segmentCodeParameterID;

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

}
