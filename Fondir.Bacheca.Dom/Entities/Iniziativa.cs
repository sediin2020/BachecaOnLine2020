using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fondir.Bacheca.Dom.Entities
{
    public class Iniziativa
    {
        public int idIniziativa { get; set; }
        public string codiceIniziativa { get; set; }
        public string titolo { get; set; }
        public int? idComparto { get; set; }
        public int? tipo_iniziativa { get; set; }
        public string tipologiainiziativa { get; set; }
        public int? IdAreaTematica { get; set; } = 0;
        public string AreaTematica { get; set; }
        public int? IdTematica { get; set; } = 0;
        public string Tematica { get; set; }
        public int? idUtente { get; set; }
        public string struttura { get; set; }
        public decimal? costo_iniziativa { get; set; }
        public decimal? iva { get; set; }
        public decimal? totale_costo { get; set; }
        public int? NumeroEdizioni { get; set; }
        public string PeriodoSvolgimento { get; set; }
        public int? stato { get; set; }
        public int? regione_sede { get; set; }
        public int? sede_didattica { get; set; }
        public string ParoleChiaveNew { get; set; }
    }



    public class BrochureIniziativa
    {
        public int id { get; set; }
        public Nullable<int> idIniziativa { get; set; }
        public string codiceIniziativa { get; set; }
        public string titoloIniziativa { get; set; }
        public Nullable<int> idDocumento { get; set; }
        public string tipoDocumento { get; set; }
        public string nomeFile { get; set; }
        public string breve_descrizione { get; set; }
        public string data { get; set; }
    }
}
