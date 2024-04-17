using UnityEngine;

public class ButtonCntrl : MonoBehaviour
{
    public Material[] mats;
    private MeshRenderer meshRenderer;
    private bool isEnabled = false;

    public void Reset()
    {
        isEnabled = false;
        meshRenderer.material = mats[0];
    }

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            isEnabled = true;
            meshRenderer.material = mats[1];
        }
    }
}
