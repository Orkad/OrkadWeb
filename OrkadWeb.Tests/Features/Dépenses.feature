Fonctionnalité: Dépenses
Expose les fonctionnalités relatives aux dépenses

Contexte: 
	Etant donné que l'utilisateur Nicolas existe
	Et que je suis connecté en tant que Nicolas
	Et que nous somme le 16/01/2022

Scénario: Ajout de dépenses
	Quand j'ajoute la dépense de 300€ à la date du 14/01/2022 que j'appelle téléphone
	Et que j'ajoute la dépense de 20€ à la date du 16/01/2022 que j'appelle galette des rois
	Et que j'ajoute la dépense de 10€ à la date du 01/12/2021 que j'appelle cadeau nul
	Alors le total du mois de janvier 2022 s'élève à 50€
	Et le total du mois de décembre 2021 s'élève à 10€
	Et j'ai 
	
