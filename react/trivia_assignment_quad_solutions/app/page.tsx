"use client";

import Image from "next/image";
import {SetStateAction, useState} from "react";
import {useRouter} from "next/navigation";
import {NextRouter} from "next/router";

function StartupForm() {
    const [amount, setAmount] = useState(1);
    const [isLoading, setIsLoading] = useState(false);
    const router = useRouter();

    const handleChange = (event: { target: { valueAsNumber: SetStateAction<number>; }; }) => {
        setAmount(event.target.valueAsNumber);
    };
    const handleSubmit = async (event: { preventDefault: () => void; }) => {
        event.preventDefault();
        setIsLoading(true);

        try {
            // @ts-ignore
            await CallForQuestionsAsync(amount, router)
        } catch (e) {
            console.error("Error occured while fetching:", e);
            alert("Something went wrong with fetching the questions, try again.");
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <label>Enter the number of trivia questions you wish to receive:
                <input
                    type="number"
                    value={amount}
                    min={1} max={100} step={1}
                    onChange={handleChange}
                />
            </label>
            <br/>
            <br/>
            <button
                type="submit"
                disabled={isLoading}
                className="bg-blue-500 text-white p-2 rounded hover:bg-blue-600 disabled:bg-gray-400"
            >
                {isLoading ? "loading..." : "Start with the questions!"}
            </button>
        </form>
    )
}

async function CallForQuestionsAsync(amount: number, router: NextRouter) {
    const url = `https://localhost:7211/Questions?Amount=${amount}`;
    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`Response status: ${response.status}`);
        }

        const result: QuestionModel = await response.json();
        localStorage.setItem("quiz_questions", JSON.stringify(result));

        await router.push("/Questions");
        return result;
    } catch (error) {
        // @ts-ignore
        console.error(error.message);
    }
}

export default function Home() {
    return (
        <div className="flex flex-col flex-1 items-center justify-center bg-zinc-50 font-sans dark:bg-black">
            <main
                className="flex flex-1 w-full max-w-3xl flex-col items-center justify-between py-32 px-16 bg-white dark:bg-black sm:items-start">
                <Image
                    className="dark:invert"
                    src="/next.svg"
                    alt="Next.js logo"
                    width={100}
                    height={20}
                    priority
                />
                <div className="flex flex-col items-center gap-6 text-center sm:items-start sm:text-left">
                    <StartupForm/>
                </div>
            </main>
        </div>
    );
}
