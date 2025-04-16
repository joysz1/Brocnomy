using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class DecisionLoader : MonoBehaviour
{
    [SerializeField] private DecisionDatabase decisionDatabase;

    private void Start()
    {
        LoadDecisions();
    }

    private void LoadDecisions()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Decisions/questions");
        if (jsonFile == null)
        {
            Debug.LogError("Could not load questions.json from Resources/Decisions folder");
            return;
        }

        DecisionDataWrapper wrapper = JsonUtility.FromJson<DecisionDataWrapper>(jsonFile.text);
        if (wrapper != null && wrapper.decisions != null)
        {
            decisionDatabase.decisions = wrapper.decisions;
            Debug.Log($"Successfully loaded {decisionDatabase.decisions.Length} decisions");
        }
        else
        {
            Debug.LogError("Failed to parse questions.json");
        }
    }

    [System.Serializable]
    private class DecisionDataWrapper
    {
        public Decision[] decisions;
    }
} 