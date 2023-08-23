using UnityEngine;
using System.Collections;
using NCMB;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
public class UserManager : MonoBehaviour
{
    public static UserManager instance = null;
    [Header("ユーザー名")]
    public InputField currentUserName;
    [Header("秘密の言葉")]
    public InputField secret;
    [Header("パスワード")]
    public InputField pw;
    [Header("パスワード再確認用")]
    public InputField re_pw;
    //public InputField mpurse_id;

    [Header("ログイン状態確認用")]
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
    // mobile backendに接続してログイン ------------------------
    private void Start()
    {
        //数時間以内にログインしたことがあるなら自動ログイン
        if (NCMBUser.CurrentUser != null)
        {
            NCMBUser currentUser = NCMBUser.CurrentUser;
            print(currentUser.UserName);
            check_loginmode.text = "状態：ログイン成功";
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
                    check_loginmode.text = "状態：新規登録またはログインに失敗";
                }
                else
                {
                    UnityEngine.Debug.Log("※ログインに成功！");
                    check_loginmode.text = "状態：ログイン成功";
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
            check_loginmode.text = "状態：メールアドレスかパスワードに不備";
        }
    }
    // mobile backendに接続して新規会員登録 ------------------------
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
                    check_loginmode.text = "状態：新規登録に成功";
                    AddSecret();

                }
            });
        }
        else
        {
            GManager.instance.setrg = 1;
            check_loginmode.text = "状態：メールアドレスかパスワードに不備";
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
                check_loginmode.text = "状態：新規登録に失敗";
            }
            else
            {
                //成功時の処理
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
        //検索件数を設定
        query.Limit = 99;
        //データストアでの検索を行う
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null)
            {
                GManager.instance.setrg = 1;
                check_loginmode.text = "状態：秘密の言葉が不一致";
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
    // mobile backendに接続してログアウト ------------------------

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
        check_loginmode.text = "状態：サービスな情報を取得中です、少々お待ちください";
        yield return new WaitForSeconds(2f);
        check_loginmode.text = "状態：サービスな情報を取得完了";
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
        //検索件数を設定
        query.Limit = 99;
        //データストアでの検索を行う
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
                            check_loginmode.text = "状態：サービスのための位置情報更新に失敗";
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
                            check_loginmode.text = "状態：サービスのための位置情報更新に失敗";
                            //
                        }
                        else
                        {
                            GManager.instance.setrg = 0;
                            check_loginmode.text = "状態：位置情報が不明です。";
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
                        check_loginmode.text = "状態：位置情報が不明です。";
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
            check_loginmode.text = "状態：サービスのための位置情報を不明のまま続行";
            mainUI.SetActive(true);
            loginUI.SetActive(false);
        }
    }
}