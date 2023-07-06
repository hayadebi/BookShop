using UnityEngine;
using System.Collections;
using NCMB;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
public class UserManager : MonoBehaviour
{
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

    // mobile backend�ɐڑ����ă��O�C�� ------------------------
    private void Start()
    {
        //�����Ԉȓ��Ƀ��O�C���������Ƃ�����Ȃ玩�����O�C��
        //�N������ɒS����(�����܂�)
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

}