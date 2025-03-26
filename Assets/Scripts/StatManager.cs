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
        unemploymentText.text = "Unemployment% :" + unemployment;
    }

    public float getUnemployment()
    {
        return unemployment;
    }

}
