using System.Collections.Generic;
public class Subtraction
{
    public class Operation
    {
        public int Number1 { get; }
        public int Number2 { get; }
        public int Result { get; }
        public Operation(int n1, int n2)
        {
            Number1 = n1;
            Number2 = n2;
            Result = n1 - n2;
        }
        public override string ToString()
        {
            return $"{Number1} - {Number2} = ?";
        }
    } 

    private readonly List<Operation> _operations = new List<Operation>();
    private static readonly Random _random = new Random();

    private void GenerateOperationRandom(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int n1 = _random.Next(1, 101);
            int n2 = _random.Next(1, 101);
            _operations.Add(new Operation(n1, n2)); // Aggiungo l'operazione alla lista
        }
    }

    private readonly RegToChrono _gState;

    public Subtraction(RegToChrono gState)
    {
        _gState = gState;
        _operations = new List<Operation>();
        GenerateOperationRandom(3);
    }

    int points = 0;

    public void StartSubtractionGame()
    {
        string currentDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        _gState.GetPlayerName(currentDate); // Aggiungo la data e l'ora del gioco alla lista
        Chronology chronology = new Chronology(_gState);

        WriteLine("Starting Subtraction Game!\n");
        int score = 0;

        for (int i = 0; i < _operations.Count; i++)
        {
            Operation currentOp = _operations[i]; // Prendo l'operazione corrente

            while (true)
            {
                try
                {
                    Write($"Operation {i + 1}: {currentOp} Enter your answer: ");
                    string? answer = ReadLine();
                    if (string.IsNullOrEmpty(answer))
                    {
                        throw new ArgumentException("Answer cannot be null.");
                    }
                    if (!int.TryParse(answer, out int resAnswer)) //il ! serve per negare il risultato. 
                    {
                        throw new ArgumentException("Invalid input. Please enter a valid integer.");
                    }

                    if (resAnswer == currentOp.Result)
                    {
                        WriteLine("Correct! +3 points");
                        score += 3;
                    }
                    else
                    {
                        WriteLine($"Incorrect! The correct answer is {currentOp.Result}. -2 point");
                        score -= 2;
                    }
                }
                catch (ArgumentException ex)
                {
                    WriteLine($"\nERROR!: {ex.Message}\n");
                    continue; // Torno all'inizio del ciclo while per chiedere nuovamente la risposta
                }
                break; // Esco dal ciclo while per passare alla prossima operazione
            }
        }
        points += score; // Aggiungo il punteggio ottenuto in questa sessione al punteggio totale

        bool stayInMenu = true;
        do //aggiungo un ciclo do-while per rimanere nel menu finchè non scelgo di uscire o continuare
        {
            WriteLine("\ne - exit game\nC - continue\nS - score\n");
            string? exit_addition = ReadLine();
            switch (exit_addition)
            {
                case "e":
                    WriteLine("\nExiting the game. Goodbye!\n");
                    return; // Esco dal metodo per tornare al menu principale del gioco senza perdere lo stato del gioco
                case "C" or "c":
                    WriteLine("\nContinuing the game!\n");
                    _operations.Clear(); // Pulisce la lista delle operazioni
                    GenerateOperationRandom(3); // Genera altre 3 operazioni
                    StartSubtractionGame(); // Richiama il metodo per continuare il gioco
                    stayInMenu = false; // Esco dal ciclo do-while
                    break;
                case "S" or "s":
                    WriteLine($"\nYour score is: {points}\n");
                    WriteLine("b - back\n");
                    string? back_addition_score = ReadLine()?.ToLower();

                    do
                    {
                        if (back_addition_score == "b")
                        {
                            // Torna al menu principale del gioco
                            // stayInMenu rimane true per rimanere nel ciclo do-while
                        }
                        else
                        {
                            WriteLine($"{back_addition_score} is invalid, you must enter b.\n");
                            back_addition_score = ReadLine()?.ToLower(); // Chiede di nuovo l'input perché altrimenti rimane in un ciclo infinito
                        }
                    }
                    while (back_addition_score != "b"); // Continua a chiedere finché non viene inserito "b"
                    break;
            }
        }
        while (stayInMenu);
    }
}