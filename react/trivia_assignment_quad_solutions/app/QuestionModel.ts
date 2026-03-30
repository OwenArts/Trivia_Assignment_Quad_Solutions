class QuestionModel {
    private questionId: string;
    private question: string;
    private answers: string[];


    constructor(Question: string, Answers : string[], QuestionId :string) {
        this.question = Question;
        this.questionId = QuestionId;
        this.answers = Answers;
    }
}