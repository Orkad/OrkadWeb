Fonctionnalité: Transaction chart

Contexte:
	Etant donné que l'utilisateur Orkad existe
	* je suis connecté en tant que Orkad
	* nous somme le 15 août 2023

Scénario: Tableau vide
	Lorsque je récupère les données du tableau pour août 2023
	Alors le tableau n'a aucune donnée

Scénario: Tableau rempli
	Etant donné les transactions suivantes
		| montant | date       | nom                |
		| -123    | 20/08/2023 | une dépense        |
		| 256     | 10/08/2023 | un gain            |
		| 789     | 30/07/2023 | un gain en juillet |
	Lorsque je récupère les données du tableau pour août 2023
	Alors le tableau à les données suivantes
		| X          | Y   |
		| 01/08/2023 | 0   |
		| 10/08/2023 | 256 |
		| 20/08/2023 | 133 |