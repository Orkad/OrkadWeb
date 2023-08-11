Fonctionnalité: Admin

Contexte:
	Etant donné que l'utilisateur admin existe
	Et que l'utilisateur est administrateur
	Et que l'utilisateur utilisateur existe
	

Scénario: listing
	Etant donné que je suis connecté en tant que admin
	Lorsque j'affiche la liste des utilisateurs
	Alors il y a les utilisateurs suivants
		| name        | role  |
		| admin       | Admin |
		| utilisateur | User  |

Scénario: droits
	Etant donné que je suis connecté en tant que utilisateur
	Lorsque j'affiche la liste des utilisateurs
	Alors il y a une erreur
