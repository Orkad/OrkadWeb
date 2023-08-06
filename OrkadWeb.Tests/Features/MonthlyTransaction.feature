Fonctionnalité: Transaction mensuelle

Contexte:
	Etant donné que l'utilisateur Orkad existe
	Etant donné que je suis connecté en tant que Orkad

Scénario: charge
	Etant donné qu'il existe une charge mensuelle loyer d'un montant de 899€
	Lorsque j'affiche le budget mensuel
	Alors il y a les charges mensuelles suivantes
		| Libellé | Montant |
		| loyer   | 899€    |

Scénario: revenu
	Etant donné qu'il existe un revenu mensuel salaire d'un montant de 2000€
	Lorsque j'affiche le budget mensuel
	Alors il y a les revenus mensuels suivants
		| Libellé | Montant |
		| salaire | 2000€   |

Scénario: ajout d'une charge
	Lorsque j'ajoute une charge "loyer" d'un montant de 899€
	Lorsque j'affiche le budget mensuel
	Alors il y a les charges mensuelles suivantes
		| Libellé | Montant |
		| loyer   | 899€    |

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

  	
