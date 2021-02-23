using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTime : MonoBehaviour
{
    public float bpm;
    public float secPerBeat;
    public float songPosition;
    public float songPosInBeats;
    public float dspTimeSong;

    // Start is called before the first frame update
    void Start()
    {
        bpm = 120f;
        secPerBeat = 60f / bpm;

        //record the start time
        dspTimeSong = (float)AudioSettings.dspTime;

        //play the song
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        //calculate the song position
        songPosInBeats = songPosition / secPerBeat;
        songPosition = (float)(AudioSettings.dspTime - dspTimeSong);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key was pressed at time " + songPosition);
            Debug.Log("Space key was pressed at beat " + songPosInBeats);
            if((int)songPosInBeats - songPosInBeats > -.25f && (int)songPosInBeats % 2 == 0)
            {
                Debug.Log("perfect " + ((int)songPosInBeats - songPosInBeats));
            }
            else if ((int)songPosInBeats - songPosInBeats + 1 < .25f && (int)(songPosInBeats + 1) % 2 == 0)
            {
                Debug.Log("perfect " + ((int)songPosInBeats - songPosInBeats + 1));
            }
            else
            {
                Debug.Log("Miss...");
            }
        }
        //Debug.Log(audioSource.time);
    }
}
