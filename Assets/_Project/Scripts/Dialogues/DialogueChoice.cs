using TMPro;
using UnityEngine;

public class DialogueChoice : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _choiceText;

    public TextMeshProUGUI ChoiceText {get => _choiceText; set => _choiceText = value; }
    public int ChoiceIndex { get; set; }

    public void OnChoiceClicked()
    {
        DialogueManager.Instance.MakeChoice(ChoiceIndex);
    }

}
