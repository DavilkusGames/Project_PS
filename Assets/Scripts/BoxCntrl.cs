using UnityEngine;

public class BoxCntrl : MonoBehaviour
{
    private Transform trans;
    private Vector3 spawnPos;
    private Quaternion spawnDir;

    private void Start()
    {
        trans = transform;
        spawnPos = transform.position;
        spawnDir = transform.rotation;
    }

    public void Respawn()
    {
        transform.SetParent(null);
        transform.position = spawnPos;
        transform.rotation = spawnDir;
    }
}
