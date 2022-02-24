using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Fondir.Bacheca.WebUI.Helper
{
    public class Configuration
    {
        public string BaseUrl { get; set; }

        public string TitoloApplicazione { get; set; }

        public string TitoloAvviso { get; set; }

        public int AnnoAvviso { get; set; }

        public int ChiaveOneToOne { get; set; }

        public int ChiaveSeminari { get; set; }

        public int ChiaveMaster { get; set; }

        public int ChiaveSettoreCreditoFinanziario { get; set; }

        public int ChiaveSettoreCommercioTurismoServizi { get; set; }

        public string ChiaveSettoreCF { get; set; }

        public string ChiaveSettoreCTS { get; set; }

        public string ChiaveSettoreEntrambi { get; set; }

        public int AiutiAllaFormazioneID { get; set; }

        public int DimensioneImpresaPiccolaID { get; set; }

        public int RegimeIVAID { get; set; }

        public int StatoID_Piano_Inviato { get; set; }

        public int StatoID_Piano_non_Inviato { get; set; }

        public int StatoID_Piano_Eliminato { get; set; }

        #region path / folder

        public string BaseDirectoryUploadsRichieste { get; set; }

        public string BaseDirectoryUploadsPubblic { get; set; }


        private string _BaseDirectory;

        public string BaseDirectory
        {
            get
            {
                return _BaseDirectory.EndsWith("/") ? _BaseDirectory : _BaseDirectory + "/" ;
            }
            set
            {
                _BaseDirectory = value;
            }
        }

        public string FolderUploadRichiesta_Documenti { get; set; }

        public string FolderUploadRichiesta_Formulario { get; set; }

        public string FolderUploadRichiesta_DocumentiIniziativeSpese { get; set; }

        public string FolderUploadRichiesta_CaricamentoCorsi { get; set; }

        public string FolderUploadRichiesta_Spesa { get; set; }

        public string LogPath { get; set; }

        #endregion

        public string RegistroIndividualeOnetoOneExcel { get; set; }

        public List<Template> SettoreTemplates { get; set; }


        #region mail / smtp


        public bool InvioMailSSL { get; set; }


        //ssl
        public string SSLSmtpServer { get; set; }

        public int SSLSmtpServerPort { get; set; }

        public string SSLSmtpServerUsername { get; set; }

        public string SSLSmtpServerPassword { get; set; }

        public string SSLSmtpServerSenderEmail { get; set; }


        public string SmtpServer { get; set; }

        public int SmtpServerPort { get; set; }

        public string SmtpServerUsername { get; set; }

        public string SmtpServerPassword { get; set; }

        public string SmtpServerSenderEmail { get; set; }


        public bool InvioMailAbilitato { get; set; }

        public string InvioMailIndirizzoTest { get; set; }

        //public string EmailDisplayNameFromNoReplay { get; set; }

        //public string EmailAddressFromNoReplay { get; set; }

        public string EmailAddressBCCLetteraApprovazione { get; set; }

        public string TestoAutoMail { get; set; }


        #endregion

        public string TestoFooter { get; set; }

        public string IndirizzoFondir { get; set; }

        public int MesiDataFineAttivita { get; set; }

        public string TitoliNome { get; set; }

        public int PercentualePartecipante { get; set; }

        public int VoceSpesaID_CostoIniziativa { get; set; }

        public string WkhtmltopdfPath { get; set; }

      //  public string ActiveIP { get; set; }

        public class Template
        {
            public string Settore { get; set; }

            public int SettoreID { get; set; }

            public string TipoTemplate { get; set; }
        }

        public static Configuration ReadConfiguration()
        {
            Configuration config = new Configuration();
            return config.ReadJson<Configuration>();
        }

        string PathConfiguration
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ConfigurationFile"])
                    ? ConfigurationManager.AppSettings["ConfigurationFile"]
                    : Environment.CurrentDirectory;
            }
            set { }
        }

        public string FolderUploadRichiesta_FormularioFinanziamento { get; set; }

        public T ReadJson<T>()
        {
            try
            {
                var _text = File.ReadAllText(PathConfiguration);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(_text);
            }
            catch
            {
                return default(T);
            }
        }

        public void CreateJson<T>(T obj)
        {
            try
            {
                var _x = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                File.WriteAllText(PathConfiguration, _x);
            }
            catch
            {
                throw;
            }
        }
    }
}
