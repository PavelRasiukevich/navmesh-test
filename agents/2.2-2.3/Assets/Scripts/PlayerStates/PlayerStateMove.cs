using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMove : PlayerBaseState
{
    public override void EntryState(PlayerController player)
    {
        player.ParticleDust.Play();
    }

    public override void Update(PlayerController player)
    {
        if (player.Agent.remainingDistance < player.Agent.stoppingDistance)
        {
            player.Pointer.gameObject.SetActive(false);
            player.TransitionToState(player.Idle);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out RaycastHit hit))
            {
                ITargetable t = hit.collider.GetComponent<ITargetable>();

                if (t != null)
                {
                    player.ChosenTarget = t;
                    player.Agent.SetDestination(t.Transform.position);
                    player.TransitionToState(player.Attack);
                }
                else
                {
                    player.Pointer.gameObject.SetActive(true);
                    player.Agent.SetDestination(hit.point);
                    player.Pointer.transform.position = player.Agent.destination;
                }
            }
        }
    }
}
