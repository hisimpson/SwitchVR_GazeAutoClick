using UnityEngine;
using System.Collections;

public class MainApp : MonoBehaviour {
    [SerializeField]
    SwitchVRMode switchMode;
    [SerializeField]
    GameObject eventSystem;
    [SerializeField]
    GameObject gvrControllerMain;

    SwitchInput switchInput = new SwitchInput();

    // Use this for initialization
    void Start () {
        switchInput.Init(eventSystem, gvrControllerMain);
        CheckNull.LogError("switchMode is null", switchMode);
        CheckNull.LogError("switchInput is null", switchInput);

#if UNITY_EDITOR
        StartCoroutine(SwitchVrDevice(switchMode.mode));
#else
        //daydream에서 시작했는지, none으로 시작했는지, Cardboar로 시작했는지 검사한다.
        //앱 최초 실행시 모드로 실행하기 때문에 SwitchVrDevice() 함수는 실행하지 않는다.
        //버튼에 의해 모드를 전환 할때는 SwitchVrDevice() 함수를 실행한다.
        switchMode.DetectMode();

        if (switchMode.mode == SwitchVRMode.Mode.VR)
        {
            switchInput.ChangeMode(switchMode.useVrDevice);
        }
#endif
    }

    public void ClickButton () {
        Debug.Log("Click Button !!!!");
	}

    IEnumerator SwitchVrDevice(SwitchVRMode.Mode mode)
    {
        yield return StartCoroutine(switchMode.ChangeDeviceMode(switchMode.mode));

        if (switchMode.mode == SwitchVRMode.Mode.VR)
        {
            switchInput.ChangeMode(switchMode.useVrDevice);
        }
    }
}
