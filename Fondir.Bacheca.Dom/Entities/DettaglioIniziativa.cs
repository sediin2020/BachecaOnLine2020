using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fondir.Bacheca.Dom.Entities
{
    public class DettaglioIniziativa
    {
        public string codice { get; set; }

        public string titolo { get; set; }

        public string tipologia { get; set; }

        public string modalita_percorso { get; set; }

        public string comparto { get; set; }

        public string areatematica { get; set; }

        public string tematica { get; set; }

        public string obiettivi { get; set; }

        public string metodologie_formative { get; set; }

        public string rilevazione_fabbisogni { get; set; }

        public string esperienza_soggetto_erogatore { get; set; }

        public string esperienza_personale_docente { get; set; }

        public string programma_evento { get; set; }

        public string risultati_attesi { get; set; }

        public string persona_referente_iniziativa { get; set; }

        public string recapito_telefonico_referente_iniziativa { get; set; }

        public string recapito_mail_referente_iniziativa { get; set; }

        public decimal costo_partecipante { get; set; }

        public decimal iva { get; set; }

        public decimal totale_costo { get; set; }


        public int iniziativaID { get; set; }

        public string soggetto_erogatore { get; set; }

        public string criterio_applicazione_sconti { get; set; }


        public int utenteId { get; set; }

        public int tipo_iniziativa { get; set; }

        public int totale_ore { get; set; }
    }
}
