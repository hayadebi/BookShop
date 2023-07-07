using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;

public class GPSManager : MonoBehaviour
{
    public double user_latitude=-9999999;
    public double user_longitude=-9999999;
    public bool getgpstrg = false;
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
}