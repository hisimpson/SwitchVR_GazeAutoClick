#define EVENTSYSTEM_TIMER
//#define GENERATE_EVNET
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GazeSlider : MonoBehaviour {

    Image fillImage;
    bool init { get; set; }

    bool gazedAt;
    float fillTime = 2f;
    float timer;

    Coroutine fillBarRoutine;

    GameObject target;
    GameObject controlTarget;

    // Use this for initialization
    void Start () {
        Transform fillTrans = transform.Find("fill");
        CheckNull.LogError<Transform>("fillTrans is null", fillTrans);
        fillImage = fillTrans.GetComponent<Image>();
        CheckNull.LogError<Image>("fillImage is null", fillImage);

        init = true;

#if EVENTSYSTEM_TIMER
        fillTime = CustomGvrPointerInputModule.selectTime;
#endif

        OnEnable();
    }

    public float Amount
    {
        get
        {
            return fillImage.fillAmount;
        }

        set
        {
            fillImage.fillAmount = value;
        }
    }

    public void SetTarget(GameObject obj)
    {
#if !GENERATE_EVNET
        target = obj;
#else
        if (obj == null)
        {
            target = null;
            controlTarget = null;
            return;
        }

        if (target == obj)
            return;

        target = obj;

        controlTarget = null;
        Transform trans = target.transform;
        GameObject o;
        while (trans)
        {
            o = trans.gameObject;
            if (o.GetComponent<Canvas>())
                return;

            if (o.GetComponent<Toggle>() || o.GetComponent<Button>())
            {
                controlTarget = o;
                return;
            }

            if (o.GetComponent<Slider>())
            {
                controlTarget = o;
                return;
            }

            trans = trans.parent;
        }
#endif
    }

    public void OnEnable()
    {
        if (init == false)
            return;

        gazedAt = true;
        Amount = 0;
        fillBarRoutine = StartCoroutine(FillBar());
    }

    public void OnDisable()
    {
        gazedAt = false;
        if (fillBarRoutine != null)
            StopCoroutine(fillBarRoutine);

        timer = 0f;
        Amount = 0f;
        target = null;
        controlTarget = null;
    }

    IEnumerator FillBar()
    {
        timer = 0f;

        while (timer < fillTime)
        {
#if EVENTSYSTEM_TIMER
            timer = (fillTime - CustomGvrPointerInputModule.countTimer);
#else
            timer += Time.deltaTime;
#endif
            Amount = timer / fillTime;
            yield return null;

            if (gazedAt == true)
                continue;

            timer = 0f;
            yield break;
        }

#if GENERATE_EVENT
        OnGenerateEvent();
#endif
    }

#if GENERATE_EVENT
    void OnGenerateEvent()
    {
        if (controlTarget == null)
            return;

        ExecuteEvents.ExecuteHierarchy(controlTarget, new PointerEventData(EventSystem.current),
            ExecuteEvents.pointerDownHandler);
        ExecuteEvents.Execute<IPointerUpHandler>(controlTarget, new PointerEventData(EventSystem.current),
            ExecuteEvents.pointerUpHandler);
        ExecuteEvents.Execute<IPointerClickHandler>(controlTarget, new PointerEventData(EventSystem.current), 
            ExecuteEvents.pointerClickHandler);
    }
#endif

    //GvrPointerInputModule.cs 이벤트 발생과 비슷하게 했지만 슬라이더는 Gaze 입력이 제대로 안됨
#if false
    void OnButtonEvent()
    {
        if (controlTarget == null)
            return;

        ExecuteEvents.Execute(controlTarget, new PointerEventData(EventSystem.current),
                GvrExecuteEventsExtension.pointerHoverHandler);

        ExecuteEvents.ExecuteHierarchy(controlTarget, new PointerEventData(EventSystem.current),
            ExecuteEvents.pointerDownHandler);

        //ExecuteEvents.Execute(controlTarget, new PointerEventData(EventSystem.current), ExecuteEvents.initializePotentialDrag);

        ExecuteEvents.Execute(controlTarget, new PointerEventData(EventSystem.current),
                ExecuteEvents.updateSelectedHandler);


        ExecuteEvents.Execute<IPointerUpHandler>(controlTarget, new PointerEventData(EventSystem.current),
            ExecuteEvents.pointerUpHandler);
        ExecuteEvents.Execute<IPointerClickHandler>(controlTarget, new PointerEventData(EventSystem.current), 
            ExecuteEvents.pointerClickHandler);
    }
#endif
}

