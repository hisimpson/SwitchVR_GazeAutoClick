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
        //daydream과 cardboard를 에디터에서 테스트 할려면 여기만 설정하면 된다.
#if UNITY_EDITOR
        mode = Mode.VR;
        useVrDevice = UseVrDevice.CARDBOARD;
        //useVrDevice = UseVrDevice.DAYDREAM;
#endif
    }

    // Use this for initialization
    void Start()
    {
        transitionTime = Time.timeSinceLevelLoad + nextTransition;
        //yield return StartCoroutine(ChangeMode(mode));
        
        string[] supportedDevices = VRSettings.supportedDevices;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DetectMode()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (string.IsNullOrEmpty(VRDevice.model) == true)
                mode = Mode.NOVR;
            else
            {
                mode = Mode.VR;
                if (VRDevice.model.IndexOf("daydream", StringComparison.CurrentCultureIgnoreCase) > -1)
                    useVrDevice = UseVrDevice.DAYDREAM;
                else
                    useVrDevice = UseVrDevice.CARDBOARD;
            }
        }
    }

    public IEnumerator ChangeDeviceMode(Mode emode)
    {
        if (mode == Mode.NOVR)
        {
            Debug.Log("Change Mode: none Device");
            VRSettings.LoadDeviceByName("none");
        }
        else if (mode == Mode.VR)
        {
            Debug.Log("Change Mode: VR Device");
            //If failed loading daydream device, load "cardboard" device.
            VRSettings.LoadDeviceByName("daydream");
            //VRSettings.LoadDeviceByName("cardboard");
        }

        yield return null;

#if UNITY_EDITOR
        if (mode == Mode.VR)
            GvrViewer.Instance.VRModeEnabled = true;
        else
            GvrViewer.Instance.VRModeEnabled = false;
#else
        if (mode == Mode.VR)
        {
            VRSettings.enabled = true;
            if (VRDevice.model.IndexOf("daydream", StringComparison.CurrentCultureIgnoreCase) > -1)
            {
                Debug.Log("Change Mode: VR daydream Device");
                useVrDevice = UseVrDevice.DAYDREAM;
            }   
            else
            {
                Debug.Log("Change Mode: VR cardboard Device");
                useVrDevice = UseVrDevice.CARDBOARD;
            }
        }
#endif
    }
}
