using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private PlayerController player;

    public override void EntryState(EnemyController enemy)
    {
        enemy.Particle.Stop();
       
        player = enemy.Target.GetComponent<PlayerController>();
    }

    public override void Update(EnemyController enemy)
    {
        Quaternion rotation = Quaternion.RotateTowards(enemy.transform.rotation, Quaternion.LookRotation(enemy.Target.position - enemy.transform.position), 3.0f * Time.deltaTime);
        enemy.transform.rotation = rotation;

        AttackTarget(enemy);
    }

    private void AttackTarget(EnemyController enemy)
    {
        if (Vector3.Distance(enemy.transform.position, enemy.Target.position) <= enemy.DetectionDistance)
        {
            if (enemy.HasReloaded)
                player.TakeDamage(enemy.Stats.power);
        }
        else
        {
            if (!enemy.IsNear && !enemy.HasDetected)
                enemy.TransitionToState(enemy.Patrol);
            else
                enemy.TransitionToState(enemy.Chase);
        }
    }
}
