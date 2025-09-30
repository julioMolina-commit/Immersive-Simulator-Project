using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnim : MonoBehaviour
{
    Animator cameraAnim;
    private void Start()
    {
        cameraAnim = GetComponent<Animator>();

        cameraAnim.SetBool("isShaking", false);
    }
    public void StartCameraShake()
    {
        cameraAnim.SetBool("isShaking", true);
    }
    public void StopCameraShake()
    {
        cameraAnim.SetBool("isShaking", false);
        print("Works");
    }
}
