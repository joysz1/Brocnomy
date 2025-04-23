using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private GameObject teleportPos;
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = teleportPos.transform.position;
    }
}
