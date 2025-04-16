using UnityEngine;
using System;

public class DecisionManager : MonoBehaviour
{
    public static DecisionManager Instance { get; private set; }

    [SerializeField] private DecisionDatabase decisionDatabase;
    
    public event Action<Decision> OnDecisionPresented;
    public event Action OnDecisionsComplete;

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
        //PresentNextDecision();
    }

    public void PresentNextDecision()
    {
        if (decisionDatabase.HasMoreDecisions())
        {
            Decision currentDecision = decisionDatabase.GetCurrentDecision();
            OnDecisionPresented?.Invoke(currentDecision);
        }
        else
        {
            OnDecisionsComplete?.Invoke();
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        Decision currentDecision = decisionDatabase.GetCurrentDecision();
        if (currentDecision != null && choiceIndex >= 0 && choiceIndex < currentDecision.choices.Length)
        {
            ChoiceImpact impact = currentDecision.choices[choiceIndex];
            WorldStateManager.Instance.UpdateVariables(
                impact.profitChange,
                impact.populationChange,
                impact.pollutionChange,
                impact.stockChange
            );

            decisionDatabase.MoveToNextDecision();
            PresentNextDecision();
        }
    }

    public void ResetDecisions()
    {
        decisionDatabase.ResetDecisions();
    }
} 