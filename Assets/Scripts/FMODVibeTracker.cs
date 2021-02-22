using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class FMODVibeTracker : MonoBehaviour
{
    //Variables that are modified in the callback need to be part of a separate class.
    //This class needs to be 'blittable' otherwise it can't be pinned in memory.

    //Specifies that the fields of the type should be laid out in memory in the same order they are declared
    //C# structs implicitly have sequential layout.
    [StructLayout(LayoutKind.Sequential)] 
    public class TimelineInfo
    {
        public int currentBar = 0;
        public int currentBeat = 0;
        public int position = 0;
        public float tempo = 0;
        public int timeSignatureUpper = 0;
        public int timeSignatureLower = 0;
        public float songLength = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }
    
    public string music = "event:/MainTheme";

    public TimelineInfo timelineInfo;
    GCHandle timelineHandle;

    FMOD.Studio.EVENT_CALLBACK beatCallback;
    FMOD.Studio.EventDescription descriptionCallback;

    public static FMOD.Studio.EventInstance musicInstance;

    //THE SINGLETON HAS APPEARED: This allows for everything to access this SINGLE INSTANCE of the script and access its variables
    //The drawback is that if this script ever gets destroyed, it's gonna mean everything that relies on the information provided
    //by this script is gonna break.
    //public static FMODVibeTracker vt;

    /*
     * void Awake()
     * {
     *     vt = this;
     * }
     */
    // Start is called before the first frame update
    void Start()
    {
        timelineInfo = new TimelineInfo();

        //Explicitly create the delegate object and assign it to a member so it doesn't get freed
        //by the garbage collected while it's being used
        beatCallback =  new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);

        musicInstance = FMODUnity.RuntimeManager.CreateInstance(music);

        //Pin the class that will store the data modified during the callback
        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        //Pass the object through the userdata of the instance
        musicInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));

        musicInstance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        musicInstance.start();

        musicInstance.getDescription(out descriptionCallback);
        descriptionCallback.getLength(out int length);

        timelineInfo.songLength = length;
    }

    // Update is called once per frame
    void Update()
    {
        musicInstance.getTimelinePosition(out timelineInfo.position);
    }

    private void OnDestroy()
    {
        musicInstance.setUserData(IntPtr.Zero);
        musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicInstance.release();
        timelineHandle.Free();
    }

    //private void OnGUI()
    //{
    //    GUILayout.Box(String.Format("Current Beat = {0}, Last Marker = {1}, Position = {2}, Tempo = {3}",
    //        timelineInfo.currentBeat, (string)timelineInfo.lastMarker, timelineInfo.position, timelineInfo.tempo));
    //}

    //Attribute tag indicates that the function should compile ahead of time, with MonoPInvokeCallback
    //noting that it will handle unmanaged items of type EVENT_CALLBACK
    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

        // Retrieve the user data
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);

        if (result != FMOD.RESULT.OK)
        {
            Debug.Log("Timeline Callback error: " + result);
        }

        else if (timelineInfoPtr != IntPtr.Zero)
        {
            //Get the object to store beat and marker details
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.currentBar = parameter.bar;
                        timelineInfo.currentBeat = parameter.beat;
                        timelineInfo.tempo = parameter.tempo;
                        timelineInfo.timeSignatureUpper = parameter.timesignatureupper;
                        timelineInfo.timeSignatureLower = parameter.timesignaturelower;
                    }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.lastMarker = parameter.name;
                    }
                    break;
            }
        }
        return FMOD.RESULT.OK;
    }
}
