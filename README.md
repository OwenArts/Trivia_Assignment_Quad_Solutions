# Trivia Assignment - Quad Solutions

Een interactieve trivia-applicatie gebouwd met een **ASP.NET Core Web API** backend en een **Next.js (React)** frontend. De applicatie haalt vragen op via de OpenTDB API, beheert sessies in de backend en valideert antwoorden real-time.

## Aan de slag
Volg deze stappen om de applicatie lokaal te draaien.

### 1. Vereisten
* [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
* [Node.js (v24)](https://nodejs.org/)
* [npm](https://www.npmjs.com/)

---

### 2. Backend aan de gang krijgen (C#)
De API verzorgt de logica, de sessies en de validatie van de antwoorden.

1. Navigeer naar de backend map vanuit de main map:
   ```bash
   cd CSharp\Trivia Assignment backend
2. Herstel de NuGet packages:
   ```bash
   dotnet restore
3. Start de API:
   ```bash
    dotnet run --launch-profile "https"

De API draait standaard op: https://localhost:7211

### 3. Frontend aan de gang krijgen (Next.js)
De frontend is een moderne React applicatie die communiceert met de API.

1. Navigeer naar de frontend map vanuit de main map:
   ```bash
   cd react\trivia_assignment_quad_solutions

2. Installeer de dependencies:
   ```bash
    npm install

3. Start de development server:
    ```bash
    npm run dev

De frontend is nu bereikbaar op: http://localhost:3000

### Gebruikte Technieken
- Backend: ASP.NET Core Web API, C#, Dependency Injection, Session Management.
- Frontend: Next.js (App Router), TypeScript, Tailwind CSS, Fetch API.
- Data: [OpenTDB](https://opentdb.com/api_config.php) (Open Trivia Database).

### Belangrijke opmerkingen
- CORS: De backend is geconfigureerd om requests van http://localhost:3000 te accepteren.
- Local Storage: De opgehaalde vragen worden tijdelijk in de browser opgeslagen zodat de quizervaring vloeiend blijft bij het navigeren.
- Validatie: Antwoorden worden per stuk naar de backend gestuurd voor verificatie (POST /CheckAnswers).
- Zodra een antwoord door de backend is gevalideerd wordt deze vraag uit de backend verwijderd en kan dus niet opnieuw worden opgevraagd.
- Ontwikkeld als onderdeel van het assessment voor Quad Solutions.
