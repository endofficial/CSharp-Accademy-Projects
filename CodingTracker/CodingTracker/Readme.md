# CODING TRACKER

Coding tracker è la mia applicazione in C#, utile per registrare sessioni di lavoro, che utilizza SQlite.

Questa applicazione rispetta l'acronimo CRUD (Create, Read, Update e Delete).

## Key Features

- **Registrazione delle sessioni**: Il programma registra nuove sessioni di lavoro, registrando la data, un tempo di inizio, un tempo di fine e memorizzando una durata.
- **CRUD**: L'applicazione consente di aggiungere, leggere, modificare ed eliminare le singole sessioni.
- **Dettagli del codice**: Il codice è stato scritto utilizzando Spectre.Console.
- **Archiviazione dati**: Il programma comunica con un database, utilizzando SQlite con Dapper.
- **Inizializzazione automatica database**: Se il database non esiste, viene auto-generato dal programma all'avvio.
- **Auto-generazione dati**: Se il database è vuoto, vengono auto-generati 100 record.
- **Gestione degli errori**: Il codice prevede una gestione robusta degli errori causati dall'utente e delle eccezioni possibili.
- **Test unitari**: Il codice prevede dei test unitari per testare il corretto funzionamento di alcuni metodi.
- **Console UI**: All'apertura del programma, dopo il caricamento del database, viene mostrato un menù a tendina. L'utente, in base alla sua scelta, utilizzando le frecce direzionali su e giù, naviga nel menù.
	- <img src="Resources\doc\images\Menu.png" alt="menu" width="500"> 

## Functionality & Usage

- **Live session**
	- ***Registrazione data***: Registrare la data in cui la sessione si svolge.
		- <img src="Resources\doc\images\Live-session-date.png" alt="date" width="500">
	
	- ***Scelta dell'utente***: Digitare 'P' o 'p' per andare avanti con il programma. Digitare '0' per tornare al menù.
		- <img src="Resources\doc\images\play-session.png" alt="play session" width="500">
	
	- ***Descrizione***: Aggiungere una descrizione alla sessione.
		- <img src="Resources\doc\images\description.png" alt="description session" width="500">
	
	- ***Start session***: Il cronometro inizia e si fermerà solo quando l'utente premerà qualsiasi tasto.
		- <img src="Resources\doc\images\live-session.png" alt="start session" width="500">

- **Register a new session**
	- ***Registrazione data***: Registrare la data in cui la sessione si è svolta.
	- ***Descrizione***: Aggiungere una descrizione alla sessione.
	- ***Start time***: Indicare l'orario di inizio.
		- <img src="Resources\doc\images\startTime.png" alt="Start Time" width="500">
	
	- ***End time***: Indicare l'orario di fine.
		- <img src="Resources\doc\images\endTime.png" alt="End Time" width="500">
	
- **Visualizzazione sessioni**
	- ***Visualizzazione delle sessioni***: Viene mostrata una tabella con tutte le sessioni nel database.
		- <img src="Resources\doc\images\all-records.png" alt="All records" width="500">
	
	- ***Filtri***: Viene mostrato un elenco di filtri.
		- <img src="Resources\doc\images\choice-list.png" alt="view filters" width="500">
	
		- ***Filtro anno***: L'utente può scegliere di quale anno vuole visualizzare le sessioni.
			- <img src="Resources\doc\images\orderToYear.png" alt="filter to year" width="500">
		
		- ***Filtro mesi***: L'utente può scegliere di quale mese vuole visualizzare le sessioni.
			- <img src="Resources\doc\images\orderToMonth.png" alt="filter to month" width="500">
		
		- ***Filtro giorni***: L'utente può scegliere di quale giorno vuole visualizzare le sessioni.
			- <img src="Resources\doc\images\orderToDay.png" alt="filter to day" width="500">
		
		- ***Filtro per ordine crescente***: L'utente può scegliere di visualizzare le sessioni in ordine crescente.
		- ***Filtro per ordine decrescente***: L'utente può scegliere di visualizzare le sessioni in ordine decrescente.
		
- **Update session**
	- ***Visualizzazione delle sessioni***: Viene mostrata una tabella con tutte le sessioni nel database.
	- ***Cosa si vuole aggiornare?***: Viene mostrata una lista in cui l'utente può navigare per scegliere cosa vuole aggiornare.
		- <img src="Resources\doc\images\all-sessions.png" alt="view all sessions" width="500">

- **Delete session**
	- ***Visualizzazione delle sessioni***: Viene mostrata una tabella con tutte le sessioni nel database.
		- <img src="Resources\doc\images\allsessiontodelete.png" alt="view all sessions to delete" width="500">

	- ***Eliminare tutto il database***: Scegliendo l'opzione '1', verrà cancellato l'intero database.
	- ***Scegliere database da eliminare***: Scegliendo '2', dovrà essere indicato un id corrispondente alla sessione che si vuole eliminare.
		- <img src="Resources\doc\images\allsessiontodelete.png" alt="option to delete one session" width="500">
