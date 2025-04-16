using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private DecisionDatabase decisionDatabase;

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
        InitializeGame();
    }

    private void InitializeGame()
    {
        // Ensure all managers are properly initialized
        if (UIManager.Instance == null)
        {
            GameObject uiManager = new GameObject("UIManager");
            uiManager.AddComponent<UIManager>();
        }

        if (WorldStateManager.Instance == null)
        {
            GameObject worldStateManager = new GameObject("WorldStateManager");
            worldStateManager.AddComponent<WorldStateManager>();
        }

        if (DecisionManager.Instance == null)
        {
            GameObject decisionManager = new GameObject("DecisionManager");
            decisionManager.AddComponent<DecisionManager>();
        }

        // Reset the game state
        WorldStateManager.Instance.ResetGame();
        DecisionManager.Instance.ResetDecisions();
    }

    public void ResetGame()
    {
        InitializeGame();
    }
} 