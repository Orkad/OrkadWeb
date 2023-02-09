Fonctionnalité: Inscription Email
	On s'assure ici que l'email est bien envoyé lors d'une inscription

Scénario: Nominal
	Quand je tente de m'inscrire avec les informations suivantes
		| Username | Password    | Email        |
		| Orkad    | Default@123 | test@test.fr |
	Alors il n'y a pas d'erreurs
	Et l'utilisateur Orkad existe
	Et un email a bien été envoyé à l'adresse test@test.fr

