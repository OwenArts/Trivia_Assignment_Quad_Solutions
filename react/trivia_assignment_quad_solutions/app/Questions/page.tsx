"use client";
import {useEffect, useState} from "react";
import {useRouter} from "next/navigation";

async function checkAnswerWithBackend(questionId: string, selectedAnswer: string) {
    const answer: AnswerModel = {
        questionId: questionId,
        givenAnswer: selectedAnswer,
        correctAnswer: ""
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
        console.error(error.message);
    }
}

export default function QuestionsPage() {
    const [questions, setQuestions] = useState<QuestionModel[]>([]);
    const [results, setResults] = useState<Record<string, AnswerModel>>({});
    const router = useRouter();

    useEffect(() => {
        const saved = localStorage.getItem("quiz_questions");
        if (saved) {
            setQuestions(JSON.parse(saved));
        }
    }, []);

    const handleAnswerSelection = async (questionId: string, selectedAnswer: string) => {
        if (results[questionId]) {
            return;
        }

        const responseData = await checkAnswerWithBackend(questionId, selectedAnswer);

        if (responseData && responseData.length > 0) {
            setResults(prev => ({
                ...prev,
                [questionId]: responseData[0]
            }));
        }
    };

    return (
        <main className="p-8 max-w-4xl mx-auto">
            <h1 className="text-2xl font-bold mb-6">Answer the following questions:</h1>
            {questions.map((q) => {
                const result = results[q.questionId];
                const isAnswered = !!result;

                return (
                    <div key={q.questionId} className="mt-4 p-4 border rounded">
                        <p className="font-semibold" dangerouslySetInnerHTML={{__html: q.question}}/>

                        <div className="flex flex-wrap gap-3 mt-4">
                            {q.answers.map((answer) => {
                                let buttonClass = "bg-blue-500 text-white";

                                if (isAnswered) {
                                    if (answer === result.correctAnswer) {
                                        buttonClass = "bg-green-500 text-white font-bold";
                                    } else if (answer === result.givenAnswer && !result.wasAnswerCorrect) {
                                        buttonClass = "bg-red-500 text-white";
                                    } else {
                                        buttonClass = "bg-gray-300 text-gray-500 opacity-50";
                                    }
                                }

                                return (
                                    <button
                                        key={answer}
                                        type="button"
                                        disabled={isAnswered}
                                        className={`${buttonClass} px-4 py-2 rounded transition-colors shadow-sm`}
                                        dangerouslySetInnerHTML={{__html: answer}}
                                        onClick={() => handleAnswerSelection(q.questionId, answer)}
                                    />
                                );
                            })}
                        </div>
                    </div>
                );
            })}
            <button
                type="button"
                className="text-blue-500 underline font-bold mt-6 p-4 border-2 rounded"
                onClick={() => router.push("/")}
            >
                Get new questions
            </button>
        </main>
    );
}