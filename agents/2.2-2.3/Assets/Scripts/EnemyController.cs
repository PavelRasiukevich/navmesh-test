using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class EnemyController : MonoBehaviour, ITargetable
{
    #region General
    private float viewAngle;
    private float viewDistance;
    [SerializeField] float initialViewAngle;
    [SerializeField] float initialViewDistance;
    [SerializeField] float viewAngleWhenChasing;
    [SerializeField] float viewDistanceWhenChasing;
    [SerializeField] float detectionDistance;
    [SerializeField] float angleBetweenVectors;
    [SerializeField] float distanceToTarget;
    [SerializeField] Transform target;
    private Transform[] waypoints;
    [SerializeField] Transform[] points;
    [SerializeField] EnemyStatsScriptable stats;
    [SerializeField] Image healthBar;
    #endregion

    #region Particles
    [SerializeField] ParticleSystem particleDust;
    [SerializeField] ParticleSystem hit;
    [SerializeField] ParticleSystem deathParticle;
    #endregion

    #region States
    private EnemyBaseState currentState;
    private EnemyPatrolState patrol;
    private EnemyChaseTargetState chase;
    private EnemyAttackState attack;
    #endregion

    #region Components
    [SerializeField] Light lamp;
    private NavMeshAgent agent;
    #endregion

    #region Properties
    public float ViewDistance { get => viewDistance; set => viewDistance = value; }
    public float DetectionDistance { get => detectionDistance; }
    public float AngleBetweenVectors { get => angleBetweenVectors; }
    public float DistanceToTarget { get => distanceToTarget; }
    public Transform Target { get => target; }
    public NavMeshAgent Agent { get => agent; }
    public EnemyPatrolState Patrol { get => patrol; }
    public EnemyChaseTargetState Chase { get => chase; }
    public EnemyAttackState Attack { get => attack; }
    public bool HasDetected
    {
        get
        {
            Vector3 _targetDir = target.position - transform.position;
            distanceToTarget = Vector3.Distance(transform.position, target.position);
            angleBetweenVectors = Vector3.Angle(_targetDir, transform.forward);

            if (Physics.Raycast(transform.position, _targetDir, out RaycastHit hitInfo, viewDistance))
            {
                PlayerController player = hitInfo.collider.gameObject.GetComponent<PlayerController>();

                if (angleBetweenVectors < viewAngle)
                    if (distanceToTarget < viewDistance)
                        if (player)
                            return true;
            }

            return false;
        }
    }
    public bool IsNear
    {
        get
        {
            return Vector3.Distance(transform.position, target.position) < detectionDistance;
        }
    }
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

    public Light Lamp { get => lamp; }
    public Transform[] Waypoints { get => waypoints; set => waypoints = value; }
    public EnemyStatsScriptable Stats { get => stats; }
    public float AngleWhenChasing { get => viewAngleWhenChasing; }
    public float ViewDistanceWhenChasing { get => viewDistanceWhenChasing; }
    public float ViewAngle { get => viewAngle; set => viewAngle = value; }
    public float InitialViewDistance { get => initialViewDistance; set => initialViewDistance = value; }
    public float InitialViewAngle { get => initialViewAngle; set => initialViewAngle = value; }

    public Transform Transform => transform;

    public ParticleSystem Particle { get => particleDust; set => particleDust = value; }

    #endregion

    private void Awake()
    {
        waypoints = new Transform[5];
        SetRandomPoints();

        healthBar = GetComponentsInChildren<Image>()[1];

        viewAngle = initialViewAngle;
        viewDistance = initialViewDistance;

        agent = GetComponent<NavMeshAgent>();
        lamp = GetComponentInChildren<Light>();

        patrol = new EnemyPatrolState();
        chase = new EnemyChaseTargetState();
        attack = new EnemyAttackState();
    }

    private void SetRandomPoints()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = points[UnityEngine.Random.Range(0, points.Length)];
        }
    }

    private void OnEnable()
    {
        stats.timeToReload = 0;
    }

    private void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;

        stats.currentToughness = stats.initialToughness;
        healthBar.fillAmount = stats.currentToughness / stats.initialToughness;

        TransitionToState(patrol);
    }

    private void Update()
    {
        if (stats.currentToughness <= 0)
        {
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            stats.currentToughness = 0;
            EnemyEventManager.CallAllHPLostEvent(this);
        }

        currentState.Update(this);
        DrawViewArea();
    }

    public void TransitionToState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EntryState(this);
    }

    public void TakeDamage(int damage)
    {
        if (stats.currentToughness > 0)
        {
            hit.Play();
            stats.currentToughness -= damage;
            healthBar.fillAmount = (float)stats.currentToughness / stats.initialToughness;
            Debug.Log(stats.currentToughness);
        }

    }


    void OnMouseEnter()
    {
        EventManager.CallChangeCursorToSword();
    }

    void OnMouseExit()
    {
        EventManager.CallChangeCursorToArrow();
    }

    private void DrawViewArea()
    {
        Vector3 left = transform.position + Quaternion.Euler(Vector3.up * viewAngle) * (transform.forward * viewDistance);
        Vector3 right = transform.position + Quaternion.Euler(-Vector3.up * viewAngle) * (transform.forward * viewDistance);

        Debug.DrawLine(transform.position, right, Color.blue);
        Debug.DrawLine(transform.position, left, Color.blue);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
    }
}
