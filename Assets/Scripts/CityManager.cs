using UnityEngine;

public class CityManager : MonoBehaviour
{

    public static CityManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void moveToVisPos(Transform visPos)
    {
        this.transform.position = visPos.position;
    }

    public void movetoInvisPos(Transform invisPos)
    {
        this.transform.position = invisPos.position;
    }
}
