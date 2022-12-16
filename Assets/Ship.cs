using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Ship : MonoBehaviour
{

    [SerializeField] const int START_LUCK_POINTS = 3;
    [SerializeField] const float DEATH_CHANCE = 0.3f;
    NavMeshAgent _agent;

    public Path path;

    public int currentDestinationId;


    int _luckPoints = START_LUCK_POINTS;

    [SerializeField] Animator animator;

    static readonly int Dead = Animator.StringToHash("Dead");
    static readonly int Dodged = Animator.StringToHash("Dodged");

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(path.wayPoints[currentDestinationId]);
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_agent.remainingDistance < 2.5f)
        {
            currentDestinationId++;
            if (currentDestinationId >= path.wayPoints.Count) currentDestinationId = 0;
            _agent.SetDestination(path.wayPoints[currentDestinationId]);
        }

    }

    public void TakeAShot()
    {
        if (_luckPoints <= 1) Die();
        else
        {
            _luckPoints--;
            if (Random.Range(0, 1f) < DEATH_CHANCE)
                Die();
            else
                Dodge();
        }
    }

    void Die()
    {
        _agent.isStopped = true;
        animator.SetTrigger(Dead);
    }

    void Dodge()
    {
        animator.SetTrigger(Dodged);
    }


    void OnDestroy()
    {
        if(ShipsManager.Instance != null)
            ShipsManager.Instance.ReleaseShip();
    }
}
