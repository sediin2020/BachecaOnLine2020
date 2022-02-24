using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fondir.Bacheca.Dom.Entities
{
    public class DettaglioEdizioniXIniziativa
    {
        public int idEdizione { get; set; }

        public int numero_edizione { get; set; }

        public string data_inizio_edizione { get; set; }

        public string data_fine_edizione { get; set; }

        public string indirizzo { get; set; }

        public string civico { get; set; }

        public string cap { get; set; }

        public string localita { get; set; }

        public string codice_provincia { get; set; }

        public string regione { get; set; }

        public string NomePiattaformaNew { get; set; }

        public string LinkNew { get; set; }
    }
}
