using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Drone : MonoBehaviour
{
    private float attackRange = 3f;
    private float rayDistance = 5f;
    private float stoppingDistance = 1.5f;

    private Vector3 destination;
    private Quaternion desiredRotation;
    private Vector3 direction;
    private Drone target;
    private DroneState currentState;

    void Update()
    {
        switch (currentState)
        {
            case DroneState.Wander:
                {
                    if (NeedDestination())
                    {
                        GetDestination();
                    }

                    transform.rotation = desiredRotation;

                    transform.Translate(translation: Vector3.forward * Time.deltaTime * 5f);



                    break;
                }
            
        }
    }

    private void GetDestination()
    {
        Vector3 testPosition = (transform.position + (transform.forward * 4f)) +
                              new Vector3(x: UnityEngine.Random.Range(-4.5f, 4.5f), y: 0f,
                              z: UnityEngine.Random.Range(-4.5f , 4.5f));

        destination = new Vector3(testPosition.x , y: 1f , testPosition.z);

        direction = Vector3.Normalize(destination - transform.position);
        direction = new Vector3(direction.x , y:0f , direction.z);
        desiredRotation = Quaternion.LookRotation(direction);
    }

    private bool NeedDestination()
    {
        if (destination == Vector3.zero)
        {
            return true;
        }

        var distance = Vector3.Distance(a: transform.position, b: destination);
        if (distance <= stoppingDistance)
        {
            return true;
        }

        return false;
    }
    public enum Team
    {
        Red,
        Blue
    }
    public enum DroneState
    { 
        Wander,
        Chase,
        Attack   
    }
}
