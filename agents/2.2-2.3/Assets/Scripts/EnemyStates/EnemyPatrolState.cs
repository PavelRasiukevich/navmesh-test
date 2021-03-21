using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    private int index = 0;

    public override void EntryState(EnemyController enemy)
    {
        enemy.Particle.Play();
       
        enemy.ViewAngle = enemy.InitialViewAngle;
        enemy.ViewDistance = enemy.InitialViewDistance;

        enemy.Lamp.enabled = false;
    }

    public override void Update(EnemyController enemy)
    {
        

        if (enemy.IsNear || enemy.HasDetected)
            enemy.TransitionToState(enemy.Chase);

        PatrolArea(enemy);
    }

    private void PatrolArea(EnemyController enemy)
    {
        enemy.Agent.SetDestination(enemy.Waypoints[index].position);

        if (Vector3.Distance(enemy.transform.position, enemy.Waypoints[index].position) < enemy.Agent.stoppingDistance + 0.5)
        {
            if (index < enemy.Waypoints.Length - 1)
                index++;
            else
                index = 0;
        }
    }

}
