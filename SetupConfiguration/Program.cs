using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fondir.Bacheca.WebUI.Helper;

namespace SetupConfiguration
{
    class Program
    {
        static void Main(string[] args)
        {
            Configuration c = new Configuration();
            c.AiutiAllaFormazioneID = 2;
            c.ChiaveMaster = 3;
            c.ChiaveOneToOne = 1;
            c.ChiaveSeminari = 2;
            c.DimensioneImpresaPiccolaID = 2;
            c.LogPath = @"D:\TFS_Sediin\Fondir\Main\Bacheca.2017\Fondir.Bacheca.FE.WebUI\Logs";
            //c.PathDocumenti = @"D:\TFS_Sediin\Fondir\Main\Bacheca.2017\Fondir.Bacheca.FE.WebUI\Documenti";
            c.RegimeIVAID = 3;
            c.StatoID_Piano_Inviato = 6;
            c.StatoID_Piano_non_Inviato = 7;
            c.ChiaveSettoreCF = "CREDITIZIO FINANZIARIO -ASSICURATIVO";
            c.ChiaveSettoreCTS = "COMMERCIO TURISMO SERVIZI - LOGISTICA SPEDIZIONE TRASPORTI -ALTRI SETTORI ECONOMICI";
            c.ChiaveSettoreEntrambi = "ENTRAMBI I COMPARTI";

            var _settoreTemplates = new List<Configuration.Template>();

            Configuration.Template t = new Configuration.Template();
            t.TipoTemplate = "Creditizio";
            t.SettoreID = 1;
            t.Settore = "Creditizio-finanziario";
            _settoreTemplates.Add(t);

            t = new Configuration.Template();
            t.TipoTemplate = "Commercio";
            t.SettoreID = 2;
            t.Settore = "Assicurativo";
            _settoreTemplates.Add(t);

            t = new Configuration.Template();
            t.TipoTemplate = "Commercio";
            t.SettoreID = 3;
            t.Settore = "Commercio-turismo-servizi";
            _settoreTemplates.Add(t);

            t = new Configuration.Template();
            t.TipoTemplate = "Commercio";
            t.SettoreID = 4;
            t.Settore = "Logistica-spedizioni-trasporto";
            _settoreTemplates.Add(t);

            t = new Configuration.Template();
            t.TipoTemplate = "Commercio";
            t.SettoreID = 5;
            t.Settore = "Altri settori economici";
            _settoreTemplates.Add(t);

            c.SettoreTemplates = _settoreTemplates;

            c.CreateJson<Configuration>( c);

        }
    }
}
