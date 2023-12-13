using UnityEngine;

public class footstepTrigger : MonoBehaviour
{
    bool set = true;
    public int index;
    public Vector3 lookAtPosition;
    public LayerMask ground;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("checkPos", 0.1f);
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "floor")
        {
            transform.position += new Vector3(0, 0.1f, 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "floor")
        {
            set = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "floor")
        {
            set = false;
        }

        if (other.tag == "Player")
        {
            FindObjectOfType<footStepInstantiator>().destroyOnTrigger(index);
        }
    }

    public void checkPos()
    {
        if (!set)
            Invoke("checkPos", 1);

        else
        {
            RaycastHit hit;
            //Physics.Raycast(transform.position, -transform.up, out hit);

            if (Physics.Raycast(transform.position, -transform.up, out hit, 10, ground))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + 0.1f, transform.position.z);
            }

            transform.LookAt(lookAtPosition);
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

}
