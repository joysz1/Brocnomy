using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance;

    [SerializeField]
    private float money;
    [SerializeField]
    private float unemployment;
    [SerializeField]
    private TMP_Text moneyText;
    [SerializeField]
    private TMP_Text unemploymentText;
    //[SerializeField]
    //private CityManager cityManager;
    [SerializeField]
    private GameObject firewall;
    [SerializeField]
    private Transform firewallVisPos;
    [SerializeField]
    private Transform firewallInvisPos;

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

    void Start()
    {
        unemploymentText.text = "Unemployment% :" + unemployment;
        moneyText.text = "Money :" + money;
    }

    void Update()
    {
        //check if press yes or no
        if (Input.GetKeyDown(KeyCode.Y))
        {
            changeMoney(10);
            changeUnemployment(10);
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            changeMoney(-10);
            changeUnemployment(-10);
        }
    }

    public void changeMoney(float newMoney)
    {
        money += newMoney;
        if (money < 0.01)
        {
            SceneManager.LoadScene("Lose");
        }
        moneyText.text = "Money :" + money;
    }

    public float getMoney()
    {
        return money;
    }

    public void changeUnemployment(float newUnemployment)
    {
        unemployment += newUnemployment;
        if (unemployment > 99.99)
        {
            SceneManager.LoadScene("Lose");
        }
        else if (unemployment > 60)
        {
            moveToVisPos(firewall.transform, firewallVisPos);
        } else if (unemployment < 40)
        {
            movetoInvisPos(firewall.transform, firewallInvisPos);
        }
        unemploymentText.text = "Unemployment% :" + unemployment;
    }

    public float getUnemployment()
    {
        return unemployment;
    }

    public void moveToVisPos(Transform obj, Transform visPos)
    {
        obj.position = visPos.position;
    }

    public void movetoInvisPos(Transform obj, Transform invisPos)
    {
        obj.position = invisPos.position;
    }
}
