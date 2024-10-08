using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GridController gridController;
    [SerializeField] private TMP_Text textElement;
    [SerializeField] private TMP_Text scoreTMP;

    public void ChangeTextElementString(string receivedString) {
        textElement.text = receivedString;
    }
    public void ChangeScoreText(string receivedString)
    {
        scoreTMP.text = receivedString;
    }

}
