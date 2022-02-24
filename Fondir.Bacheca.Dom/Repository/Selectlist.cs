using Fondir.Bacheca.Dom.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fondir.Bacheca.Dom.Entities;

using entities = Fondir.Bacheca.Dom.Entities;

namespace Fondir.Bacheca.Dom.Repository
{
    public class Selectlist : ISelectlist
    {
        FondirBachecaDbContext context = new FondirBachecaDbContext();

        public IEnumerable<Entities.Selectlist> Select(string procedureName)
        {
            try
            {
                return context.Database.SqlQuery<entities.Selectlist>("exec " + procedureName);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
