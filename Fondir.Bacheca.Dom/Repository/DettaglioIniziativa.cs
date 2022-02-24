using Fondir.Bacheca.Dom.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fondir.Bacheca.Dom.Entities;

namespace Fondir.Bacheca.Dom.Repository
{
    public class DettaglioIniziativa : IDettaglioIniziativa
    {
        FondirBachecaDbContext context = new FondirBachecaDbContext();

        public Entities.DettaglioIniziativa Detail(int idIniziativa)
        {
            try
            {
                return context.Database.SqlQuery<Entities.DettaglioIniziativa>("exec bachecaOnLine_DettaglioIniziativa {0} ", idIniziativa).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;

            }
        }
    }
}
