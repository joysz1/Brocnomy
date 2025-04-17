using UnityEngine;
using System;

public enum WorldState
{
    Neutral,
    Chaotic,
    Utopian
}

public class WorldStateManager : MonoBehaviour
{
    public static WorldStateManager Instance { get; private set; }

    // Game variables
    public float CompanyProfit { get; private set; }
    public float Population { get; private set; }
    public float Pollution { get; private set; }
    public float StockMarket { get; private set; }

    // World state thresholds
    private const float MIN_PROFIT_NEUTRAL = 400000f;
    private const float MAX_PROFIT_NEUTRAL = 700000f;
    private const float MIN_POPULATION_NEUTRAL = 3000000000f;
    private const float MAX_POPULATION_NEUTRAL = 7000000000f;
    private const float MIN_POLLUTION_NEUTRAL = 30f;
    private const float MAX_POLLUTION_NEUTRAL = 70f;
    private const float MIN_STOCK_NEUTRAL = 4000f;
    private const float MAX_STOCK_NEUTRAL = 7000f;

    private const float CHAOS_POLLUTION_THRESHOLD = 80f;
    private const float CHAOS_POPULATION_THRESHOLD = 2000000000f;
    private const float CHAOS_STOCK_THRESHOLD = 3000f;

    private const float UTOPIA_PROFIT_THRESHOLD = 850000f;
    private const float UTOPIA_POPULATION_THRESHOLD = 7000000000f;
    private const float UTOPIA_POLLUTION_THRESHOLD = 20f;
    private const float UTOPIA_STOCK_THRESHOLD = 6000f;

    private int utopiaTurns = 0;
    private const int UTOPIA_TURNS_REQUIRED = 5;
    private int totalTurns = 0;
    private const int TOTAL_QUESTIONS = 20;

    public int state { get; set; }

    public WorldState CurrentWorldState { get; private set; }

    public event Action<WorldState> OnWorldStateChanged;
    public event Action<bool> OnGameEnded; // true for good ending, false for bad

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
        // Initialize starting values
        CompanyProfit = 500000f;
        Population = 5000000000f;
        Pollution = 50f;
        StockMarket = 5500f;
        
        UpdateWorldState();
    }

    public void UpdateVariables(float profitChange, float populationChange, float pollutionChange, float stockChange)
    {
        CompanyProfit = Mathf.Max(0, CompanyProfit + profitChange);
        Population = Mathf.Max(0, Population + populationChange);
        Pollution = Mathf.Clamp(Pollution + pollutionChange, 0, 100);
        StockMarket = Mathf.Max(0, StockMarket + stockChange);

        totalTurns++;
        UpdateWorldState();
        CheckEndConditions();
    }

    private void UpdateWorldState()
    {
        WorldState newState = DetermineWorldState();
        
        if (newState != CurrentWorldState)
        {
            CurrentWorldState = newState;
            OnWorldStateChanged?.Invoke(newState);
        }
    }

    private WorldState DetermineWorldState()
    {
        // Check for Chaotic state
        if (Pollution > CHAOS_POLLUTION_THRESHOLD ||
            Population < CHAOS_POPULATION_THRESHOLD ||
            StockMarket < CHAOS_STOCK_THRESHOLD)
        {
            Debug.Log("Chaotic State");

            state = 0;
            return WorldState.Chaotic;
        }

        // Check for Utopian state
        if (CompanyProfit > UTOPIA_PROFIT_THRESHOLD &&
            Population > UTOPIA_POPULATION_THRESHOLD &&
            Pollution < UTOPIA_POLLUTION_THRESHOLD &&
            StockMarket > UTOPIA_STOCK_THRESHOLD)
        {
            utopiaTurns++;
            if (utopiaTurns >= UTOPIA_TURNS_REQUIRED)
            {
                OnGameEnded?.Invoke(true);
            }
            state = 2;
            return WorldState.Utopian;
        }
        else
        {
            utopiaTurns = 0;
        }

        // Default to Neutral state

        Debug.Log("Neutral State");
        state = 1;
        return WorldState.Neutral;
    }

    private void CheckEndConditions()
    {
        // Check for Bad Ending
        if ((Pollution > 90f && Population < 1000000000f) || StockMarket < 2500f)
        {
            OnGameEnded?.Invoke(true);
            return;
        }

        // Check for end of all questions
        if (totalTurns >= TOTAL_QUESTIONS)
        {
            // Determine final state based on current conditions
            if (CurrentWorldState == WorldState.Utopian)
            {
                OnGameEnded?.Invoke(true);
            }
            else if (CurrentWorldState == WorldState.Chaotic)
            {
                OnGameEnded?.Invoke(true);
            }
            else
            {
                // Neutral ending
                OnGameEnded?.Invoke(true);
            }
        }
    }

    public void ResetGame()
    {
        CompanyProfit = 500000f;
        Population = 5000000000f;
        Pollution = 50f;
        StockMarket = 5500f;
        totalTurns = 0;
        utopiaTurns = 0;
        UpdateWorldState();
    }
} 