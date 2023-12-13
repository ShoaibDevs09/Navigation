using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
public class footStepInstantiator : MonoBehaviour
{
    public Transform target;
    private NavMeshPath path;
    public GameObject step1, step2;
    public Vector3[] points;
    public float stepSize;
    Vector3 startPosition;
    public List<GameObject> footsteps = new List<GameObject>();
    public bool instantiated;
    public Transform[] footstepTargets;

    void Start()
    {
        instantiate();
    }

    public void instantiate()
    {
        if (FindObjectOfType<footStepTarget>())
            target = FindObjectOfType<footStepTarget>().transform;

        instantiated = true;
        if (footsteps.Count > 0)
        {
            foreach (GameObject go in footsteps)
                Destroy(go);
        }

        int counter = 0;
        footsteps = new List<GameObject>();
        path = new NavMeshPath();
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 10, NavMesh.AllAreas);
        Vector3 startPos = hit.position;
        NavMesh.SamplePosition(target.position, out hit, 10, NavMesh.AllAreas);
        Vector3 targetPos = hit.position;

        NavMesh.CalculatePath(startPos, targetPos, NavMesh.AllAreas, path);
        points = path.corners;
        startPosition = transform.position;
        for (int i = 0; i < points.Length; i++)
        {
            float distance = 0;
            distance = Vector3.Distance(startPosition, points[i]);
            float rot = Vector3.Angle(transform.forward, points[i] - startPosition) + Vector3.Angle(transform.up, points[i] - startPosition);
            //print(rot);

            for (int j = 0; j < (distance / stepSize) - 1; j++)
            {
                if (j >= 0)
                {
                    if (j % 2 == 0)
                        footsteps.Add(Instantiate(step1, Vector3.Lerp(startPosition, points[i], j / (distance / stepSize)) + (Vector3.up * 1.5f), Quaternion.Euler(new Vector3(0, rot, 0))));

                    else
                        footsteps.Add(Instantiate(step2, Vector3.Lerp(startPosition, points[i], j / (distance / stepSize)) + (Vector3.up * 1.5f), Quaternion.Euler(new Vector3(0, rot, 0))));

                    footsteps[footsteps.Count - 1].GetComponent<footstepTrigger>().index = counter;
                    footsteps[footsteps.Count - 1].GetComponent<footstepTrigger>().lookAtPosition = points[i];
                    counter++;
                    //footsteps[footsteps.Count - 1].transform.LookAt(points[i]);
                }
            }

            startPosition = points[i];
        }
    }

    public void destroyFootsteps()
    {
        instantiated = false;
        if (footsteps.Count > 0)
            foreach (GameObject go in footsteps)
                Destroy(go);

        footsteps = new List<GameObject>();
    }

    public void destroyOnTrigger(int i)
    {
        for (int j = 0; j <= i; j++)
        {
            Destroy(footsteps[j]);
            //footsteps.Remove(footsteps[j]);
        }
    }
}
