Fonctionnalité: Inscription

Scénario: Nominal
	Quand je tente de m'inscrire avec les informations suivantes
		| Username | Password    | Email        |
		| Orkad    | Default@123 | test@test.fr |
	Alors il n'y a pas d'erreurs
	Et l'utilisateur Orkad existe

Scénario: Utilisateur existant
	Etant donné que l'utilisateur Orkad existe
	Quand je tente de m'inscrire avec les informations suivantes
		| Username | Password    | Email        |
		| Orkad    | Default@123 | test@test.fr |
	Alors il y a une erreur
	
Plan du scénario: Erreurs du nom d'utilisateur
	Quand je tente de m'inscrire avec le nom d'utilisateur <Username>
	Alors il y a une erreur

Exemples:
	| Username                     |
	| Orka                         |
	| TooLoooooooooooooooooongName |
	| Orkad!                       |

Plan du scénario: Erreurs du mot de passe
	Quand je tente de m'inscrire avec le mot de passe <Password>
	Alors il y a une erreur

Exemples:
	| Password                            |
	| De@1                                |
	| TooLoooooooooooooooooongDefault@123 |
	| WithoutNumbers!                     |
	| WithoutSpecial123                   |
