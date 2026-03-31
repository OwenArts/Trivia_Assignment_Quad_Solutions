interface AnswerModel {
    QuestionId: string;
    GivenAnswer: string;
    CorrectAnswer?: string;
    WasAnswerCorrect?: boolean;
}