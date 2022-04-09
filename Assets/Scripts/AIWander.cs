using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIWander : MonoBehaviour
{
    public enum AIStates
    {
        Waiting,
        Wandering
    }

    public Transform[] pointsToWander;
    public Transform currentWanderPoint;
    public float currentTime;
    public float maxWaitTime;
    public float minWaitTime;
    public float aiSpeed;
    public bool currentlyWaiting;

    public AIStates aiState;

    private float timeDrain;

    private int pointNumber;

    private NavMeshAgent AI;

    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        timeDrain = 100f;
        AI = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        AIStateDecider();
        anim.SetFloat("Velocity", AI.velocity.magnitude);
    }
    private void FixedUpdate()
    {
        AIStateActor();
    }

    private void AIStateDecider()
    {
        float dist = Vector3.Distance(gameObject.transform.position, currentWanderPoint.position);
        if (dist < 2.0f && aiState == AIStates.Wandering)
        {
            currentlyWaiting = true;
            aiState = AIStates.Waiting;
        }
    }

    private void AIStateActor()
    {
        if (aiState == AIStates.Waiting)
        {
            if (currentlyWaiting == true)
            {
                currentTime = Random.Range(minWaitTime, maxWaitTime);
                currentlyWaiting = false;
            }
            if (currentTime > 0)
            {
                currentTime -= timeDrain * Time.deltaTime;
                aiSpeed = 0f;
            }
            else if (currentTime <= 0)
            {
                pointNumber = Random.Range(0, pointsToWander.Length + 1);
                currentWanderPoint = pointsToWander[pointNumber];
                aiState = AIStates.Wandering;
            }
        }
        if (aiState == AIStates.Wandering)
        {
            aiSpeed = 5;
            AI.destination = currentWanderPoint.position;
        }
    }
}
