Fonctionnalité: Revenu

    Contexte:
        Etant donné que l'utilisateur Orkad existe
        Etant donné que je suis connecté en tant que Orkad

    Scénario: Affichage d'un revenu
        Etant donné qu'il existe un revenu salaire d'un montant de 2000€
        Lorsque j'affiche le budget mensuel
        Alors il y a les revenus suivants
          | Libellé | Montant |
          | salaire | 2000€   |

    Scénario: Une charge n'est pas un revenu
        Etant donné qu'il existe une charge mensuelle loyer d'un montant de 1000€
        Lorsque j'affiche le budget mensuel
        Alors il n'y a aucun revenu

    Scénario: Création d'un revenu
        Lorsque j'ajoute un revenu "salaire" d'un montant de 2000€
        Et que j'affiche le budget mensuel
        Alors il y a les revenus suivants
          | Libellé | Montant |
          | salaire | 2000    |

    Scénario: Mise à jour d'un revenu
        Etant donné qu'il existe un revenu salaire d'un montant de 2000€
        Lorsque je modifie le revenu "salaire" par
          | Libellé         | Montant |
          | salaire modifié | 2100    |
        Et que j'affiche le budget mensuel
        Alors il y a les revenus suivants
          | Libellé         | Montant |
          | salaire modifié | 2100    |

    Scénario: Suppression d'un revenu
        Etant donné qu'il existe un revenu salaire d'un montant de 2000€
        Lorsque je supprime le revenu "salaire"
        Et que j'affiche le budget mensuel
        Alors il n'y a aucun revenu