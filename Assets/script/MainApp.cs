using UnityEngine;
using System.Collections;

public class MainApp : MonoBehaviour {
    [SerializeField]
    SwitchVRMode switchMode;


    SwitchInput switchInput = new SwitchInput();

    // Use this for initialization
    void Start () {
        switchInput.Init();
        CheckNull.LogError("switchMode is null", switchMode);
        CheckNull.LogError("switchInput is null", switchInput);

        if (switchMode.mode == SwitchVRMode.Mode.VR && 
            switchMode.useVrDevice == SwitchVRMode.UseVrDevice.CARDBOARD)
        {
            switchInput.ChangeCardboardMode();
        }
    }
	
	public void ClickButton () {
        Debug.Log("Click Button !!!!");
	}
}
