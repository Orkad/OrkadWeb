Fonctionnalité: Charges

Contexte:
	Etant donné que l'utilisateur Orkad existe
	Etant donné que je suis connecté en tant que Orkad

Scénario: charge
	Etant donné qu'il existe une charge mensuelle loyer d'un montant de 899€
	Lorsque j'affiche le budget mensuel
	Alors il y a les charges mensuelles suivantes
		| Libellé | Montant |
		| loyer   | 899€    |

Scénario: ajout d'une charge
	Lorsque j'ajoute une charge "loyer" d'un montant de 899€
	Lorsque j'affiche le budget mensuel
	Alors il y a les charges mensuelles suivantes
		| Libellé | Montant |
		| loyer   | 899€    |

Scénario: ajout d'une charge libellé vide
	Lorsque j'ajoute une charge "" d'un montant de 899€
	Alors il y a une erreur avec le message suivant : Le nom de la charge ne doit pas être vide

Scénario: modification d'une charge
	Etant donné qu'il existe une charge mensuelle loyer d'un montant de 899€
	Lorsque je modifie la charge "loyer" par
		| Libellé       | Montant |
		| loyer modifié | 905     |
	Lorsque j'affiche le budget mensuel
	Alors il y a les charges mensuelles suivantes
		| Libellé       | Montant |
		| loyer modifié | 905€    |
   
Scénario: suppression d'une charge
	Etant donné qu'il existe une charge mensuelle loyer d'un montant de 899€
	Lorsque je supprime la charge "loyer"
	Lorsque j'affiche le budget mensuel
	Alors il n'y a aucune charge mensuelle

 Règle: le montant d'une charge doit être supérieur à zéro
 	Exemple: ajout d'une charge à zéro
 		Lorsque j'ajoute une charge "loyer" d'un montant de 0€ 
 		Alors il y a une erreur avec le message suivant : Le montant de la charge doit être supérieur à 0
 		
 	Exemple: ajout d'une charge négative
 		Lorsque j'ajoute une charge "loyer" d'un montant de -1€
 		Alors il y a une erreur avec le message suivant : Le montant de la charge doit être supérieur à 0
 	
 	Exemple: modification d'une charge à zéro
 		Etant donné qu'il existe une charge mensuelle loyer d'un montant de 123€
	    Lorsque je modifie la charge "loyer" par
	    | Libellé       | Montant |
	    | loyer modifié | 0       |
        Alors il y a une erreur avec le message suivant : Le montant de la charge doit être supérieur à 0
       
	Exemple: modification d'une charge négative
		Etant donné qu'il existe une charge mensuelle loyer d'un montant de 123€
		Lorsque je modifie la charge "loyer" par
		| Libellé       | Montant |
		| loyer modifié | -1      |
  		Alors il y a une erreur avec le message suivant : Le montant de la charge doit être supérieur à 0