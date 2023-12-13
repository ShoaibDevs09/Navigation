using UnityEngine;

public class footStepTarget : MonoBehaviour
{
    public int index;
    private void OnDestroy()
    {
        if (FindObjectOfType<footStepInstantiator>())
            FindObjectOfType<footStepInstantiator>().destroyFootsteps();
    }
}
