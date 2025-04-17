using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceButton : MonoBehaviour
{
    [SerializeField]
    private int choiceIndex;


    public void ButtonPressed()
    {
        UIManager.Instance.SetDecisionPanelInactive();
        DecisionManager.Instance.MakeChoice(choiceIndex);
    }
    
}
