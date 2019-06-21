using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//email, username, icon number, description
public class LoginEvent : UnityEvent<string, string, int, string>
{
}

public static class WorkShopEvents
{
    public static LoginEvent loginEvent;
}
