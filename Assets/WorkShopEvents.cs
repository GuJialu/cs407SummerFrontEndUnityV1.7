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
    public static LoginEvent loginEvent = new LoginEvent();
}
