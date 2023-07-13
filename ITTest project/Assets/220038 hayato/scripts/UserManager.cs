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
    [Header("Gmailアドレス")]
    public InputField mail;
    [Header("Gmailパスワード")]
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
                    UnityEngine.Debug.Log("※ログインに失敗: " + e.ErrorMessage);
                    check_loginmode.text = "状態：ログイン失敗";
                }
                else
                {
                    UnityEngine.Debug.Log("※ログインに成功！");
                    check_loginmode.text = "状態：ログイン成功";
                    StartCoroutine(nameof(SaveUserGPS));
                    mainUI.SetActive(true);
                    loginUI.SetActive(false);
                }
            });
        }
        else UnityEngine.Debug.Log("※メールアドレスかパスワードがちゃんと入力されていません");
    }

    // mobile backendに接続して新規会員登録 ------------------------

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
                    UnityEngine.Debug.Log("ユーザーの新規登録に失敗: " + e.ErrorMessage);
                    check_loginmode.text = "状態：新規登録に失敗";
                }
                else
                {
                    UnityEngine.Debug.Log("ユーザーの新規登録に成功");
                    check_loginmode.text = "状態：新規登録に成功";
                }
            });
        }
        else UnityEngine.Debug.Log("※パスワードが一致しないか、またはメールアドレス等が入力されていません");
    }

    // mobile backendに接続してログアウト ------------------------

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
            UnityEngine.Debug.Log("ログイン中のユーザー: " + currentUser.UserName);
            query.OrderByDescending(currentUser.UserName);
            //検索件数を設定
            query.Limit = 4;
            //データストアでの検索を行う
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
                            UnityEngine.Debug.Log("位置情報をデータストアへ送信失敗しました");
                        }
                        else
                        {
                            UnityEngine.Debug.Log("位置情報をデータストアへ送信成功しました");
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
                    UnityEngine.Debug.Log("位置情報をデータストアへ送信失敗しました");
                }
                else
                {
                    UnityEngine.Debug.Log("位置情報をデータストアへ送信成功しました");
                    NCMBUser user = new NCMBUser();
                    user.UserName = currentUserName.text;
                }
            });
        }
    }
}