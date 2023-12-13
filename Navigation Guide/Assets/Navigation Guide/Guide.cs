using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Guide : MonoBehaviour
{
    public NavMeshAgent agent;
    public float maxSpeed, minSpeed, maxDistance, minDistance;
    public Transform player;
    public Transform targetT;
    Vector3 target, startPosition;
    //public AudioSource source;

    public bool idle;
    bool returningToPlayer;

    private void Start()
    {
        startPosition = transform.position;
    }

    void OnEnable()
    {
        transform.position = startPosition;
        setDestination(targetT.position);
    }

    public void setDestination(Vector3 position)
    {
        target = position;
        agent.isStopped = false;
        agent.destination = position;
    }

    void Update()
    {
        float disSpeed = maxSpeed - Vector3.Distance(transform.position, player.position);
        float disSpeed2 = Vector3.Distance(transform.position, player.position) - maxSpeed;

        if (!idle)
        {
            if (Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
                agent.isStopped = true;

            if (Vector3.Distance(transform.position, player.position) > maxDistance)
            {
                returningToPlayer = true;
                if (agent.destination != player.position)
                    agent.destination = player.position;

                agent.speed = Mathf.Clamp(disSpeed2, minSpeed, maxSpeed);
            }

            if (returningToPlayer)
            {
                if (Vector3.Distance(transform.position, player.position) < minDistance)
                    returningToPlayer = false;
            }

            else if (!returningToPlayer)
            {
                if (agent.destination != target)
                    setDestination(target);

                agent.speed = Mathf.Clamp(disSpeed, minSpeed, maxSpeed);
            }
        }

        else
        {
            if (agent.destination != player.position)
            {
                agent.isStopped = false;
                agent.destination = player.position;
                agent.speed = maxSpeed;
            }
        }
    }
}
