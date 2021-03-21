using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerStatsScriptable stats;
    //public GameObject point;
    [SerializeField] ITargetable chosenTarget;
    [SerializeField] Image healthBar;

    #region Particles
    [SerializeField] ParticleSystem particleDust;
    [SerializeField] ParticleSystem hit;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem pointer;
    #endregion

    #region Components
    [SerializeField] NavMeshAgent agent;
    #endregion

    #region States
    PlayerBaseState currentState;
    PlayerStateIdle idle;
    PlayerStateMove move;
    PlayerAttackState attack;
    #endregion

    #region Properties
    public PlayerStateIdle Idle { get => idle; }
    public PlayerStateMove Move { get => move; }
    public PlayerAttackState Attack { get => attack; }
    public NavMeshAgent Agent { get => agent; }
    public PlayerStatsScriptable Stats { get => stats; }
    public ITargetable ChosenTarget { get => chosenTarget; set => chosenTarget = value; }
    public bool HasReloaded
    {
        get
        {
            stats.timeToReload -= Time.deltaTime;

            if (stats.timeToReload <= 0)
            {
                stats.timeToReload = stats.initialReloadTime;
                return true;
            }

            return false;
        }
    }
    public ParticleSystem ParticleDust { get => particleDust; set => particleDust = value; }
    public ParticleSystem Pointer { get => pointer; set => pointer = value; }
    #endregion

    private void Awake()
    {
        healthBar = GetComponentsInChildren<Image>()[1];

        agent = GetComponent<NavMeshAgent>();

        idle = new PlayerStateIdle();
        move = new PlayerStateMove();
        attack = new PlayerAttackState();

    }

    private void OnEnable()
    {
        PlayerEventManager.SwitchState += SwitchStateHandler;
    }

    private void OnDisable()
    {
        PlayerEventManager.SwitchState -= SwitchStateHandler;
    }

    private void SwitchStateHandler()
    {
        TransitionToState(idle);
    }

    private void Start()
    {
        stats.currentToughness = stats.initialToughness;
        healthBar.fillAmount = stats.currentToughness / stats.initialToughness;
        TransitionToState(idle);
    }

    private void Update()
    {
        if (stats.currentToughness <= 0)
        {
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            PlayerEventManager.CallAllHPLostEvent(this);
        }

        currentState.Update(this);
    }

    public void TransitionToState(PlayerBaseState state)
    {
        currentState = state;
        currentState.EntryState(this);
    }

    public void TakeDamage(int damageAmmount)
    {
        if (stats.currentToughness > 0)
        {
            hit.Play();
            stats.currentToughness -= damageAmmount;
            healthBar.fillAmount = (float)stats.currentToughness / stats.initialToughness;
            Debug.Log(stats.currentToughness);
        }
    }
}
