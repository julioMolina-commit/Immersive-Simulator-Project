using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundtrack : MonoBehaviour
{
    public void ChangeOst(AudioClip ostClip)
    {
        GameObject.Find("OST_Player").GetComponent<AudioSource>().clip = ostClip;
        GameObject.Find("OST_Player").GetComponent<AudioSource>().Play();
    }
}
