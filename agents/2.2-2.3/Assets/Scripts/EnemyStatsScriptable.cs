using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemy/Stats")]
public class EnemyStatsScriptable : ScriptableObject
{
    public int initialToughness;
    public int currentToughness;
    public int power;
    public float initialReloadTime;
    public float timeToReload;
}
