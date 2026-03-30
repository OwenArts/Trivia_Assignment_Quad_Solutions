"use client";

import Image from "next/image";
import {SetStateAction, useState} from "react";

function StartupForm() {
    const [amount, setAmount] = useState(1);

    const handleChange = (event: { target: { valueAsNumber: SetStateAction<number>; }; }) => {
        setAmount(event.target.valueAsNumber);
    };
    const handleSubmit = async (event: { preventDefault: () => void; }) => {
        event.preventDefault();
        const result: string | undefined = await CallForQuestionsAsync(amount)
        alert(JSON.stringify(result, null, 2));
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
                className={"hover:underline"}
                type="submit">
                Fetch Questions
            </button>
        </form>
    )
}

async function CallForQuestionsAsync(amount: number) {
    const url = `https://localhost:7211/Questions?Amount=${amount}`;
    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`Response status: ${response.status}`);
        }

        const result = await response.json();
        console.log(result);
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
