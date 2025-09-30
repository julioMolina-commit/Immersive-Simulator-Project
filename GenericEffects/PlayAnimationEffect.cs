using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationEffect : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void PlayAnimation()
    {
        anim.SetBool("isPlaying", true);
    }
    public void StopAnimation()
    {
        anim.SetBool("isPlaying", false);
    }
}
