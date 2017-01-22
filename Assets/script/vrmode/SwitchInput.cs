using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwitchInput {

    Transform gvrControllerPointerTrans;
    Transform gvrGazePointerTrans;

    //GvrLaserPointer가 활성화 되어 있으면 GvrReticlePointer가 정상적으로 동작하지 않는다.
    //GvrReticlePointer를 실행할때는 GvrLaserPointer를 디즈블시킨다.
    GvrLaserPointer gvrLaserPointer;

    public void Init()
    { 
        Transform cameraTrans = Camera.main.gameObject.transform;
        gvrControllerPointerTrans = cameraTrans.parent.Find("GvrControllerPointer");
        gvrGazePointerTrans = cameraTrans.Find("CustomReticlePointer");

        CheckNull.LogError("gvrGazePointerTrans is null", gvrGazePointerTrans);
        CheckNull.LogError("gvrControllerPointerTrans is null", gvrControllerPointerTrans);

        Transform laserTrans = gvrControllerPointerTrans.transform.Find("Laser");
        if (laserTrans)
            gvrLaserPointer = laserTrans.GetComponent<GvrLaserPointer>();
        CheckNull.LogError("gvrLaserPointer is null", gvrLaserPointer);
    }
	
    public void ChangeCardboardMode()
    {
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

            SetCardboardUI(obj);
        }

        SetCardboardCursor();
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

    void SetCardboardCursor()
    {
        gvrGazePointerTrans.gameObject.SetActive(true);
        gvrControllerPointerTrans.gameObject.SetActive(false);
        gvrLaserPointer.enabled = false;
    }
}
