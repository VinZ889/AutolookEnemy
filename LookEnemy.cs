using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class LookEnemy : MonoBehaviour
{
    //public Transform target;
    float rotationSpeed = 2f;
    float visDist = 20f;
    float visAngle = 30f;
    Transform target;

    void Update()
    {
        
        SearchTarget();
        if (target)
        {
            LookAtEnemy();
        }

    }

    
    private void LookAtEnemy()
    {
        Vector3 direction = target.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        RaycastHit hit;
        bool seeWall = false;
        Debug.DrawRay(transform.position, direction, Color.red);
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            if (hit.collider.gameObject.tag == "wall")
            {
                seeWall = true;
                return;

            }
            else
            { 
                if (direction.magnitude < visDist && angle < visAngle && !seeWall)
                {
                    direction.y = 0;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
                }
        
            }
        }
        
        
    }

    private void SearchTarget()
    {
        var sceneTargets = FindObjectsOfType<EnemyHealth>();
        if (sceneTargets.Length == 0)
        {
            return;
        }
        Transform closestTarget = sceneTargets[0].transform;


        foreach (EnemyHealth testTarget in sceneTargets)
        {
            closestTarget = GetClosest(closestTarget, testTarget.transform);
        }
        target = closestTarget;
    }

    private Transform GetClosest(Transform transformA, Transform transformB)
    {
        var distToA = Vector3.Distance(transform.position, transformA.position);
        var distToB = Vector3.Distance(transform.position, transformB.position);
        if (distToA < distToB)
        {
            return transformA;
        }
        return transformB;
    }
}
