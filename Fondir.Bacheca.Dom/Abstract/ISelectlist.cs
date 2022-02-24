using Fondir.Bacheca.Dom.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fondir.Bacheca.Dom.Abstract
{
  public  interface ISelectlist
    {
        IEnumerable<Selectlist> Select(string procedureName);
    }
}
