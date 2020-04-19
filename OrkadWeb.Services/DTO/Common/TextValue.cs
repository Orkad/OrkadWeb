using System;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Services.DTO.Common
{
    public class TextValue
    {
        /// <summary>
        /// Valeur d'affichage
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Valeur identifiante (cachée)
        /// </summary>
        public string Value { get; set; }
    }
}
