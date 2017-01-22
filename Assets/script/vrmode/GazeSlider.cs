#define EVENTSYSTEM_TIMER
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
        //here !!! , send event
    }
}

