using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Fondir.Bacheca.WebUI.Helper
{
    public static class FondirHtmlHelper
    {
        #region web ui bacheca

        public static bool IsTipologiaOneToOne(this HtmlHelper helper, int tipo)
        {
            return FondirConfiguration(helper).ChiaveOneToOne == tipo;
        }

        public static bool IsTipologiaSeminari(this HtmlHelper helper, int tipo)
        {
            return FondirConfiguration(helper).ChiaveSeminari == tipo;
        }

        public static bool IsTipologiaMaster(this HtmlHelper helper, int tipo)
        {
            return FondirConfiguration(helper).ChiaveMaster == tipo;
        }

        //public static bool IsSettoreCreditizioFinanziario(this HtmlHelper helper, string settore)
        //{
        //    try
        //    {
        //        return FondirConfiguration(helper).ChiaveSettoreCF == settore;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //public static bool IsSettoreCommercio(this HtmlHelper helper, string settore)
        //{
        //    try
        //    {
        //        return FondirConfiguration(helper).ChiaveSettoreCTS == settore;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //public static bool IsSettoreCreditizioFinanziario(this HtmlHelper helper, int settore)
        //{
        //    try
        //    {
        //        return FondirConfiguration(helper).ChiaveSettoreCreditoFinanziario == settore;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        public static bool IsSettoreCommercio(this HtmlHelper helper, int? settore)
        {
            try
            {
                return FondirConfiguration(helper).SettoreTemplates
                    .Where(s => s.TipoTemplate == "Commercio")
                    .FirstOrDefault(s => s.SettoreID == settore.GetValueOrDefault()) != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsSettoreEntrambi(this HtmlHelper helper, string settore)
        {
            try
            {
                return FondirConfiguration(helper).ChiaveSettoreEntrambi == settore;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion


        #region web ui iniziative

        public static Configuration FondirConfiguration(this HtmlHelper helper)
        {
            return Configuration.ReadConfiguration();
        }


        public static int AiutiDiStato_AiutiAllaFormazioneID(this HtmlHelper helper)
        {
            try
            {
                return FondirConfiguration(helper).AiutiAllaFormazioneID;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static string BaseUrl(this HtmlHelper helper)
        {
            //var urlHelper = helper.ViewContext.RequestContext.HttpContext.Request.Url;

            //return urlHelper.AbsoluteUri.Replace(urlHelper.AbsolutePath, "");

            return new Uri(Configuration.ReadConfiguration().BaseUrl).ToString();
        }

        //public static string ToUrl(this HtmlHelper helper, string fragment)
        //{
        //    return new UriBuilder(BaseUrl(helper) + fragment).Uri.AbsoluteUri;
        //}

        public static MvcHtmlString GetProgressbar(this HtmlHelper helper, decimal percentuale, string id, string height)
        {
            var _h = "";// !string.IsNullOrWhiteSpace(height) ? "height: " + height + "px; line-height: " + height + "px;" : "";
            _h = percentuale > 0 ? _h : _h + "color:black; margin-left: 15px";

            var _progress = "<div class='progress' id='" + id + "'>";
            _progress += "<div class='progress-bar progress-bar-striped progress-bar-animated' role='progressbar' ";
            _progress += "aria-valuenow='" + Math.Round(percentuale) + "' aria-valuemin='0' aria-valuemax='100' style='width: " + Math.Round(percentuale) + "%; " + _h + "'>" + percentuale + "%</div>";
            _progress += "</div>";

            return new MvcHtmlString(_progress);
        }

        public static decimal CalcPercentuale(this HtmlHelper helper, int val, int total)
        {
            return Math.Round((decimal)val * 100 / total, 2);
        }

        public static int AiutiDiStato_GetPercentuale(this HtmlHelper helper, int? dimensioneImpresa, int? dirigentiFormazione, int? dirigentiSvantaggiate)
        {
            //REGOLA

            //Dimensione Impresa
            //1   Piccolissima
            //2   Piccola - 30%
            //3   Media - 40%
            //4   Grande - 50%
            //5   Grandissima

            //Dirigenti occupati in formazione appartenenti alle categorie svantaggiate

            //N.Dirigenti in formazione

            //se appartanemte Dirigenti occupati in formazione uguale N.Dirigenti in formazione

            //media 30%
            //grande 40%


            Func<int> getPercentualeNegativo = delegate ()
            {
                var _p = 0;

                if (dirigentiFormazione.HasValue && dirigentiSvantaggiate.HasValue)
                {
                    if (dirigentiFormazione == dirigentiSvantaggiate)
                    {
                        _p = 10;
                    }
                }

                return _p;
            };

            var _percentuale = 0;
            switch (dimensioneImpresa.GetValueOrDefault())
            {
                //piccola
                case 1:
                case 2:
                    _percentuale = 30;
                    break;
                //media
                case 3:
                    _percentuale = 40 - getPercentualeNegativo();
                    break;
                //grande
                case 4:
                case 5:
                    _percentuale = 50 - getPercentualeNegativo();
                    break;

                default:
                    break;
            }

            return _percentuale;
        }

        public static bool IsDimensioneImpresa_Piccola(this HtmlHelper helper, int? dimensioneImpresa)
        {
            return dimensioneImpresa.GetValueOrDefault() == DimensioneImpresa_PiccolaID(helper);
        }

        public static int DimensioneImpresa_PiccolaID(this HtmlHelper helper)
        {
            try
            {
                return FondirConfiguration(helper).DimensioneImpresaPiccolaID;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int MesiDataFineAttivita(this HtmlHelper helper)
        {
            try
            {
                return FondirConfiguration(helper).MesiDataFineAttivita;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int RegimeIVAID(this HtmlHelper helper)
        {
            try
            {
                return FondirConfiguration(helper).RegimeIVAID;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static bool IsRegimeIVA(this HtmlHelper helper, int? regimeIVA)
        {
            return regimeIVA.GetValueOrDefault() == RegimeIVAID(helper);
        }

        public static int StatoID_Piano_Inviato(this HtmlHelper helper)
        {
            try
            {
                return FondirConfiguration(helper).StatoID_Piano_Inviato;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int StatoID_Piano_non_Inviato(this HtmlHelper helper)
        {
            try
            {
                return FondirConfiguration(helper).StatoID_Piano_non_Inviato;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int PercentualePartecipante(this HtmlHelper helper)
        {
            try
            {
                return FondirConfiguration(helper).PercentualePartecipante;
            }
            catch (Exception)
            {
                return 70;
            }
        }

        public static string GetTemplateSettoreRiferimento(this HtmlHelper helper, int settoreID)
        {
            try
            {
                var _templates = FondirConfiguration(helper).SettoreTemplates;

                return _templates.FirstOrDefault(s => s.SettoreID == settoreID).TipoTemplate;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static bool IsCreditizio(this HtmlHelper helper, int settoreID)
        {
            return GetTemplateSettoreRiferimento(helper, settoreID).ToLower() == "creditizio";
        }

        public static bool IsCommercio(this HtmlHelper helper, int settoreID)
        {
            return GetTemplateSettoreRiferimento(helper, settoreID).ToLower() == "commercio";
        }

        #endregion

        public static void RedirectIfNotAjax(this HtmlHelper helper)
        {
            if (!helper.ViewContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                helper.ViewContext.RequestContext.HttpContext.Response.Redirect("/");
        }



        public static MvcHtmlString GetNome(this HtmlHelper helper, string nominativo)
        {
            try
            {
                var _nominativo = "";
                string[] _nomeSplitted = null;
                var _cognome = "";
                var _nome = "";

                _nominativo = nominativo;
                _nomeSplitted = _nominativo.Split(' ');
                _cognome = _nomeSplitted.LastOrDefault();
                _nome = string.Join(" ", _nomeSplitted);

                _nome = _nominativo.Substring(0, _nome.Length - _cognome.Length-1);

                return new MvcHtmlString(_nome);
            }
            catch (Exception)
            {
                return new MvcHtmlString(nominativo);
            }
        }


        public static MvcHtmlString GetCognome(this HtmlHelper helper, string nominativo)
        {
            try
            {
                var _nominativo = "";
                string[] _nomeSplitted = null;
                var _cognome = "";

                _nominativo = nominativo;
                _nomeSplitted = _nominativo.Split(' ');
                _cognome = _nomeSplitted.LastOrDefault();

                return new MvcHtmlString(_cognome);
            }
            catch (Exception)
            {
                return new MvcHtmlString(nominativo);
            }
        }


        //public static string BaseUploaFolderdRichiesta(this HtmlHelper helper)
        //{
        //    try
        //    {
        //        return FondirConfiguration(helper).BaseUploaFolderdRichiesta;
        //    }
        //    catch (Exception)
        //    {
        //        return "";
        //    }
        //}




        //public static string PathDocumenti(this HtmlHelper helper)
        //{
        //    try
        //    {
        //        return FondirConfiguration(helper).PathDocumenti;
        //    }
        //    catch (Exception)
        //    {
        //        return "";
        //    }
        //}

        //public static string PathDocumentiFormulario(this HtmlHelper helper)
        //{
        //    try
        //    {
        //        return FondirConfiguration(helper).PathDocumentiFormulario;
        //    }
        //    catch (Exception)
        //    {
        //        return "";
        //    }
        //}



        public static MvcHtmlString RimuoviTitoliNome(this HtmlHelper helper, object val)
        {
            try
            {
                if (val == null)
                {
                    return new MvcHtmlString("");
                }

                var _listaTitoli = FondirConfiguration(helper).TitoliNome.Split(',');
                var _titolo = val.ToString().TrimStart().TrimEnd().Trim().Split(' ').FirstOrDefault().TrimStart().TrimEnd().Trim().ToLower();

                foreach (var item in _listaTitoli)
                {
                    var _val = item.TrimStart().TrimEnd().Trim().ToLower();

                    if (_titolo.ToString().Equals(_val))
                    {
                        return new MvcHtmlString(val.ToString().TrimStart().TrimEnd().Trim().Substring(_val.Length).TrimStart().TrimEnd().Trim());
                    }
                }

                return new MvcHtmlString(val.ToString());
            }
            catch (Exception)
            {
                return new MvcHtmlString("");
            }
        }



        public static MvcHtmlString ToDate(this HtmlHelper helper, DateTime? val)
        {
            try
            {
                return new MvcHtmlString(val.HasValue ? val.GetValueOrDefault().ToShortDateString() : "-");
            }
            catch (Exception)
            {
                return new MvcHtmlString(DateTime.Now.ToShortDateString());
            }
        }

        public static MvcHtmlString ToCurrency(this HtmlHelper helper, object val)
        {
            try
            {
                return new MvcHtmlString(Convert.ToDecimal(val).ToString("n") + " &euro;");
            }
            catch (Exception)
            {
                return new MvcHtmlString("0,00 &euro;");
            }
        }

        public static MvcHtmlString ToInt(this HtmlHelper helper, decimal valore)
        {
            try
            {
                return new MvcHtmlString(((int)valore).ToString());
            }
            catch (Exception)
            {
                return new MvcHtmlString("0");
            }
        }

        public static MvcHtmlString AlertAttendereOperazioneInCorso(this HtmlHelper helper)
        {
            var _text = "getAlert_Waid(\"Attendere, operazione in corso...\")";
            return new MvcHtmlString(_text);
        }

        public static MvcHtmlString AlertAttendereCaricamentoModulo(this HtmlHelper helper)
        {
            var _text = "getAlert_Waid(\"Attendere, caricamento del modulo in corso...\")";
            return new MvcHtmlString(_text);
        }

        public static MvcHtmlString AlertAttendereRicercaInCorso(this HtmlHelper helper)
        {
            var _text = "getAlert_Waid(\"Attendere, ricerca in corso...\")";
            return new MvcHtmlString(_text);
        }

        public static MvcHtmlString AlertAttendereCaricamentoInCorso(this HtmlHelper helper)
        {
            var _text = "getAlert_Waid(\"Attendere, caricamento in corso...\")";
            return new MvcHtmlString(_text);
        }

        public static MvcHtmlString AlertAttendereVerificaInCorso(this HtmlHelper helper)
        {
            var _text = "getAlert_Waid(\"Attendere, verifica in corso...\")";
            return new MvcHtmlString(_text);
        }

        public static MvcHtmlString AlertDanger(this HtmlHelper helper, string messaggio)
        {
            var _text = "<div class=\"alert alert-danger\">" + messaggio + "</div>";
            return new MvcHtmlString(_text);
        }

        public static MvcHtmlString AlertWarning(this HtmlHelper helper, string messaggio)
        {
            var _text = "<div class=\"alert alert-warning\">" + messaggio + "</div>";
            return new MvcHtmlString(_text);
        }

        public static MvcHtmlString AlertSuccess(this HtmlHelper helper, string messaggio)
        {
            var _text = "<div class=\"alert alert-success\">" + messaggio + "</div>";
            return new MvcHtmlString(_text);
        }



    }
}
