using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCSimplePatrol : MonoBehaviour
{
    [SerializeField] private bool patrolWaiting;
    [SerializeField] private float totalWaitTimer = 3f;
    [SerializeField] private float switchProbability = 0.2f;
    [SerializeField] List<Waypoint> patrolPoints;

    NavMeshAgent navAgent;

    private int currentPatrolIndex;

    private float waitTimer;

    private bool traveling;
    private bool waiting;
    private bool patrolForward;

    public bool patrolling;
    void Start()
    {
        navAgent = this.GetComponent<NavMeshAgent>();
        ChangePatrolPoint();

        SetDestination();
    }

    private void Update()
    {
        if (patrolling)
        {
            if (traveling && navAgent.remainingDistance <= 1f)
            {
                traveling = false;
                if (patrolWaiting)
                {
                    waiting = true;
                    waitTimer = 0f;
                }
                else
                {
                    ChangePatrolPoint();
                    SetDestination();
                }
            }

            if (waiting)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= totalWaitTimer)
                {
                    waiting = false;
                    ChangePatrolPoint();
                    SetDestination();
                }
            }
        }
     
    }

    private void SetDestination()
    {
        Vector3 targetVector = patrolPoints[currentPatrolIndex].transform.position;
        navAgent.SetDestination(targetVector);
        traveling = true;
    }

    private void ChangePatrolPoint()
    {
        if (UnityEngine.Random.Range(0 , 1f) <= switchProbability)
        {
            patrolForward = !patrolForward;
        }

        if (patrolForward)
        {
            currentPatrolIndex++;

            if (currentPatrolIndex >= patrolPoints.Count)
            {
                currentPatrolIndex = 0;
            }
        }
        else
        {
            if (--currentPatrolIndex < 0)
            {
                currentPatrolIndex = patrolPoints.Count - 1;
            }
        }
    }
}
