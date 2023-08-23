using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class clickbutton : MonoBehaviour
{
    public GameObject target_obj;
    public GameObject target_set = null;
    public UserManager umanager;
    private bool tmptrg = false;
    private bool istrgcheck = false;
    // Start is called before the first frame update
    void Start()
    {
        tmptrg = false;
        istrgcheck = false;
    }
    public void SetOnView()
    {
        GManager.instance.setrg = 0;
        target_obj.SetActive(true);
        if (umanager != null)
        {
            StartCoroutine(IsPermission());
            umanager.signUp();
        }
        if (target_set != null) target_set.SetActive(false);
    }
    public void SetNotView()
    {
        GManager.instance.setrg = 0;
        if (target_set != null) target_set.SetActive(true);
        if (umanager != null)
        {
            StartCoroutine(IsPermission());
            umanager.signUp();
        }
        target_obj.SetActive(false);
    }

    public void quitClick()
    {
        Application.Quit();
    }
    public IEnumerator IsPermission()
    {
        if (!istrgcheck)
        {
            istrgcheck = true;
            try
            {
                tmptrg = CheckPermission("android.permission.ACCESS_FINE_LOCATION");
            }
            catch (System.Exception e)
            {
                try
                {
                    print("error:" + e.ToString());
                    tmptrg = CheckPermission("android.permission.ACCESS_COARSE_LOCATION");
                }
                catch (System.Exception _e)
                {
                    print("error:" + _e.ToString());
                    tmptrg = true;
                }
                finally
                {
                    tmptrg = true;
                }
            }
        }
        yield return new WaitUntil(() => tmptrg == true);
    }
    private bool CheckPermission(string permission)
    {
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        using (var compat = new AndroidJavaClass("android.support.v4.app.ActivityCompat"))
        {
            var check = compat.CallStatic<int>("checkSelfPermission", activity, permission);

            if (check == 0) return true;

            int REQUEST_CODE = 1;
            compat.CallStatic("requestPermissions", activity, new String[] {
                    permission
                }, REQUEST_CODE);

            //再チェック
            check = compat.CallStatic<int>("checkSelfPermission", activity, permission);
            if (check == 0) return true;

            // "設定からパーミッションを許可してください。機能が使用できません。";
        }
        return false;
    }
}

