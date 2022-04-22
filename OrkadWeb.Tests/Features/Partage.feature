#language: fr
Fonctionnalité: Partage
Expose la fonctionnalité de base de partage de dépenses

Contexte: 
	Etant donné que il existe les utilisateurs suivants :
	| Nom d'utilisateur |
	| Nicolas           |
	| Léa               |
	Et qu'il existe un partage entre Nicolas et Léa
	Et que je suis connecté en tant que Nicolas
	
Scénario: Affichage des informations principales sur le partage
	Quand j'ajoute la dépense de 30€ à la date du 14/01/2022
	Quand Nicolas ajoute la dépense de 30€ nommée "chocolat" au partage
	Et que Léa ajoute la dépense de 50€ nommée "gateau" au partage
	Alors le partage affiche que les dépenses totales s'élèvent à hauteur de 80€
	Et le partage affiche qu'il y a un écart de 20€
	Et Nicolas doit 10€ à Léa sur le partage
	
