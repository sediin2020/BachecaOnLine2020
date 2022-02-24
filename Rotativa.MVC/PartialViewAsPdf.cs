// Decompiled with JetBrains decompiler
// Type: Rotativa.MVC.PartialViewAsPdf
// Assembly: Rotativa.MVC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AFE8B199-049E-4B91-92C6-F1D7392E2549
// Assembly location: C:\Users\g.tuzzolino\Desktop\Nuova cartella (2)\Rotativa.MVC.dll

using System.Web.Mvc;

namespace Rotativa.MVC
{
    public class PartialViewAsPdf : ViewAsPdf
    {
        public PartialViewAsPdf()
        {
        }

        public PartialViewAsPdf(string partialViewName)
          : base(partialViewName)
        {
        }

        public PartialViewAsPdf(object model)
          : base(model)
        {
        }

        public PartialViewAsPdf(string partialViewName, object model)
          : base(partialViewName, model)
        {
        }

        protected override ViewEngineResult GetView(ControllerContext context, string viewName, string masterName)
        {
            return ViewEngines.Engines.FindPartialView(context, this.ViewName);
        }
    }
}
