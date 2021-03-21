using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] CursorScriptable cursor;

    private void Awake()
    {
        Cursor.SetCursor(cursor.arrowCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnEnable()
    {
        EventManager.ChangeArrowToSword += ArrowToSwordHandler;
        EventManager.ChangeSwordToArrow += SwordToArrowHandler;
        PlayerEventManager.AllHPLost += PlayerDeathHandler;

        EnemyEventManager.AllHPLost += EnemyDeathHandler;
    }

    private void EnemyDeathHandler(EnemyController obj)
    {
        PlayerEventManager.CallSwitchState();
        Destroy(obj.gameObject);
    }

    private void OnDisable()
    {
        EventManager.ChangeArrowToSword -= ArrowToSwordHandler;
        EventManager.ChangeSwordToArrow -= SwordToArrowHandler;
        PlayerEventManager.AllHPLost -= PlayerDeathHandler;
        EnemyEventManager.AllHPLost -= EnemyDeathHandler;
    }

    private void ArrowToSwordHandler()
    {
        Cursor.SetCursor(cursor.swordCursor, Vector3.zero, CursorMode.ForceSoftware);
    }

    private void SwordToArrowHandler()
    {
        Cursor.SetCursor(cursor.arrowCursor, Vector3.zero, CursorMode.ForceSoftware);
    }

    private void PlayerDeathHandler(PlayerController player)
    {
        Debug.Log("Dead!");
        Destroy(player.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
