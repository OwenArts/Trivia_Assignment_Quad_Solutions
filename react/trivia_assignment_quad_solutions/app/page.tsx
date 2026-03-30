"use client";

import Image from "next/image";
import {useState} from "react";

function StartupForm() {
    const [amount, setAmount] = useState(1);

    return (
        <form>
            <label>Enter the number of trivia questions you wish to receive:
                <input
                    type="number"
                    value={amount}
                    min={1} max={100} step={1}
                    onChange={(e) => setAmount(e.target.valueAsNumber)}
                />
            </label>
            <br/>
            <br/>
            <button
                className={"hover:underline"}
                type="submit"
                onClick={CallForQuestions}>
                Fetch Questions
            </button>
        </form>
    )
}

function CallForQuestions(){
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
