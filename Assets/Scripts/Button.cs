using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    [SerializeField]
    private StatManager statManager;

    public void yesPress()
    {
        statManager.changeMoney(10);
        statManager.changeUnemployment(10);
    }

    public void noPress()
    {
        statManager.changeMoney(-10);
        statManager.changeUnemployment(-10);
    }

    public void restart()
    {
        SceneManager.LoadScene("MainGame");
    }
}
