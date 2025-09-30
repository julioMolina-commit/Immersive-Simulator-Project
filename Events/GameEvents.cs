using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents
{
    public class IntIntEvent : UnityEvent<int, int> { };

    public static UnityEvent EnterPickUpMode = new UnityEvent();
    public static UnityEvent ExitPickUpMode= new UnityEvent();
    public static UnityEvent EnterDialogue = new UnityEvent();
    public static UnityEvent ExitDialogue = new UnityEvent();

    public static UnityEvent SlowTime = new UnityEvent();

    public static UnityEvent SaveNewSettings = new UnityEvent();
}
