using UnityEngine;
using System.Collections;
using NCMB;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
public class UserManager : MonoBehaviour
{
    public static UserManager instance = null;
    [Header("���[�U�[��")]
    public InputField currentUserName;
    [Header("Gmail�A�h���X")]
    public InputField mail;
    [Header("Gmail�p�X���[�h")]
    public InputField pw;
    [Header("�p�X���[�h�Ċm�F�p")]
    public InputField re_pw;
    //public InputField mpurse_id;

    [Header("���O�C����Ԋm�F�p")]
    public Text check_loginmode;
    public GPSManager gpsmanager;
    public NCMBObject userobj;
    public GameObject mainUI;
    public GameObject loginUI;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // mobile backend�ɐڑ����ă��O�C�� ------------------------
    private void Start()
    {
        //�����Ԉȓ��Ƀ��O�C���������Ƃ�����Ȃ玩�����O�C��
        if (NCMBUser.CurrentUser != null) 
        {
            NCMBUser currentUser = NCMBUser.CurrentUser;
            print(currentUser.UserName);
            check_loginmode.text = "��ԁF���O�C������";
            StartCoroutine(nameof(SaveUserGPS));
            mainUI.SetActive(true);
            loginUI.SetActive(false);
        }
    }
    public void logIn()
    {
        if (mail.text != "" && pw.text != "") {
            NCMBUser.LogInAsync(currentUserName.text, pw.text, (NCMBException e) => {
                if (e != null)
                {
                    UnityEngine.Debug.Log("�����O�C���Ɏ��s: " + e.ErrorMessage);
                    check_loginmode.text = "��ԁF���O�C�����s";
                }
                else
                {
                    UnityEngine.Debug.Log("�����O�C���ɐ����I");
                    check_loginmode.text = "��ԁF���O�C������";
                    StartCoroutine(nameof(SaveUserGPS));
                    mainUI.SetActive(true);
                    loginUI.SetActive(false);
                }
            });
        }
        else UnityEngine.Debug.Log("�����[���A�h���X���p�X���[�h�������Ɠ��͂���Ă��܂���");
    }

    // mobile backend�ɐڑ����ĐV�K����o�^ ------------------------

    public void signUp()
    {
        if (pw.text == re_pw.text)
        {
            NCMBUser user = new NCMBUser();
            if (currentUserName.text == "") currentUserName.text = "anonymous user." + DateTime.Now.ToString();
            user.UserName = currentUserName.text;
            user.Email = mail.text;
            user.Password = pw.text;
            user.SignUpAsync((NCMBException e) =>
            {
                if (e != null)
                {
                    UnityEngine.Debug.Log("���[�U�[�̐V�K�o�^�Ɏ��s: " + e.ErrorMessage);
                    check_loginmode.text = "��ԁF�V�K�o�^�Ɏ��s";
                }
                else
                {
                    UnityEngine.Debug.Log("���[�U�[�̐V�K�o�^�ɐ���");
                    check_loginmode.text = "��ԁF�V�K�o�^�ɐ���";
                }
            });
        }
        else UnityEngine.Debug.Log("���p�X���[�h����v���Ȃ����A�܂��̓��[���A�h���X�������͂���Ă��܂���");
    }

    // mobile backend�ɐڑ����ă��O�A�E�g ------------------------

    public void logOut()
    {

        NCMBUser.LogOutAsync((NCMBException e) => {
            if (e == null)
            {
                ;
            }
        });
    }
    public IEnumerator SaveUserGPS()
    {
        gpsmanager.GetUserGPS();
        yield return new WaitUntil(() => gpsmanager.getgpstrg);
        DateGPS();
    }
    void DateGPS()
    {
        NCMBQuery<NCMBObject> query = null;
        query = new NCMBQuery<NCMBObject>("UserGPS");
        NCMBUser currentUser = NCMBUser.CurrentUser;
        if (currentUser != null)
        {
            UnityEngine.Debug.Log("���O�C�����̃��[�U�[: " + currentUser.UserName);
            query.OrderByDescending(currentUser.UserName);
            //����������ݒ�
            query.Limit = 4;
            //�f�[�^�X�g�A�ł̌������s��
            query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
            {
                if (e != null)
                {
                    NCMBObject obj = new NCMBObject("UserGPS");
                    obj.Add("username", currentUser.UserName);
                    obj.Add("login_latitude", gpsmanager.user_latitude);
                    obj.Add("login_longitude", gpsmanager.user_longitude);
                    obj.SaveAsync((NCMBException e) => {
                        if (e != null)
                        {
                            UnityEngine.Debug.Log("�ʒu�����f�[�^�X�g�A�֑��M���s���܂���");
                        }
                        else
                        {
                            UnityEngine.Debug.Log("�ʒu�����f�[�^�X�g�A�֑��M�������܂���");
                        }
                    });
                }
                else
                {
                    foreach (NCMBObject obj in objList)
                    {
                        userobj = obj;
                        NCMBObject tmpobj = userobj;
                        tmpobj["login_latitude"] = gpsmanager.user_latitude;
                        tmpobj["olgin_longitude"] = gpsmanager.user_longitude;
                        break;
                    }
                }
            });
        }
        else
        {
            NCMBObject obj = new NCMBObject("UserGPS");
            obj.Add("username", currentUser.UserName);
            obj.Add("login_latitude", gpsmanager.user_latitude);
            obj.Add("login_longitude", gpsmanager.user_longitude);
            obj.SaveAsync((NCMBException e) => {
                if (e != null)
                {
                    UnityEngine.Debug.Log("�ʒu�����f�[�^�X�g�A�֑��M���s���܂���");
                }
                else
                {
                    UnityEngine.Debug.Log("�ʒu�����f�[�^�X�g�A�֑��M�������܂���");
                    NCMBUser user = new NCMBUser();
                    user.UserName = currentUserName.text;
                }
            });
        }
    }
}