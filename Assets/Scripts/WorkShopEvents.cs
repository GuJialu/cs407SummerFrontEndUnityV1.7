using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//email
public class LoginEvent : UnityEvent<string>
{
}

public static class WorkShopEvents
{
    public static UnityEvent loginEvent = new UnityEvent();
    public static UnityEvent logoutEvent = new UnityEvent();
}
