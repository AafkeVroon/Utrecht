using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EnemyVisionCone : MonoBehaviour
{
    [SerializeField] private LayerMask viewMask;

    // [Header("Events")]
   // [SerializeField] private Vector3Events ;
    Event myEvent;
    //check if target is in characther field of view
    public bool InPov(Transform viewer, Transform t, float maxAng, float maxRad)
    {
        Collider[] overlaps = new Collider[10];                                        //buffer holding values where colliders overlap
        int count = Physics.OverlapSphereNonAlloc(viewer.position, maxRad, overlaps);  //count of number of overlaps

        for (int i = 0; i < count; i++)                                                
        {
            if (overlaps[i] != null)                                                   //if overlaps is not null
            {
                if (overlaps[i].transform == t)                                          //and if overlaps[i] matches the target
                {
                    Vector3 directionBetween = (t.position - viewer.position).normalized;
                    directionBetween.y *= 0;

                    float angle = Vector3.Angle(viewer.forward, directionBetween);

                    if (angle <= maxAng)
                    {
                        Ray ray = new Ray(viewer.position, t.position - viewer.position);
                        RaycastHit hit;
                        //debug drawlihne(viewer.positionm, t.position)
                        if (Physics.Raycast(ray, out hit, maxRad , viewMask))
                        {
                            if (hit.transform == t)
                            {
                                Debug.Log(t + "Spotted at" + hit.transform.position + "by" + viewer);

                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }   
}
