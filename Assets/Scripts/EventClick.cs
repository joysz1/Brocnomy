using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EventClick : MonoBehaviour, IPointerClickHandler
{
    public Camera camera;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("sending ray");
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            // Do something with the object that was hit by the raycast.
            Destroy(objectHit.gameObject);
        }
    }
}