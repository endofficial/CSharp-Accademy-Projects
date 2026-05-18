//Avevo ereditato la classe RegToChrono per poter accedere alla lista delle partite registrate ma è sbagliato perchè così facendo ogni volta che creo una nuova istanza di Addition la lista delle partite registrate si resetta.
//La logica della cronologia è che non è una lista di partite ma un'azione che usa la lista delle partite registrate. 
//Quindi basta la composizione; implica che la classe Chronology "ha un" RegToChrono per poter accedere alla lista delle partite registrate.
public class Chronology 
{
    // Composizione: Chronology "ha un" RegToChrono per accedere alle partite registrate
    private readonly RegToChrono _gState;

    // Costruttore che accetta un'istanza di RegToChrono
    public Chronology(RegToChrono gState)
    {
        _gState = gState;
    }
    public void StartChronology()
    {
        WriteLine("\nOpen to chronology\n");
        _gState.GetChrono(); // Chiamo il metodo GetChrono della classe base per stampare la cronologia
    }
    
}
