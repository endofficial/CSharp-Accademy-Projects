public class RegToChrono
{
    //voglio fare in modo che ogni giocatore abbia una lista di partite giocate
    public record Games (DateTime Date);

    public record Players(string PlayerName)
    {
        public List<Games> Games = new();
    }

    protected List<Players> _players = new List<Players>();

    //metodo per ottenere il giocatore in base al nome
    public Players? GetPlayerName(string inputName) // serve per registrare una partita per un giocatore
    {
        if (DateTime.TryParse(inputName, out _)) //il tryparse ritorna true se riesce a convertire la stringa in data, se è così da null
        {
            return null; // Se l'input è una data, ritorno null
        }

        var player = _players.Where(p => p.PlayerName == inputName).FirstOrDefault(); // Restituisco il primo giocatore che corrisponde al nome, o null se non trovato
        if (player is null)
        {
            player = new Players(inputName); // Creo un nuovo giocatore se non esiste
            _players.Add(player); // Aggiungo il nuovo giocatore alla lista
        }
        player.Games.Add(new Games(DateTime.Now)); // Aggiungo una nuova partita con la data e l'ora corrente
        return player;
    }
    
    public void GetChrono() //void perché non ritorna nulla, deve solo stampare
    {
        WriteLine("--- Games chronology ---\n");
        if (_players.Count == 0)
        {
            WriteLine("\nNo games recorded!\n");
            return;
        }

        // Stampo il nome del giocatore e la lista delle partite giocate
        foreach (var playerX in _players)
        {
            Write($"\nPlayer: {playerX.PlayerName}\n"); // Stampo il nome del giocatore

            foreach (var game in playerX.Games)
            {
                WriteLine($" - {game.Date}"); // Stampo la data della partita
            }
        }
        WriteLine("\n--- End of chronology ---\n");
    }
}
