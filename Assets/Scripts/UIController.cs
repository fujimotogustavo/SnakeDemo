using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GridController gridController;
    [SerializeField] private TMP_Text textElement;
    [SerializeField] private TMP_Text scoreTMP;

    public void DisplayGridInString()
    {
        string fullGridString = "";

        for (int y = 0; y < GridController.cols; y++)
        {
            for (int x = 0; x < GridController.rows; x++)
            {
                switch (gridController.gridArr[x, y])
                {
                    case SquareType.Empty:
                        fullGridString += "- ";
                        break;

                    case SquareType.Snake:
                        fullGridString += "S ";
                        break;

                    case SquareType.Fruit:
                        fullGridString += "F ";
                        break;
                }
            }
            fullGridString += "\n";
        }


        textElement.text = fullGridString;
    }

    public void ChangeTextElementString(string receivedString) {
        textElement.text = receivedString;
    }
    public void ChangeScoreText(string receivedString)
    {
        scoreTMP.text = receivedString;
    }

}
