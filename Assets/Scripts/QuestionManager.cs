using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QuestionManager : MonoBehaviour
{
    public Text questionText;
    public Text scoreText;
    public Text FinalScore;
    public Button[] replyButtons;
    public QtsData qtsData; // Reference to the scriptable object
    public GameObject Right;
    public GameObject Wrong;
    public GameObject GameFinished;

    public TMP_InputField answerInputField; // ⚡ Подключи InputField сюда через инспектор
    public Button submitButton; // Кнопка "Подтвердить ответ"


    private int currentQuestion = 0;
    private static int score = 0;

    void Start()
    {
        SetQuestion(currentQuestion);
        Right.gameObject.SetActive(false);
        Wrong.gameObject.SetActive(false);
        GameFinished.gameObject.SetActive(false);
    }

    void SetQuestion(int questionIndex)
{
    var question = qtsData.questions[questionIndex];
    questionText.text = question.questionText;

    // Remove previous listeners
    foreach (Button r in replyButtons)
    {
        r.onClick.RemoveAllListeners();
    }

    answerInputField.gameObject.SetActive(false);
    submitButton.gameObject.SetActive(false);

    if (question.questionType == QtsData.QuestionType.MultipleChoice)
    {
        for (int i = 0; i < replyButtons.Length; i++)
        {
            replyButtons[i].gameObject.SetActive(true);
            replyButtons[i].GetComponentInChildren<Text>().text = question.replies[i];
            int replyIndex = i;
            replyButtons[i].onClick.AddListener(() =>
            {
                CheckReply(replyIndex);
            });
        }
    }
    else if (question.questionType == QtsData.QuestionType.TrueFalse)
    {
        replyButtons[0].gameObject.SetActive(true);
        replyButtons[1].gameObject.SetActive(true);
        replyButtons[2].gameObject.SetActive(false);
        replyButtons[3].gameObject.SetActive(false);

        replyButtons[0].GetComponentInChildren<Text>().text = "True";
        replyButtons[1].GetComponentInChildren<Text>().text = "False";

        replyButtons[0].onClick.AddListener(() => CheckTrueFalse(true));
        replyButtons[1].onClick.AddListener(() => CheckTrueFalse(false));
    }
    else if (question.questionType == QtsData.QuestionType.WordGame)
    {
        foreach (Button r in replyButtons)
        {
            r.gameObject.SetActive(false);
        }

        answerInputField.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(true);

        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(CheckWordAnswer);
    }
}


    void CheckReply(int replyIndex)
{
    if (replyIndex == qtsData.questions[currentQuestion].correctReplyIndex)
    {
        score++;
        scoreText.text = "" + score;
        Right.gameObject.SetActive(true);
    }
    else
    {
        Wrong.gameObject.SetActive(true);
    }

    foreach (Button r in replyButtons)
    {
        r.interactable = false;
    }

    StartCoroutine(Next());
}

    void CheckTrueFalse(bool playerAnswer)
{
    if (playerAnswer == qtsData.questions[currentQuestion].correctTrueFalse)
    {
        score++;
        scoreText.text = "" + score;
        Right.gameObject.SetActive(true);
    }
    else
    {
        Wrong.gameObject.SetActive(true);
    }

    foreach (Button r in replyButtons)
    {
        r.interactable = false;
    }

    StartCoroutine(Next());
}


    IEnumerator Next()
    {
        yield return new WaitForSeconds(2);

        currentQuestion++;

        if (currentQuestion < qtsData.questions.Length)
        {
            // Reset for the next question
            Reset();
        }
        else
        {
            // Game finished
            GameFinished.SetActive(true);

            float scorePercentage = (float)score / qtsData.questions.Length * 100f;
            FinalScore.text = "You scored " + scorePercentage.ToString("F0") + "%";

            if (scorePercentage < 50)
            {
                FinalScore.text += "\nGame Over";
            }
            else if (scorePercentage < 60)
            {
                FinalScore.text += "\nKeep Trying";
            }
            else if (scorePercentage < 70)
            {
                FinalScore.text += "\nGood Job";
            }
            else if (scorePercentage < 80)
            {
                FinalScore.text += "\nWell Done!";
            }
            else
            {
                FinalScore.text += "\nYou're a genius!";
            }
        }
    }

    public void Reset()
    {
        // Hide feedback panels
        Right.SetActive(false);
        Wrong.SetActive(false);

        // Enable all reply buttons
        foreach (Button r in replyButtons)
        {
            r.interactable = true;
        }

        answerInputField.text = "";
        answerInputField.interactable = true;
        submitButton.interactable = true;

        // Set the next question
        SetQuestion(currentQuestion);
    }


    void CheckWordAnswer()
{
    string playerAnswer = answerInputField.text.Trim().ToLower();
    string correctAnswer = qtsData.questions[currentQuestion].correctWordAnswer.Trim().ToLower();

    if (playerAnswer == correctAnswer)
    {
        score++;
        scoreText.text = "" + score;
        Right.gameObject.SetActive(true);
    }
    else
    {
        Wrong.gameObject.SetActive(true);
    }

    answerInputField.interactable = false;
    submitButton.interactable = false;

    StartCoroutine(Next());
}
}

