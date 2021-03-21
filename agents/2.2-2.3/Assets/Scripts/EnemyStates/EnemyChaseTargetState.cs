using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseTargetState : EnemyBaseState
{
    public override void EntryState(EnemyController enemy)
    {
        enemy.Particle.Play();
        
        enemy.ViewAngle = enemy.AngleWhenChasing;
        enemy.ViewDistance = enemy.ViewDistanceWhenChasing;

        enemy.Lamp.enabled = true;
        enemy.Lamp.range = enemy.ViewDistanceWhenChasing;
    }

    public override void Update(EnemyController enemy)
    {
        if (!enemy.IsNear && !enemy.HasDetected)
            enemy.TransitionToState(enemy.Patrol);

        ChaseTarget(enemy);

    }

    private void ChaseTarget(EnemyController enemy)
    {
        Quaternion rotation = Quaternion.RotateTowards(enemy.transform.rotation, Quaternion.LookRotation(enemy.Target.position - enemy.transform.position), 3.0f *Time.deltaTime);
        enemy.transform.rotation = rotation;

        enemy.Agent.SetDestination(enemy.Target.position);

        if (Vector3.Distance(enemy.transform.position, enemy.Target.position) <= enemy.DetectionDistance)
            enemy.TransitionToState(enemy.Attack);
    }


}
