using Fondir.Bacheca.Dom.Abstract;
using System;
using System.Collections.Generic;
using Fondir.Bacheca.Dom.Entities;

//using entities = Fondir.Bacheca.Dom.Entities;


namespace Fondir.Bacheca.Dom.Repository
{
    public class DettaglioEdizioniXIniziativa : IDettaglioEdizioniXIniziativa
    {
        FondirBachecaDbContext context = new FondirBachecaDbContext();

        public IEnumerable<Entities.DettaglioEdizioniXIniziativa> Select(int idIniziativa)
        {
            try
            {
                //return context.Database.SqlQuery<entities.Iniziativa>("Select * From v_BachecaOnLine_dataListRicerca");
                return context.Database.SqlQuery<Entities.DettaglioEdizioniXIniziativa>("exec bachecaOnLine_DettaglioEdizioni {0} ", idIniziativa);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
