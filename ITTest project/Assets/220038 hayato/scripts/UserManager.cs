using UnityEngine;
using System.Collections;
using NCMB;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
public class UserManager : MonoBehaviour
{
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

    // mobile backendに接続してログイン ------------------------
    private void Start()
    {
        //数時間以内にログインしたことがあるなら自動ログイン
        //誰かしらに担当※(教えます)
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

}