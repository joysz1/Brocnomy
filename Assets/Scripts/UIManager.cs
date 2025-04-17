using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    /**
    [Header("Variable Displays")]
    [SerializeField] private Slider profitSlider;
    [SerializeField] private Slider populationSlider;
    [SerializeField] private Slider pollutionSlider;
    [SerializeField] private Slider stockSlider;

    [Header("Decision UI")]
    [SerializeField] private GameObject decisionPanel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button[] choiceButtons;

    [Header("End Game UI")]
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TextMeshProUGUI endGameText;
    */
    [SerializeField]
    private Material chaoticSkybox;
    [SerializeField]
    private GameObject chaoticCity;
    [SerializeField]
    private Material neutralSkybox;
    [SerializeField] 
    private GameObject neutralCity;
    [SerializeField]
    private Material utopianSkybox;
    [SerializeField]
    private GameObject utopianCity;
    [SerializeField]
    private GameObject cityActivePos;
    [SerializeField]
    private GameObject cityInactivePos;

    private GameObject decisionPanel;
    private TextMeshProUGUI questionText;
    private TextMeshProUGUI[] choiceButtonTexts = new TextMeshProUGUI[2];

    private TextMeshProUGUI profitText;
    private TextMeshProUGUI populationText;
    private TextMeshProUGUI pollutionText;
    private TextMeshProUGUI stockText;

    private GameObject endGamePanel;
    private TextMeshProUGUI endGameText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Subscribe to events
        //GameObject newMonitor = Instantiate(monitor);
        chaoticCity = Instantiate(chaoticCity);
        chaoticCity.transform.position = cityInactivePos.transform.position;
        neutralCity = Instantiate(neutralCity);
        utopianCity = Instantiate(utopianCity);
        utopianCity.transform.position = cityInactivePos.transform.position;

        RenderSettings.skybox = neutralSkybox;

        WorldStateManager.Instance.OnWorldStateChanged += UpdateOfficeBackground;
        WorldStateManager.Instance.OnGameEnded += ShowEndGameScreen;
        DecisionManager.Instance.OnDecisionPresented += ShowDecision;
        DecisionManager.Instance.OnDecisionsComplete += OnDecisionsComplete;

        decisionPanel = GameObject.Find("DecisionPanel").gameObject;
        questionText = GameObject.Find("QuestionText").gameObject.GetComponent<TextMeshProUGUI>();
        choiceButtonTexts[0] = GameObject.Find("Button1Text").gameObject.GetComponent<TextMeshProUGUI>();
        //choiceButtons[1] = GameObject.Find("Button3").gameObject.GetComponent<Button>();
        //choiceButtonTexts[1] = GameObject.Find("Button3Text").gameObject.GetComponent<TextMeshProUGUI>();
        choiceButtonTexts[1] = GameObject.Find("Button2Text").gameObject.GetComponent<TextMeshProUGUI>();
        profitText = GameObject.Find("ProfitCounter").gameObject.GetComponent<TextMeshProUGUI>();
        populationText = GameObject.Find("PopulationCounter").gameObject.GetComponent<TextMeshProUGUI>();
        pollutionText = GameObject.Find("PollutionCounter").gameObject.GetComponent<TextMeshProUGUI>();
        stockText = GameObject.Find("StockCounter").gameObject.GetComponent<TextMeshProUGUI>();

        endGamePanel = GameObject.Find("EndgamePanel").gameObject;
        endGameText = GameObject.Find("EndgameText").gameObject.GetComponent<TextMeshProUGUI>();


        // Initialize UI
        UpdateVariableDisplays();
        decisionPanel.SetActive(false);
        endGamePanel.SetActive(false);

        DecisionManager.Instance.PresentNextDecision();
    }

    private void Update()
    {
        UpdateVariableDisplays();
    }

    private void UpdateVariableDisplays()
    {
        profitText.text = (WorldStateManager.Instance.CompanyProfit.ToString());
        populationText.text = (WorldStateManager.Instance.Population.ToString("0." + new string('#', 339)));
        pollutionText.text = (WorldStateManager.Instance.Pollution.ToString() + "%");
        stockText.text = (WorldStateManager.Instance.StockMarket.ToString());
    }

    private void ShowDecision(Decision decision)
    {
        Debug.Log("showing decision");
        decisionPanel.SetActive(true);
        questionText.text = decision.questionText;

        for (int i = 0; i < choiceButtonTexts.Length; i++)
        {
            if (i < decision.choices.Length)
            {
                choiceButtonTexts[i].text = decision.choices[i].choiceText;
                
            }
        }
    }

    public void SetDecisionPanelInactive()
    {
        decisionPanel.SetActive(false);
    }

    private void UpdateOfficeBackground(WorldState newState)
    {
        /*
        sitch (newState)
        {
            case WorldState.Neutral:
                officeBackground.sprite = neutralOffice;
                break;
            case WorldState.Chaotic:
                officeBackground.sprite = chaoticOffice;
                break;
            case WorldState.Utopian:
                officeBackground.sprite = utopianOffice;
                break;
        }
        */
    }

    private void ShowEndGameScreen(bool dummy)
    {
        int state = WorldStateManager.Instance.state;
        endGamePanel.SetActive(true);
        if (state == 0)
        {
            endGameText.text = "Game Over! The world has fallen into chaos... but at least you're rich!";
        } else if (state == 1)
        {
            endGameText.text = "Game Over! The world is okay, but your company isn't doing great...";
        } else
        {
            endGameText.text = "Congratulations! You've created a Utopian future!";
        }
    }

    private void OnDecisionsComplete()
    {
        // Handle when all decisions are complete
        ShowEndGameScreen(WorldStateManager.Instance.CurrentWorldState == WorldState.Utopian);
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (WorldStateManager.Instance != null)
        {
            WorldStateManager.Instance.OnWorldStateChanged -= UpdateOfficeBackground;
            WorldStateManager.Instance.OnGameEnded -= ShowEndGameScreen;
        }
        if (DecisionManager.Instance != null)
        {
            DecisionManager.Instance.OnDecisionPresented -= ShowDecision;
            DecisionManager.Instance.OnDecisionsComplete -= OnDecisionsComplete;
        }
    }

    public void SetChaotic()
    {
        chaoticCity.transform.position = cityActivePos.transform.position;
        neutralCity.transform.position = cityInactivePos.transform.position;
        utopianCity.transform.position = cityInactivePos.transform.position;
        RenderSettings.skybox = chaoticSkybox;
    }

    public void SetNeutral()
    {
        chaoticCity.transform.position = cityInactivePos.transform.position;
        neutralCity.transform.position = cityActivePos.transform.position;
        utopianCity.transform.position = cityInactivePos.transform.position;
        RenderSettings.skybox = neutralSkybox;
    }

    public void SetUtopian()
    {
        chaoticCity.transform.position = cityInactivePos.transform.position;
        neutralCity.transform.position = cityInactivePos.transform.position;
        utopianCity.transform.position = cityActivePos.transform.position;
        RenderSettings.skybox = utopianSkybox;
    }
} 