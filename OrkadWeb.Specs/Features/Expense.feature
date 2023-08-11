Fonctionnalité: Dépenses

Contexte:
	Etant donné que l'utilisateur Moi existe
	Etant donné que je suis connecté en tant que Moi
	* nous somme le 31/07/2023
	
Scénario: Affichage uniquement de ses dépenses
	Etant donné que l'utilisateur Toi existe
	Etant donné les dépenses suivantes
	  | montant | date       | nom        | propriétaire |
	  | 1       | 27/07/2023 | ma dépense | Moi          |
	  | 2       | 27/07/2023 | ta dépense | Toi          |
	Quand j'affiche la liste de mes dépenses sur le mois de juillet 2023
	Alors mes dépenses sont les suivantes
	  | montant | date       | nom        |
	  | 1       | 27/07/2023 | ma dépense |
   
Scénario: Affichage des dépenses du mois
	Etant donné les dépenses suivantes
	  | montant | date       | nom        | propriétaire |
	  | 1       | 27/07/2023 | ma dépense | Moi          |
	Quand j'affiche la liste de mes dépenses sur le mois de juin 2023
	Alors il n'y aucune dépense affichée
   
Scénario: Création d'une dépense
	Quand j'ajoute la dépense de 1€ à la date du 27/07/2023 que j'appelle ma dépense
	Quand j'affiche la liste de mes dépenses sur le mois de juillet 2023 
	Alors mes dépenses sont les suivantes
	  | montant | date       | nom        |
	  | 1       | 27/07/2023 | ma dépense |
   
Scénario: Création d'une dépense future impossible
	Quand j'ajoute la dépense de 1€ à la date du 01/08/2023 que j'appelle ma dépense future
	Alors il y a une erreur
   
Scénario: Modification d'une dépense
	Etant donné les dépenses suivantes
	| montant | date       | nom        | propriétaire |
	| 1       | 27/07/2023 | ma dépense | Moi          |
    Quand je modifie la dépense "ma dépense" par
	| montant | date       | nom                 |
	| 2       | 28/07/2023 | ma dépense modifiée |
 	Quand j'affiche la liste de mes dépenses sur le mois de juillet 2023
	Alors mes dépenses sont les suivantes
	  | montant | date       | nom                 |
	  | 2       | 28/07/2023 | ma dépense modifiée |
   
Scénario: Suppression d'une dépense
	Etant donné les dépenses suivantes
	  | montant | date       | nom        | propriétaire |
	  | 1       | 27/07/2023 | ma dépense | Moi          |
	Quand je supprime la dépense "ma dépense"
	Et que j'affiche la liste de mes dépenses sur le mois de juillet 2023
	Alors il n'y aucune dépense affichée
  
