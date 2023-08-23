using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;
# if PLATFORM_ANDROID
using UnityEngine.Android;
# endif

public class GPSManager : MonoBehaviour
{
    public double user_latitude=-9999999;
    public double user_longitude=-9999999;
    public bool getgpstrg = false;
    public bool isuse = false;
    GameObject dialog = null;
    [DllImport("__Internal")]
    private static extern void GetCurrentPosition();
    public void GetUserGPS()
    {
        Resources.UnloadUnusedAssets();
        GetCurrentPosition();
    }
    public void ShowLocation(string location)
    {
        string[] locations = location.Split(',');
        double latitude = double.Parse(locations[0]);
        double longitude = double.Parse(locations[1]);
        user_latitude = latitude;
        user_longitude = longitude;
        getgpstrg = true;
    }
    private void Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation))
        {
            Permission.RequestUserPermission(Permission.CoarseLocation);
            dialog = new GameObject();
            }
#endif
        if (CanGetLonLat() && !isuse) isuse = true;
    }
    // 緯度経度情報が取得可能か
    public bool CanGetLonLat()
    {
        return Input.location.isEnabledByUser;
    }

    void OnGUI()
    {
#if PLATFORM_ANDROID
        if(!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation)&&!CanGetLonLat())
        {
            //あなたがマイクを使用する権限を拒否しました。
           //*位置情報の取得は、サービスで遠出する必要が無く快適に取り引きするために必要とします。
           //*許可が無い場合は関連サービスのご利用や、取り引き相手方とのトラブル間に介入、サポートをしづらくなります。
         dialog.AddComponent<PermissionsRationaleDialog>();
            return;
        }
        else if(dialog != null)
        {
            Destroy(dialog);
        }
#endif
        isuse = true;
        //これで、位置情報を使う作業を行うことができます。
    }
}