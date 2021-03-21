using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/Stats")]
public class PlayerStatsScriptable : ScriptableObject
{
    public int initialToughness;
    public int currentToughness;
    public int power;
    public float initialReloadTime;
    public float timeToReload;
    public float attackRange;
}
