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
    private GameObject buttonOne;
    [SerializeField]
    private GameObject buttonTwo;

    private Slider profitSlider;
    private Slider populationSlider;
    private Slider pollutionSlider;
    private Slider stockSlider;

    private GameObject decisionPanel;
    private TextMeshProUGUI questionText;
    private Button[] choiceButtons = new Button[2];
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

        WorldStateManager.Instance.OnWorldStateChanged += UpdateOfficeBackground;
        WorldStateManager.Instance.OnGameEnded += ShowEndGameScreen;
        DecisionManager.Instance.OnDecisionPresented += ShowDecision;
        DecisionManager.Instance.OnDecisionsComplete += OnDecisionsComplete;

        profitSlider = GameObject.Find("Profit").gameObject.GetComponent<Slider>();
        populationSlider = GameObject.Find("Population").gameObject.GetComponent<Slider>();
        pollutionSlider = GameObject.Find("Pollution").gameObject.GetComponent<Slider>();
        stockSlider = GameObject.Find("Stock").gameObject.GetComponent<Slider>();
        decisionPanel = GameObject.Find("DecisionPanel").gameObject;
        questionText = GameObject.Find("QuestionText").gameObject.GetComponent<TextMeshProUGUI>();
        choiceButtons[0] = GameObject.Find("Button1").gameObject.GetComponent<Button>();
        choiceButtonTexts[0] = GameObject.Find("Button1Text").gameObject.GetComponent<TextMeshProUGUI>();
        //choiceButtons[1] = GameObject.Find("Button3").gameObject.GetComponent<Button>();
        //choiceButtonTexts[1] = GameObject.Find("Button3Text").gameObject.GetComponent<TextMeshProUGUI>();
        choiceButtons[1] = GameObject.Find("Button2").gameObject.GetComponent<Button>();
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
        profitSlider.value = WorldStateManager.Instance.CompanyProfit / 1000000f; // Normalize to 0-1
        profitText.text = (WorldStateManager.Instance.CompanyProfit.ToString());
        populationSlider.value = WorldStateManager.Instance.Population / 10000000000f; // Normalize to 0-1
        populationText.text = (WorldStateManager.Instance.Population.ToString("0." + new string('#', 339)));
        pollutionSlider.value = WorldStateManager.Instance.Pollution / 100f;
        pollutionText.text = (WorldStateManager.Instance.Pollution.ToString() + "%");
        stockSlider.value = WorldStateManager.Instance.StockMarket / 10000f; // Normalize to 0-1
        stockText.text = (WorldStateManager.Instance.StockMarket.ToString());
    }

    private void ShowDecision(Decision decision)
    {
        Debug.Log("showing decision");
        decisionPanel.SetActive(true);
        questionText.text = decision.questionText;

        for (int i = 0; i < choiceButtons.Length; i++)
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
} 