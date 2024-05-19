## Task-urie proiect IP - aplicatie tip chat

Documentatie -> **Marco** 
UML ( diagrame clase,activitati,secvente, cazuri de utilizare) -> **Rares**
Interfata + callbacks -> **Marco**
Baza de date - **Molo**
Server -> **Zaco**(Rares)
Unit Test -> **Rares**
Arhitectura -> **Rares**


## Diagrama de secvente:

- Fereastra initiala:
	- Client:
		- inregistrare : nume, prenume, nr de telefon, mail, username, parola
		- autentificare : username si parola -> se schimba fereasta si userul va fi retinut pana la logout
	- Fereastra user-autentificat:
		- avem sus datele utilizatorului si buton de logout
		- jos avem un box unde ne vor aparea utilizatorii care s-au conectat la server
		- jos de tot avem un button de chat, putem selecta persoana din box cu care dorim sa initiem o conversatie
		apasam pe el, si apasam dupa butonul CHAT, 
		Fereastra CHAT dintre 2 persoane, avem sus cu cine vorbim iar mai jos avem un chatbox unde apar mesajele
		Log-ul cu chat-ul ramane la client intr-un fisier text. Cand se apasa pe CHAT se citeste din fisier istoricul conversatiei
	- Server:
		- Format mesaj: Sender|Receiver|Mesaj
		- Trebuie sa primeasca datele de autentificare, verifica datele in baza de date, 
			si daca sunt corecte se va adauga utilizatorul in lista de useri activi. 
		- Serveru va trimite fiecarui client lista de useri activi atunci cand un client nou se conecteaza/deconecteaza
