using UnityEngine;

enum States { patrolling, goingToPlayer };

public class AIStateManager : MonoBehaviour
{
    [SerializeField]States states;

    NPCMoveTowardsPlayer moveTowardsPlayer;
    NPCSimplePatrol patrol;

    Quaternion startingAngle = Quaternion.AngleAxis(-60, Vector3.up);
    Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);

    private void Awake()
    {
        moveTowardsPlayer = GetComponent<NPCMoveTowardsPlayer>();
        patrol = GetComponent<NPCSimplePatrol>();

        states = States.patrolling;
    }

    private void Update()
    {
        //---------- TESTING
        if (Input.GetKeyDown(KeyCode.T))
        {
            states = States.goingToPlayer;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            states = States.patrolling;
        }
        //-----------

        if (states == States.goingToPlayer)
        {
            moveTowardsPlayer.goingToPlayer = true;
            patrol.patrolling = false;
        }
        else if (states == States.patrolling)
        {
            moveTowardsPlayer.goingToPlayer = false;
            patrol.patrolling = true;
            CheckForPlayer();
        }
    }

    /// <summary>
    /// Check if the player is close or in sight
    /// </summary>
    /// <returns></returns>
    private Transform CheckForPlayer()
    {
        float aggroRadius = 5f;

        RaycastHit hit;
        var angle = transform.rotation * startingAngle;
        var direction = angle * Vector3.forward;
        var pos = transform.position;

        for (var i = 0; i < 24; i++)
        {
            if (Physics.Raycast(pos, direction, out hit, aggroRadius * 10))
            {
                if (hit.collider.tag == "Player")                  
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.red);
                    states = States.goingToPlayer;
                }
                else
                    Debug.DrawRay(pos, direction * hit.distance, Color.yellow);
            }
            else
                Debug.DrawRay(pos, direction * aggroRadius, Color.white);

            direction = stepAngle * direction;
        }

        return null;
    }
}
