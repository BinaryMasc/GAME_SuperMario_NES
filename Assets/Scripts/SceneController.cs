using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SceneController : MonoBehaviour
{

    [SerializeField] private AudioSource Audio;

#pragma warning disable //  Add manually in editor 
    [SerializeField] private AudioClip AudioLevel;  
    [SerializeField] private AudioClip AudioPause;
    [SerializeField] private GameObject MainCamera; //  Handle the main camera
    [SerializeField] private GameObject Player; //  Handle the Player
#pragma warning enable

    //

    // internal vars: (time in secs)
    private float timeElapsedWhenPauseStart; // elapsed time when starting pause
    private float timePauseTemp;
    private float timePauseElapsed;

    float distanceCameraPlayer;

    private bool pauseTempFlag;

    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();

        Audio.clip = AudioLevel;
        Audio.Play();

        timePauseElapsed = 0;
        pauseTempFlag = false;

        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) Pause();
        if (pauseTempFlag) PauseTemp();
        distanceCameraPlayer = MainCamera.transform.position.x - Player.transform.position.x;
        Debug.Log(distanceCameraPlayer);
        if (distanceCameraPlayer < 0)
        {
            MainCamera.transform.position = new Vector3(Player.transform.position.x, MainCamera.transform.position.y, -10);
        }
    }

    //  Undefined pause
    public void Pause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            Audio.clip = AudioPause;
            Audio.Play();
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            Audio.clip = AudioLevel;
            Audio.Play();
        }
        
    }

    //  Pause temp: [time] in seconds
    private void PauseTemp()
    {
        
        timePauseElapsed += Time.realtimeSinceStartup - timeElapsedWhenPauseStart;

        if(timePauseElapsed >= timePauseTemp)
        {
            Time.timeScale = 1;
            //
            timePauseTemp = 0;
            pauseTempFlag = false;
        }
    }

    public void PauseTemp(float time)
    {
        Time.timeScale = 0;

        timePauseTemp = time;
        timeElapsedWhenPauseStart = Time.realtimeSinceStartup;
        timePauseElapsed = 0;
        pauseTempFlag = true;
    }
}
