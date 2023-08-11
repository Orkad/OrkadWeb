Fonctionnalité: login

Contexte:
	Etant donné que l'utilisateur Test existe avec le mot de passe okpassword

Scénario: nominal
	Quand je me connecte avec la combinaison Test / okpassword
	Alors la connexion a réussie

Scénario: case sensitive
	Quand je me connecte avec la combinaison Test / OKPASSWORD
	Alors la connexion a échouée

Scénario: mauvais utilisateur
	Quand je me connecte avec la combinaison Pouet / okpassword
	Alors la connexion a échouée
