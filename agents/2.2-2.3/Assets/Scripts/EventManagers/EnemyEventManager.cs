using System;
using UnityEngine;

public class EnemyEventManager : MonoBehaviour
{
    public static Action<EnemyController> AllHPLost;

    public static void CallAllHPLostEvent(EnemyController enemy)
    {
        AllHPLost?.Invoke(enemy);
    }
}
