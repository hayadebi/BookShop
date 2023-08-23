using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiovolume : MonoBehaviour
{
    private bool isadd = false;
    float oldvolume;
    public bool setrg = false;
    public bool over_zero = true;
    private AudioSource _audio;
    void Start()
    {
        //アタッチされているAudioSource取得
        _audio = GetComponent<AudioSource>();
        if (!setrg)
        {
            _audio.volume = GManager.instance.audioMax / 4;
            oldvolume = GManager.instance.audioMax / 4;
        }
        else if (setrg)
        {
            _audio.volume = GManager.instance.seMax;
            oldvolume = GManager.instance.seMax;
        }
    }
    // Update is called once per frame
    private void Update()
    {
        if (!setrg && oldvolume != GManager.instance.audioMax / 4 && GManager.instance.setmenu <= 0)
        {
            _audio.volume = GManager.instance.audioMax / 3;
            oldvolume = GManager.instance.audioMax / 3;
        }
        else if (!setrg && GManager.instance.setmenu > 0)
        {
            _audio.volume = GManager.instance.audioMax / 9;
            oldvolume = GManager.instance.audioMax / 9;
        }
        else if (setrg && oldvolume != GManager.instance.seMax)
        {
            _audio.volume = GManager.instance.seMax;
            oldvolume = GManager.instance.seMax;
        }
    }
}
