using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwitchInput {

    Transform gvrControllerMainTrans;
    Transform gvrControllerPointerTrans;
    Transform gvrGazePointerTrans;
    GvrPointerInputModule gvrPointerInputModule;
    CustomGvrPointerInputModule customGvrPointerInputModule;

    //GvrLaserPointer의 근본적인 문제가 아니다. 
    //Cardboard 관련 input과 Daydream 관련 input을 모두 비활성화 시켜 놓고 실행해야 
    //Cardboard와 Daydream input이 충돌 하지 않는다.

    public void Init(GameObject eventSystem, GameObject gvrControllerMain)
    {
        gvrControllerMainTrans = gvrControllerMain.transform;
        Transform cameraTrans = Camera.main.gameObject.transform;
        gvrControllerPointerTrans = cameraTrans.parent.Find("GvrControllerPointer");
        gvrGazePointerTrans = cameraTrans.Find("CustomReticlePointer");

        CheckNull.LogError("gvrGazePointerTrans is null", gvrGazePointerTrans);
        CheckNull.LogError("gvrControllerPointerTrans is null", gvrControllerPointerTrans);

        Transform laserTrans = gvrControllerPointerTrans.transform.Find("Laser");

        gvrPointerInputModule = eventSystem.GetComponent<GvrPointerInputModule>();
        customGvrPointerInputModule = eventSystem.GetComponent<CustomGvrPointerInputModule>();

        CheckNull.LogError("gvrPointerInputModule is null", gvrPointerInputModule);
        CheckNull.LogError("customGvrPointerInputModule is null", customGvrPointerInputModule);
    }
	
    public void ChangeMode(SwitchVRMode.UseVrDevice useVrDevice)
    {
        gvrControllerMainTrans.gameObject.SetActive(useVrDevice == SwitchVRMode.UseVrDevice.DAYDREAM);

        GameObject[] objects = null;
        try
        {
            objects = GameObject.FindGameObjectsWithTag("ROOT");
        }
        catch (System.Exception exception)
        {
            Debug.Log(exception);
        }

        for (int n = 0; n < objects.Length; ++n)
        {
            GameObject obj = objects[n];

            //TODO: 레이어 이름 가져오기
            //http://tikuma1181.com/HPmain/?p=865
            //if (LayerMask.LayerToName(obj.layer) != "UI")
            if (obj.layer != LayerMask.NameToLayer("UI"))
                continue;

            if (useVrDevice == SwitchVRMode.UseVrDevice.CARDBOARD)
                SetCardboardUI(obj);
            else if (useVrDevice == SwitchVRMode.UseVrDevice.DAYDREAM)
                SetDaydreamUI(obj);
        }

        SetCardboardCursor(useVrDevice == SwitchVRMode.UseVrDevice.CARDBOARD);

        customGvrPointerInputModule.enabled = (useVrDevice == SwitchVRMode.UseVrDevice.CARDBOARD);
        gvrPointerInputModule.enabled = (useVrDevice == SwitchVRMode.UseVrDevice.DAYDREAM);
    }

    void SetDaydreamUI(GameObject obj)
    {
        GvrPointerGraphicRaycaster gvrRaycaster = obj.GetComponent<GvrPointerGraphicRaycaster>();
        GraphicRaycaster graphicRaycaster = obj.GetComponent<GraphicRaycaster>();

        CheckNull.LogError("gvrRaycaster is null", gvrRaycaster);
        CheckNull.LogError("graphicRaycaster is null", graphicRaycaster);

        gvrRaycaster.enabled = true;
        graphicRaycaster.enabled = false;
    }

    void SetCardboardUI(GameObject obj)
    {
        GvrPointerGraphicRaycaster gvrRaycaster = obj.GetComponent<GvrPointerGraphicRaycaster>();
        GraphicRaycaster graphicRaycaster = obj.GetComponent<GraphicRaycaster>();

        CheckNull.LogError("gvrRaycaster is null", gvrRaycaster);
        CheckNull.LogError("graphicRaycaster is null", graphicRaycaster);

        gvrRaycaster.enabled = true;
        graphicRaycaster.enabled = false;
    }

    void SetCardboardCursor(bool isGaze)
    {
        gvrGazePointerTrans.gameObject.SetActive(isGaze);
        gvrControllerPointerTrans.gameObject.SetActive(!isGaze);
    }
}
