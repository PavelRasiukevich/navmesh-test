using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerEventManager
{
    public static Action<PlayerController> AllHPLost;
    public static Action SwitchState;

    public static void CallSwitchState()
    {
        SwitchState?.Invoke();
    }

    public static void CallAllHPLostEvent(PlayerController player)
    {
        AllHPLost?.Invoke(player);
    }
}
