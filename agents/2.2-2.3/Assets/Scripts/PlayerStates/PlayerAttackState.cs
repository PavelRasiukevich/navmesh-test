using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private EnemyController enemy;

    public override void EntryState(PlayerController player)
    {
        player.Pointer.gameObject.SetActive(true);
        enemy = player.ChosenTarget.Transform.GetComponent<EnemyController>();
    }

    public override void Update(PlayerController player)
    {
        player.Pointer.transform.position = player.ChosenTarget.Transform.position;

        if (Vector3.Distance(player.transform.position, player.ChosenTarget.Transform.position) <= player.Stats.attackRange)
        {
            player.Pointer.gameObject.SetActive(false);
            player.ParticleDust.Stop();
            AttackTarget(player);
        }
        else
            player.Agent.SetDestination(player.ChosenTarget.Transform.position);


        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out RaycastHit hit))
            {

                ITargetable t = hit.collider.GetComponent<ITargetable>();

                if (t != null)
                {
                    player.ChosenTarget = t;
                    player.Pointer.transform.position = player.ChosenTarget.Transform.position;
                    player.Agent.SetDestination(hit.point);
                }
                else
                {
                    player.Pointer.gameObject.SetActive(true);
                    player.Agent.SetDestination(hit.point);
                    player.Pointer.transform.position = player.Agent.destination;
                    player.TransitionToState(player.Move);
                }
            }
        }
    }

    private void AttackTarget(PlayerController player)
    {
        if (player.HasReloaded)
            enemy.TakeDamage(player.Stats.power);
    }
}
