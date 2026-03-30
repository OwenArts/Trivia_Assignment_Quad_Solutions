class AnswerModel {
    private questionId: string;
    private givenAnswer: string;
    private correctAnswer: string;
    private wasAnswerCorrect: boolean;

    constructor(questionId: string, givenAnswer: string, correctAnswer = "", wasAnswerCorrect = false) {
        this.questionId = questionId;
        this.givenAnswer = givenAnswer;
        this.correctAnswer = correctAnswer;
        this.wasAnswerCorrect = wasAnswerCorrect;
    }
}