using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager engine;
    [FMODUnity.EventRef]
    public string fmodEvent;

    [SerializeField]
    public float segmentCode = 0.0f;

    private float oldSegmentCode = 0.0f;
    private FMOD.Studio.EventInstance musicLevel0000;
    private FMOD.Studio.PARAMETER_ID segmentCodeParameterID;
    private static EventInstance internalEvent;

    private bool walkCycle = false;
    private static EventInstance instance;

    private void Awake()
    {
        if (engine != null)
        {
            GameObject.Destroy(engine);
        }
        else
        {
            engine = this;
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

    public void OnAttack()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/AUDIO/SFX/SWORD/WOOSH/swordWoosh", GameObject.FindGameObjectWithTag("Player").transform.position);
    }

    void comboAttack()
    {

    }

    void bloodyThrust()
    {

    }

    void hatefulArrow()
    {

    }

    public void usePotion()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/AUDIO/SFX/ITEMS/COMBAT/usePotion", GameObject.FindGameObjectWithTag("Player").transform.position);
    }

    public void walkCyclePlay()
    {
        if (!walkCycle) {
            instance = FMODUnity.RuntimeManager.CreateInstance("event:/AUDIO/SFX/WALK/STEPS/walkCycle");
            instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(GameObject.FindGameObjectWithTag("Player")));
            instance.start();
            walkCycle = true;
        }
    }

    public void walkCycleStop()
    {
        if (walkCycle)
        {
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            walkCycle = false;
        }
    }
}
