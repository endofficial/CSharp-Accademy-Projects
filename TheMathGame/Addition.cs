using System.Collections.Generic; // Per usare List<T>
public class Addition 
{
    private class Operation // Classe interna per rappresentare un'operazione di addizione
    {
        public int Number1 { get; } //è solo get perchè non voglio che vengano modificate dopo la creazione
        public int Number2 { get; }
        public int Result { get; }

        public Operation(int n1, int n2) // Costruttore
        {
            Number1 = n1;
            Number2 = n2;
            Result = n1 + n2;
        }

        public override string ToString() // Sovrascrivo ToString per una facile visualizzazione
        {
            return $"{Number1} + {Number2} = ?";
        }
    }

    // Lista per tenere traccia delle operazioni
    private readonly List<Operation> _operations = new List<Operation>();

    //Generatore di numeri casuali. Static per una migliore randomizzazione
    private static readonly Random _random = new Random();

    private readonly RegToChrono _gState; //dichiaro solo il campo perchè lo inizializzo nel costruttore

    public Addition(RegToChrono gState) // Costruttore della classe Addition
    {
        _gState = gState; // Inizializzo il campo con l'istanza passata come parametro
        _operations = new List<Operation>(); // Inizializzo la lista delle operazioni. Potevo anche farlo direttamente nella dichiarazione
        GenerateOperationRandom(3); // Genera 3 operazioni iniziali
    }

    int points = 0; // Variabile per tenere traccia del punteggio totale. La inizializzo qui per mantenerla tra le sessioni di gioco e non resettarla ogni volta che inizio il gioco
    private void GenerateOperationRandom(int count) //metodo privato che genera operazioni casuali
    {
        for (int i = 0; i < count; i++)
        {
            int n1 = _random.Next(1, 101); // Numeri tra 1 e 1000
            int n2 = _random.Next(1, 101);
            _operations.Add(new Operation(n1, n2));
        }
    }    

    public void StartAdditionGame()
    {
        string currentDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        _gState.GetPlayerName(currentDate); // Aggiungo la data e l'ora del gioco alla lista
        Chronology chronology = new Chronology(_gState); 

        WriteLine("Starting Addition Game!\n");
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

                    //in questo modo perdo lo stato del gioco perché creo una nuova istanza di Welcome
                    /*Welcome back_game = new Welcome();
                    back_game.StartWelcome();*/

                    return; // Esco dal metodo per tornare al menu principale del gioco senza perdere lo stato del gioco
                case "C" or "c":
                    WriteLine("\nContinuing the game!\n");
                    _operations.Clear(); // Pulisce la lista delle operazioni
                    GenerateOperationRandom(3); // Genera altre 3 operazioni
                    StartAdditionGame(); // Richiama il metodo per continuare il gioco
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
