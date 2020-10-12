using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager MM;

    public AudioSource background;
    public AudioSource battleMusic;
    private bool onBattle;


    // Start is called before the first frame update
    void Start()
    {
        onBattle = false;   
    }

    private void Awake()
    {
        if (MM != null) //Si por algún motivo ya existe un combatManager...
        {
            GameObject.Destroy(MM); //Este script lo mata. Solo puede haber una abeja reina en la colmena.
        }
        else //En caso de que el trono esté libre...
        {
            MM = this; //Lo toma para ella!
        }

        DontDestroyOnLoad(this); //Ah, y no destruyas esto al cargar
    }
    // Update is called once per frame
    void Update()
    {
        if(CombatManager.CM.onCombat && !onBattle)
        {
            background.volume = 0.01f;
            battleMusic.Play();
            battleMusic.volume = 0.015f;
            onBattle = true;
        } 
        else if ((!CombatManager.CM.onCombat && onBattle))
        {
            background.volume = 0.02f;
            fadeOut(battleMusic);
        }
    }

    IEnumerator soundLerp(float from, float to, float speed)
    {
        float startTime = Time.unscaledTime;

        while (Time.timeScale != to)
        {
            float tempTime = Mathf.Lerp(from, to, (Time.unscaledTime - startTime) * speed);
            Time.timeScale = tempTime;
            yield return null;
        }

        Time.fixedDeltaTime = Time.timeScale * .2f; //Cambiamos el fixedDeltaTime para que las fisicas tambien adopten la camara lenta.
        //Se arreglará solo al salir del slowMo gracias a que en el Update, si no estamos en slowMo, lo normalizamos por la fuerza cada frame.
        //Un poco guarro, pero funciona.

    }

    void fadeOut(AudioSource song)
    {
        float audioVolume = song.volume;

        if (audioVolume > 0)
        {
            audioVolume -= 0.01f * Time.deltaTime;
            song.volume = audioVolume;
        }

        if (song.volume <= 0)
            onBattle = false;

    }
}
