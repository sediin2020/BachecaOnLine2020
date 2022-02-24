using Fondir.Bacheca.Dom.Abstract;
using System;
using System.Collections.Generic;
using Fondir.Bacheca.Dom.Entities;

//using entities = Fondir.Bacheca.Dom.Entities;


namespace Fondir.Bacheca.Dom.Repository
{
    public class DettaglioModuliXIniziativa : IDettaglioModuliXIniziativa
    {
        FondirBachecaDbContext context = new FondirBachecaDbContext();

        public IEnumerable<Entities.DettaglioModuliXIniziativa> Select(int idIniziativa)
        {
            try
            {
                //return context.Database.SqlQuery<entities.Iniziativa>("Select * From v_BachecaOnLine_dataListRicerca");
                return context.Database.SqlQuery<Entities.DettaglioModuliXIniziativa>("exec bachecaOnLine_DettaglioModuli {0} ", idIniziativa);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //public IEnumerable<DettaglioModuliXIniziativa> Select(int idIniziativa)
        //{
        //    try
        //    {
        //        //return context.Database.SqlQuery<entities.Iniziativa>("Select * From v_BachecaOnLine_dataListRicerca");
        //        return context.Database.SqlQuery<Entities.DettaglioModuliXIniziativa>("exec bachecaOnLine_DettaglioModuli {0} ", idIniziativa);
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

    }
}
