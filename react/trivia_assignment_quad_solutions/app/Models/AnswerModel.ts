interface AnswerModel {
    questionId: string;
    givenAnswer: string;
    correctAnswer?: string;
    wasAnswerCorrect?: boolean;
}