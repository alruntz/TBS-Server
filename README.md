## Stack

 - **Serveur :**		.NET (core) 
 - **Web API :**	ASP.NET 
 - **Database :**	MONGO DB

## Architecture du projet :

 - **APICore :**  
 DLL qui va pouvoir être transportée un peu partout. Dans notre cas elle sera utilisée côté serveur et client (unity).
 Elle ppermet d'interagir avec la WebAPI et ainsi modifier la base de donnée  (données des joueurs, items, maps, etc ...).
 Deux directives de préprocesseur `#IF` sont présentes (`USE_ADMIN, USE_PRIVATE`).
   	- `# IF USE_ADMIN` (utilisation côté serveur): 		Après compilation, la DLL fournira un total accès à l'API.
   	- `# IF USE_PRIVATE` (utilisation côté client): 		Après compilation, la DLL fournira un accès limité à l'API (routes publiques et privées)

 - **Models :**
DLL qui va pouvoir être transportée un peu partout. Dans notre cas elle sera utilisée côté serveur, API, et client.
Regroupe tous les models (item, player, spell, etc ...)
La directive de préprocessur #IF USE_MONGO est utilisée seulement si notre solution à besoin de faire des appels à la base de donnée pour initialiser les modeles.

 - **ServerConsole :**
Le serveur TCP qui va manager les clients et le moteur de jeu.

 - **GridSystem :**
Dll qui va être utilisée côté serveur et client.
C'est tout simplement le moteur de grille 2D.

 - **TBSEngine :**
Dll qui va être utilisée côté serveur et client (pour la prédiction)
Moteur de jeu.
