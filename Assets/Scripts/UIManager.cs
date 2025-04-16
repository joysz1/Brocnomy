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

    [Header("Office Background")]
    [SerializeField] private Image officeBackground;
    [SerializeField] private Sprite neutralOffice;
    [SerializeField] private Sprite chaoticOffice;
    [SerializeField] private Sprite utopianOffice;

    [SerializeField] private GameObject monitor;

    private Slider profitSlider;
    private Slider populationSlider;
    private Slider pollutionSlider;
    private Slider stockSlider;

    private GameObject decisionPanel;
    private TextMeshProUGUI questionText;
    private Button[] choiceButtons = new Button[3];
    private TextMeshProUGUI[] choiceButtonTexts = new TextMeshProUGUI[3];

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
        GameObject newMonitor = Instantiate(monitor);

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
        choiceButtons[1] = GameObject.Find("Button3").gameObject.GetComponent<Button>();
        choiceButtonTexts[1] = GameObject.Find("Button3Text").gameObject.GetComponent<TextMeshProUGUI>();
        choiceButtons[2] = GameObject.Find("Button2").gameObject.GetComponent<Button>();
        choiceButtonTexts[2] = GameObject.Find("Button2Text").gameObject.GetComponent<TextMeshProUGUI>();
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
        populationSlider.value = WorldStateManager.Instance.Population / 10000000000f; // Normalize to 0-1
        pollutionSlider.value = WorldStateManager.Instance.Pollution / 100f;
        stockSlider.value = WorldStateManager.Instance.StockMarket / 10000f; // Normalize to 0-1
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
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtonTexts[i].text = decision.choices[i].choiceText;

                int choiceIndex = i; // Capture the index for the lambda
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(choiceIndex));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        decisionPanel.SetActive(false);
        DecisionManager.Instance.MakeChoice(choiceIndex);
    }

    private void UpdateOfficeBackground(WorldState newState)
    {
        /*
        switch (newState)
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

    private void ShowEndGameScreen(bool isGoodEnding)
    {
        endGamePanel.SetActive(true);
        endGameText.text = isGoodEnding ? 
            "Congratulations! You've created a Utopian future!" : 
            "Game Over! The world has fallen into chaos...";
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