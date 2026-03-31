"use client";
import {useEffect, useState} from "react";

async function checkAnswerWithBackend(questionId: string, selectedAnswer: string) {
    const answer: AnswerModel = {
        QuestionId: questionId,
        GivenAnswer: selectedAnswer,
        CorrectAnswer: ""
    };

    const url = `https://localhost:7211/CheckAnswers`;
    try {
        const response = await fetch(url, {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify([answer]),
        });
        if (!response.ok) {
            throw new Error(`Response status: ${response.status}`);
        }

        const result: AnswerModel[] | undefined = await response.json();

        return result;
    } catch (error) {
        // @ts-ignore
        console.error(error.message);
    }
}

export default function QuestionsPage() {
    const [questions, setQuestions] = useState<QuestionModel[]>([]);

    useEffect(() => {
        const saved = localStorage.getItem("quiz_questions");
        if (saved) {
            setQuestions(JSON.parse(saved));
        }
    }, []);

    const handleAnswerSelection = (questionId: string, selectedAnswer: string) => {
        console.log("Question ID:", questionId);
        console.log("Selected Answer:", selectedAnswer);

        checkAnswerWithBackend(questionId, selectedAnswer);

        alert(`Je koos: ${selectedAnswer} voor vraag ${questionId}`);
    };

    return (
        <main className="p-8">
            <h1 className="text-2xl font-bold">Answer the following questions:</h1>
            {questions.map((q: QuestionModel, index: number) => (
                <div key={index} className="mt-4 p-4 border rounded">
                    <p className="font-semibold"
                       dangerouslySetInnerHTML={{ __html: q.question }}
                    />
                    <div className="flex flex-wrap gap-3 mt-4">
                        {q.answers.map((answer) => (
                            <button
                                key={answer}
                                type="button"
                                className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition-colors shadow-sm"
                                dangerouslySetInnerHTML={{ __html: answer }}
                                onClick={() => handleAnswerSelection(q.questionId, answer)}
                            />
                        ))}
                    </div>
                </div>
            ))}
        </main>
    );
}