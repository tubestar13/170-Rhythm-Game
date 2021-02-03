using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibeCheck : MonoBehaviour
{
    public float bpm;
    public float secPerBeat;
    public float songPosition;
    public float songPosInBeats;
    public float dspTimeSong;

    // Start is called before the first frame update
    void Start()
    {
        bpm = 60f;
        secPerBeat = 60f / bpm;

        //record the start time
        dspTimeSong = (float)AudioSettings.dspTime;

        //play the song
        GetComponent<AudioSource>().Play();
    }

    public float checkVibes()
    {
        if((int)songPosInBeats - songPosInBeats > -.2f)
        {
            //Debug.Log("Good Vibes");
            //Debug.Log("perfect " + ((int)songPosInBeats - songPosInBeats));
            return songPosInBeats;
        }
        else if((int)songPosInBeats - songPosInBeats + 1 < .2f)
        {
            //Debug.Log("Good Vibes");
            //Debug.Log("perfect " + ((int)songPosInBeats - songPosInBeats + 1));
            return songPosInBeats + 1;
        }
        else
        {
            //Debug.Log("You have failed the Vibe Check");
            return -1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspTimeSong);
        songPosInBeats = songPosition / secPerBeat;
    }
}
