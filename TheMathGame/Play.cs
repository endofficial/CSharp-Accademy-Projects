using System.Collections.Generic;

public class Play 
{
    // Composizione: Play "ha un" RegToChrono per registrare le partite giocate
    private readonly RegToChrono _gState;
    public Play(RegToChrono gState)
    {
        _gState = gState;
    }

    public void PlayGame()
    {
        bool isPlaying = true;
        while (isPlaying)
        {
            Write("\nEnter your name: ");
            string? playerName = ReadLine();
            if (string.IsNullOrWhiteSpace(playerName))
            {
                WriteLine("\nERROR!: Name cannot be null or empty.\n");
                continue;
            }

            else
            {
                WriteLine($"Welcome to the Mathgame, {playerName}!\n");
                _gState.GetPlayerName(playerName); // Aggiungo il nome del giocatore alla lista
            }

            while (isPlaying)
            {
                Write("\nChoose an operator for the game between: \nA -> '+' \nS -> '-' \nM -> 'x' \nD -> ':' \ne -> exit game\n");
                string? Input_game = ReadLine();
                if (string.IsNullOrWhiteSpace(Input_game))
                {
                    WriteLine("Input cannot be null or empty.");
                    continue;
                }

                switch (Input_game)
                {
                    case "A" or "a":
                        WriteLine("\nYou chose Addition!");
                        Addition addition = new Addition(_gState); // Passo l'istanza di RegToChrono al costruttore di Addition
                        addition.StartAdditionGame();
                        return; // Esco dal metodo per tornare al menu principale del gioco senza perdere lo stato del gioco
                    case "S" or "s":
                        WriteLine("\nYou chose Subtraction!");
                        Subtraction subtraction = new Subtraction(_gState);
                        subtraction.StartSubtractionGame();
                        return;
                    case "M" or "m":
                        WriteLine("\nYou chose Multiplication!");
                        Multiplication multiplication = new Multiplication(_gState);
                        multiplication.StartMultiplicationGame();
                        return;
                    case "D" or "d":
                        WriteLine("\nYou chose Division!");
                        Division division = new Division(_gState);
                        division.StartDivisionGame();
                        return;
                    case "e":
                        WriteLine("\nExiting the game. Goodbye!\n");
                        return;
                    default:
                        WriteLine("Invalid choice. Please select a valid operator.");
                        break;
                }
            }
        }     
    }         
}
