using Fondir.Bacheca.Dom.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fondir.Bacheca.WebUI.Models
{
    public class IniziativaViewModel
    {
        public int IniziativaID { get; set; }

        public string Tipo{ get; set; }

        public string Titolo { get; set; }

        public string Luogo { get; set; }

        public string Periodo { get; set; }

        public int? TipoInizitivaID { get; internal set; }

        public int? StrutturaID { get; internal set; }

        //public int? TematicaID { get; internal set; }

        public int? IdAreaTematica { get; internal set; }

        public int? IdTematica { get; internal set; }

        public int? RegioneID { get; internal set; }
        public string CodiceIniziativa { get; internal set; }
        public string Struttura { get; internal set; }
        public int? SettoreID { get; internal set; }
        public DateTime? DataInizio { get; internal set; }
        public DateTime? DataFine { get; internal set; }

        public long? CodiceIniziativaNumerico { get; set; }

        public int? sede_didattica { get; internal set; }

        //public int? AreaTematicaID { get; internal set; }

        //  public DateTime Data { get; set; }

        public string ParoleChiaveNew { get; internal set; }
    }

    public class IniziativaRicercaModel
    {
        public string[] Strutture { get; set; }

        public string[] Settore { get; set; }

        public string[] AreaTematica { get; set; }

        public string[] Tematica { get; set; }

        public string Regione { get; set; }

        public string ChiaveRicerca { get; set; }

        public string TipoIniziativa { get; set; }

        public string[] SedeDidattica { get; set; }

        public string OrderBy { get; set; } //= "CodiceIniziativaNumerico desc";
    }


    public class DettaglioIniziativaView
    {
        public DettaglioIniziativa Iniziativ { get; set; }

        public IEnumerable<BrochureIniziativa> Brochure { get; set; }

    }

}