using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CheckNull
{
    static public bool LogError<T>(string msg, T obj)
    {
        if (obj == null)
        {
            Debug.LogError(msg);
            return false;
        }

        return true;
    }
}

public class UtilIO
{
    public static string[] GetFiles(List<string> extensionsToCompare, string Location)
    {
        List<string> files = new List<string>();
        foreach (string file in Directory.GetFiles(Location))
        {
            if (extensionsToCompare.Contains(file.Substring(file.LastIndexOf('.') + 1).ToLower()))
                files.Add(file);
        }
        files.Sort();
        return files.ToArray();
    }
}

public static class UtilCamera
{

    public static float GetCameraWdith(float fieldOfView, float cameraAspect, float distance)
    {
        var frustumHeight = 2.0f * distance * Mathf.Tan(fieldOfView * 0.5f * Mathf.Deg2Rad);
        var frustumWidth = frustumHeight * cameraAspect;
        return frustumWidth;
    }

    public static void GetCameraWdith(Camera camera, float distance, out float frustumWidth, out float frustumHeight)
    {
        frustumHeight = 2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumWidth = frustumHeight * camera.aspect;
    }
}

public static class UtilMath
{
    public static float GetAngle(Vector3 fwd, Vector3 targetDir)
    {
        float angle = Vector3.Angle(fwd, targetDir);

        if (AngleDir(fwd, targetDir, Vector3.up) == -1)
        {
            angle = 360.0f - angle;
            if (angle > 359.9999f)
                angle -= 360.0f;
            return angle;
        }
        else
            return angle;
    }

    public static int AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0.0)
            return 1;
        else if (dir < 0.0)
            return -1;
        else
            return 0;
    }

    public static void Swap<t>(ref t x, ref t y)
    {

        t tempswap = x;
        x = y;
        y = tempswap;
    }
}

public static class UtilObject
{
    public static T FindComponentInChildWithTag<T>(GameObject parent, string tag) where T : Component
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.GetComponent<T>();
            }
            else
            {
                //tr.GetComponent<T>().FindComponentInChildWithTag(tag);
                UtilObject.FindComponentInChildWithTag<T>(tr.gameObject, tag);
            }
        }

        return null;
    }

    public static void FindChildWithTag<T>(Transform t, string tag, List<Transform> list) where T : Component
    {
        for(int n = 0; n < t.childCount; ++n)
        {
            Transform tr = t.GetChild(n);
            if (tr.tag == tag)
            {
                list.Add(tr);
            }
            else
            {
                UtilObject.FindChildWithTag<T>(tr, tag, list);
            }
        }
    }


    static public T GetComponentWithTag<T>(string tag, string objName)
    {
        T component = default(T);
        GameObject obj = FindGameObjectWithTag(tag, objName);
        if (obj == null)
            return component;
        component = obj.GetComponent<T>();
        return component;
    }

    static public GameObject FindGameObjectWithTag(string tag, string objName)
    {
        GameObject[] objects = null;
        try
        {
            objects = GameObject.FindGameObjectsWithTag(tag);
        }
        catch (System.Exception exception)
        {
            Debug.Log(exception);
        }

        if (objects == null)
            return null;

        GameObject obj = UtilObject.FindGameObjectFromArray(objects, objName);
        return obj;
    }

    static public GameObject FindGameObjectFromArray(GameObject[] arr, string name)
    {
        GameObject retObj = null;

        for (int n = 0; n < arr.Length; ++n)
        {
            if (arr[n].name == name)
            {
                retObj = arr[n];
                break;
            }
        }

        return retObj;
    }

    static public GameObject FindGameObject(GameObject parent, string name)
    {
        GameObject obj = parent;

        char[] sep = new char[] { '/' };
        string[] words = name.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);

        for (int n = 0; n < words.Length; ++n)
        {
            obj = SubFindChild(obj, words[n]);
            if (obj == null)
                break;
        };

        return obj;
    }

    static GameObject SubFindChild(GameObject parent, string name)
    {
        Transform[] trans = parent.GetComponentsInChildren<Transform>(true);
        for (int n = 0; n < trans.Length; ++n)
        {
            if (trans[n].name == name)
                return trans[n].gameObject;
        }
        return null;
    }
}
