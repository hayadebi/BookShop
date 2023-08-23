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
    [Header("�閧�̌��t")]
    public InputField secret;
    [Header("�p�X���[�h")]
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
    private bool gpsgettrg = false;
    private bool istudetrg = false;
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
            StartCoroutine(SaveUserGPS());
            mainUI.SetActive(true);
            loginUI.SetActive(false);
        }
    }
    public void logIn()
    {
        if (secret.text != "" && pw.text != "")
        {
            NCMBUser.LogInAsync(currentUserName.text, pw.text, (NCMBException e) =>
            {
                if (e != null)
                {
                    GManager.instance.setrg = 1;
                    check_loginmode.text = "��ԁF�V�K�o�^�܂��̓��O�C���Ɏ��s";
                }
                else
                {
                    UnityEngine.Debug.Log("�����O�C���ɐ����I");
                    check_loginmode.text = "��ԁF���O�C������";
                    bool tmptrg = UserSecret();
                    StartCoroutine(SaveUserGPS());
                    //mainUI.SetActive(true);
                    //loginUI.SetActive(false);
                }
            });
        }
        else
        {
            GManager.instance.setrg = 1;
            check_loginmode.text = "��ԁF���[���A�h���X���p�X���[�h�ɕs��";
        }
    }
    // mobile backend�ɐڑ����ĐV�K����o�^ ------------------------
    public void signUp()
    {
        if (pw.text == re_pw.text)
        {
            NCMBUser user = new NCMBUser();
            if (currentUserName.text == "") currentUserName.text = "anonymous user." + DateTime.Now.ToString();
            user.UserName = currentUserName.text;
            user.Password = pw.text;
            user.SignUpAsync((NCMBException e) =>
            {
                if (e != null)
                {
                    logIn();
                }
                else
                {
                    check_loginmode.text = "��ԁF�V�K�o�^�ɐ���";
                    AddSecret();

                }
            });
        }
        else
        {
            GManager.instance.setrg = 1;
            check_loginmode.text = "��ԁF���[���A�h���X���p�X���[�h�ɕs��";
        }
    }
    public void AddSecret()
    {
        NCMBObject obj = new NCMBObject("SecretWord");
        obj.Add("word", secret.text);
        obj.Add("username", currentUserName.text);
        obj.SaveAsync((NCMBException e) =>
        {
            if (e != null)
            {
                GManager.instance.setrg = 1;
                check_loginmode.text = "��ԁF�V�K�o�^�Ɏ��s";
            }
            else
            {
                //�������̏���
                bool tmptrg = UserSecret();
                StartCoroutine(SaveUserGPS());
            }
        });
    }
    public bool UserSecret()
    {
        bool tmptrg = false;
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("SecretWord");
        query.WhereEqualTo("word", secret.text);
        //����������ݒ�
        query.Limit = 99;
        //�f�[�^�X�g�A�ł̌������s��
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null)
            {
                GManager.instance.setrg = 1;
                check_loginmode.text = "��ԁF�閧�̌��t���s��v";
                logOut();
            }
            else
            {
                foreach (NCMBObject obj in objList)
                {
                    var tmp = obj["username"].ToString();
                    if (tmp == currentUserName.text)
                    {
                        tmptrg = true;
                    }
                }
            }
        });
        return tmptrg;
    }
    // mobile backend�ɐڑ����ă��O�A�E�g ------------------------

    public void logOut()
    {
        NCMBUser.LogOutAsync((NCMBException e) =>
        {
            if (e == null)
            {
                ;
            }
        });
    }
    public IEnumerator SaveUserGPS()
    {
        if (!istudetrg)
        {
            istudetrg = true;
            gpsmanager.GetUserGPS();
        }
        check_loginmode.text = "��ԁF�T�[�r�X�ȏ����擾���ł��A���X���҂���������";
        yield return new WaitForSeconds(2f);
        check_loginmode.text = "��ԁF�T�[�r�X�ȏ����擾����";
        DateGPS();
    }
   void DateGPS()
    {
        var currentUser = NCMBUser.CurrentUser;
        string currentName = "";
        if (currentUser != null) currentName = currentUser.UserName.ToString();
        else currentName = currentUserName.text;
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("UserGPS");
        query.WhereEqualTo("username", currentName);
        //����������ݒ�
        query.Limit = 99;
        //�f�[�^�X�g�A�ł̌������s��
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null)
            {
                if (gpsmanager.isuse)
                {
                    NCMBObject obj = new NCMBObject("UserGPS");
                    obj.Add("username", currentName);
                    obj.Add("login_latitude", gpsmanager.user_latitude);
                    obj.Add("login_longitude", gpsmanager.user_longitude);
                    obj.SaveAsync((NCMBException e) =>
                    {
                        if (e != null)
                        {
                            GManager.instance.setrg = 1;
                            check_loginmode.text = "��ԁF�T�[�r�X�̂��߂̈ʒu���X�V�Ɏ��s";
                        //
                    }
                        else
                        {
                            GManager.instance.setrg = 0;
                            gpsgettrg = true;
                        }
                    });
                }
                else
                {
                    NCMBObject obj = new NCMBObject("UserGPS");
                    obj.Add("username", currentName);
                    obj.Add("login_latitude", -1);
                    obj.Add("login_longitude", -1);
                    obj.SaveAsync((NCMBException e) =>
                    {
                        if (e != null)
                        {
                            GManager.instance.setrg = 1;
                            check_loginmode.text = "��ԁF�T�[�r�X�̂��߂̈ʒu���X�V�Ɏ��s";
                            //
                        }
                        else
                        {
                            GManager.instance.setrg = 0;
                            check_loginmode.text = "��ԁF�ʒu��񂪕s���ł��B";
                            gpsgettrg = true;
                        }
                    });
                }
            }
            else
            {
                foreach (NCMBObject obj in objList)
                {
                    if (gpsmanager)
                    {
                        userobj = obj;
                        NCMBObject tmpobj = userobj;
                        tmpobj["login_latitude"] = gpsmanager.user_latitude;
                        tmpobj["olgin_longitude"] = gpsmanager.user_longitude;
                        gpsgettrg = true;
                        GManager.instance.setrg = 0;
                        break;
                    }
                    else
                    {
                        userobj = obj;
                        NCMBObject tmpobj = userobj;
                        tmpobj["login_latitude"] = -1;
                        tmpobj["olgin_longitude"] = -1;
                        gpsgettrg = true;
                        GManager.instance.setrg = 0;
                        check_loginmode.text = "��ԁF�ʒu��񂪕s���ł��B";
                        break;
                    }
                }
            }
        });
        if (gpsgettrg)
        {
            mainUI.SetActive(true);
            loginUI.SetActive(false);
        }
        else
        {
            GManager.instance.setrg = 1;
            check_loginmode.text = "��ԁF�T�[�r�X�̂��߂̈ʒu����s���̂܂ܑ��s";
            mainUI.SetActive(true);
            loginUI.SetActive(false);
        }
    }
}