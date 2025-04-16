using UnityEngine;

[System.Serializable]
public class ChoiceImpact
{
    public float profitChange;
    public float populationChange;
    public float pollutionChange;
    public float stockChange;
    public string choiceText;
}

[System.Serializable]
public class Decision
{
    public string questionText;
    public ChoiceImpact[] choices;
}

[CreateAssetMenu(fileName = "DecisionDatabase", menuName = "CEO of Tomorrow/Decision Database")]
public class DecisionDatabase : ScriptableObject
{
    public Decision[] decisions;
    private int currentDecisionIndex = 0;

    public Decision GetCurrentDecision()
    {
        if (currentDecisionIndex < decisions.Length)
        {
            return decisions[currentDecisionIndex];
        }
        return null;
    }

    public void MoveToNextDecision()
    {
        currentDecisionIndex++;
    }

    public void ResetDecisions()
    {
        currentDecisionIndex = 0;
    }

    public bool HasMoreDecisions()
    {
        return currentDecisionIndex < decisions.Length;
    }
} 