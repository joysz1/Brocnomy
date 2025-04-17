using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            this.SendRay();
        }
    }

    private void SendRay()
    {
        Debug.Log("mouse down");
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            hitInfo.collider.GetComponent<ChoiceButton>().ButtonPressed();
            Debug.Log("pressing button");
        }
    }
}
