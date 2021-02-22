using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VibeCheck : MonoBehaviour
{
    FMODVibeTracker vt;
    public float bpm;
    public float secPerBeat;
    public float songPosition;
    public float songPosInBeats;

    enum SongState { STARTED, RUNNING, STOPPING };
    SongState state;

    // Start is called before the first frame update
    void Start()
    {
        vt = GetComponent<FMODVibeTracker>();
        state = SongState.STARTED;
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
        //song position is in milliseconds
        songPosition = vt.timelineInfo.position;
        //divide by 1000 to get proper beat count
        songPosInBeats = songPosition / secPerBeat / 1000;
        bpm = vt.timelineInfo.tempo;
        secPerBeat = 60f / bpm;

        if(state == SongState.STARTED && songPosition > 0)
        {
            state = SongState.RUNNING;
        }

        if(state == SongState.RUNNING && songPosition == 0)
        {
            Debug.Log("Song Ended");
            state = SongState.STOPPING;
            SceneManager.LoadScene(2);
        }
    }

}
