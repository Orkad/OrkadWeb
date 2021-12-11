using System;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Logic.Common
{
    /// <summary>
    /// Représente une réponse coté métier
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Définie une réponse d'erreur
        /// </summary>
        /// <param name="errorMessage">Message d'erreur correpondant</param>
        public static Response AsError(string errorMessage) => new Response { Success = false, Error = errorMessage };

        public static Response AsSuccess() => new Response { Success = true };

        private Response() { }

        /// <summary>
        /// Détermine que l'opération s'est déroulée avec succès
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message d'erreur lorsque un problème est rencontré
        /// </summary>
        public string Error { get; set; }
    }
}
