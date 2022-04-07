using UnityEngine;
using UnityEngine.AI;

public class NPCMoveTowardsPlayer : MonoBehaviour
{
    [SerializeField]GameObject target;

    NavMeshAgent navAgent;

    [SerializeField] private float distanceBetweenPlayer;
    [SerializeField] private float maxDistanceBetweenPlayer;

    Vector3 targetVector;

    public bool goingToPlayer;    

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (goingToPlayer)
            MoveToplayer();
    }

    private void MoveToplayer()
    {
        distanceBetweenPlayer = Vector3.Distance(target.transform.position, transform.position);
        targetVector = target.transform.position;

        if (distanceBetweenPlayer < maxDistanceBetweenPlayer)
            PauseMovement();

        SetDestination(targetVector);
    }

    private void SetDestination(Vector3 target)
    {
        navAgent.SetDestination(target);
    }
    private void PauseMovement()
    {
        navAgent.velocity = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxDistanceBetweenPlayer);
    }
}
