using System;
using UnityEngine;
using System.Collections;
using UnityEngine.VR;
using UnityEngine.UI;

public class SwitchVRMode : MonoBehaviour
{
    float nextTransition = 5.0f;

    float transitionTime;
    public enum Mode { NOVR, VR, MAX };
    public Mode mode { get; private set; }


    public enum UseVrDevice { NONE, CARDBOARD, DAYDREAM };
    public UseVrDevice useVrDevice { get; private set;  }

    void Awake()
    {
        mode = Mode.VR;
        useVrDevice = UseVrDevice.CARDBOARD;
    }

    // Use this for initialization
    void Start()
    {
        transitionTime = Time.timeSinceLevelLoad + nextTransition;
        StartCoroutine(ChangeMode(mode));

        string[] supportedDevices = VRSettings.supportedDevices;
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator ChangeMode(Mode emode)
    {
        if (mode == Mode.NOVR)
            VRSettings.LoadDeviceByName("none");
        else if (mode == Mode.VR)
        { 
            //If failed loading daydream device, load "cardboard" device.
            VRSettings.LoadDeviceByName("daydream");
            //VRSettings.LoadDeviceByName("cardboard");
        }

        yield return null;

        if (mode == Mode.VR)
        {
            VRSettings.enabled = true;
            if (VRDevice.model.IndexOf("daydream", StringComparison.CurrentCultureIgnoreCase) > -1)
                useVrDevice = UseVrDevice.DAYDREAM;
            else
                useVrDevice = UseVrDevice.CARDBOARD;
        }


#if UNITY_EDITOR
        if (mode == Mode.VR)
            GvrViewer.Instance.VRModeEnabled = true;
        else
            GvrViewer.Instance.VRModeEnabled = false;
#endif
    }
}
