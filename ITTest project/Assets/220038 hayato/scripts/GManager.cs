using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
public class GManager : MonoBehaviour
{
    public static GManager instance = null;
    [Header("UniLangで言語管理")]
    public int isEnglish = 0;
    [Header("上記に値する言語達の一覧")]
    public string[] LanguageList;
    public bool walktrg = true;
    public float seMax = 0.008f;
    public float audioMax = 0.01f;

    public int setmenu = 0;
    public bool ESCtrg = false;
    public int setrg = -1;

    public bool debug_trg = false;
    public float reset_time = 0;

    public string[] not_word;
    public bool sorttrg = true;
    public string check_onword = "";
    public string check_notword = "";
    public int old_year = 2023;
    [System.Serializable]
    public struct DevDateTime
    {
        public int year;
        public int month;
        public int day;
    }
    [Header("ここからは日にち系")]
    public DevDateTime devdays;
    public DateTime checkdev = new DateTime(2003, 7, 28);
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
    private void Start()
    {
        old_year = PlayerPrefs.GetInt("Year", 2023);
        if (old_year != GetGameDay().Year)
        {
            old_year = GetGameDay().Year;
            PlayerPrefs.SetInt("Year", old_year);
            YearReset();
        }
    }
    private void Update()
    {
        if (instance.reset_time >= 0)
        {
            instance.reset_time -= Time.deltaTime;
        }
    }
    public void YearReset()
    {
        ;
    }
    public DateTime GetGameDay()
    {
        DateTime tmp = DateTime.Today;
        return tmp;
    }
    public int AllSpanCheck(DateTime tmp_time)
    {
        int check_result = 0;

        DateTime today = DateTime.Today;
        DateTime devday = new DateTime(instance.devdays.year, instance.devdays.month, instance.devdays.day);
        if (instance.checkdev != devday)
            today = devday;
        DateTime newday = new DateTime(today.Year, tmp_time.Month, tmp_time.Day);
        TimeSpan tmpdiff = newday - today;
        check_result = (int)tmpdiff.TotalDays;
        //print(check_result.ToString());
        return check_result;
    }
    public bool MonthBoolCheck(DateTime tmp_time)
    {
        bool check_result = false;
        DateTime today = DateTime.Today;
        DateTime devday = new DateTime(instance.devdays.year, instance.devdays.month, instance.devdays.day);
        if (instance.checkdev != devday)
            today = devday;
        DateTime newday = new DateTime(today.Year, tmp_time.Month, tmp_time.Day);
        if (newday.Month == today.Month)
            check_result = true;
        return check_result;
    }
    public int DaySpanCheck(DateTime tmp_time)
    {
        int check_result = 0;
        DateTime today = DateTime.Today;
        DateTime devday = new DateTime(instance.devdays.year, instance.devdays.month, instance.devdays.day);
        if (instance.checkdev != devday)
            today = devday;
        DateTime newday = new DateTime(today.Year, tmp_time.Month, tmp_time.Day);
        check_result = newday.Day - today.Day;
        print(check_result.ToString());
        return check_result;
    }
}