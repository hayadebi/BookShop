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
    // �ܓx�o�x��񂪎擾�\��
    public bool CanGetLonLat()
    {
        return Input.location.isEnabledByUser;
    }

    void OnGUI()
    {
#if PLATFORM_ANDROID
        if(!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation)&&!CanGetLonLat())
        {
            //���Ȃ����}�C�N���g�p���錠�������ۂ��܂����B
           //*�ʒu���̎擾�́A�T�[�r�X�ŉ��o����K�v���������K�Ɏ��������邽�߂ɕK�v�Ƃ��܂��B
           //*���������ꍇ�͊֘A�T�[�r�X�̂����p��A������������Ƃ̃g���u���Ԃɉ���A�T�|�[�g�����Â炭�Ȃ�܂��B
         dialog.AddComponent<PermissionsRationaleDialog>();
            return;
        }
        else if(dialog != null)
        {
            Destroy(dialog);
        }
#endif
        isuse = true;
        //����ŁA�ʒu�����g����Ƃ��s�����Ƃ��ł��܂��B
    }
}