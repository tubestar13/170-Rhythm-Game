using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTest : MonoBehaviour
{
    Vector3 startValue;
    Vector3 endValue;

    VibeCheck vibes;

    // Start is called before the first frame update
    void Start()
    {
        startValue = this.transform.position;
        endValue = new Vector3(0, startValue.y, startValue.z);
        vibes = GetComponentInParent<VibeCheck>();
    }

    void MoveMarker()
    {
        this.transform.position = Vector3.Lerp(startValue, endValue, 1f - ((int)vibes.songPosInBeats + 1 - vibes.songPosInBeats));
    }
    // Update is called once per frame
    void Update()
    {
        MoveMarker();
    }
}
