using UnityEngine;

[CreateAssetMenu(fileName = "New QuestionData", menuName = "QuestionData")]
public class QtsData : ScriptableObject
{
    public enum QuestionType
    {
        MultipleChoice, // 4 варианта
        TrueFalse,       // True/False
        WordGame 
    }

    [System.Serializable]
    public class Question
    {
        public QuestionType questionType; // Тип вопроса
        public string questionText;
        public string[] replies;          // Только для MultipleChoice
        public int correctReplyIndex;     // Только для MultipleChoice
        public bool correctTrueFalse;     // Только для TrueFalse
        public string correctWordAnswer; // WordGame
    }

    public Question[] questions;
}
