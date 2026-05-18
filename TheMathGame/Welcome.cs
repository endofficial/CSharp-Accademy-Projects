public class Welcome
{
    public void StartWelcome()
    {
        // Creo un'istanza di RegToChrono per passarlo a Play e Chronology così da condividere lo stato 
        RegToChrono reg_To_Chrono = new RegToChrono();
        bool isRun = true;
        while (isRun)
        {
            // Ciclo per permettere all'utente di scegliere più volte tra le opzioni finché non decide di uscire anche quando più avanti scegli4 'e' per uscire dal gioco
            {
                WriteLine("Welcome to Mathgame!");
                WriteLine("P - PLAY \nC - CHRONOLOGY \nQ - QUIT\n");
                string? input_1 = ReadLine();

                if (string.IsNullOrWhiteSpace(input_1))
                {
                    WriteLine("Input cannot be null or empty.");
                    continue;
                }
                   
                switch (input_1)
                {
                    case "P" or "p":
                        Play gameStart = new Play(reg_To_Chrono);
                        gameStart.PlayGame();
                        break;
                    case "C" or "c":
                        Chronology chronology = new Chronology(reg_To_Chrono); // Passo la stessa istanza di RegToChrono a Chronology
                        chronology.StartChronology();
                        break;
                    case "Q" or "q":
                        WriteLine("\nYou chose to quit the game, goodbye!");
                        isRun = false; // Imposto isRun a false per uscire dal ciclo e terminare il programma
                        break;
                    default:
                        WriteLine("Invalid input, please try again.\n");
                        break;
                }
            }
        }
    }
}
