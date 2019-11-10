using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VillaMouzinho.Web.Controllers.Core
{
    [Authorize]
    public class CoreController : Controller
    {
        #region Properties
        public List<string> FullBreadcrumb;
        #endregion

        #region Constructor
        public CoreController()
        {
            FullBreadcrumb = new List<string>();
        }
        #endregion

        public void BuildFastBreadCrumb(List<string> routes, int type)
        {
            if (routes.Count > 0)
            {
                if (type == 1)
                {
                    ViewBag.FullBreadcrumb = routes;
                }
            }
            else
            {
                ViewBag.FullBreadcrumb = null;
            }
        }

        public static string NormalizeStringForUrl(string url)
        {
            String normalizedString = url.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder(url.Length);

            foreach (char c in normalizedString)
            {
                switch (CharUnicodeInfo.GetUnicodeCategory(c))
                {
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                        stringBuilder.Append(Char.ToLower(c));
                        break;
                    case UnicodeCategory.DecimalDigitNumber:
                        stringBuilder.Append(c);
                        break;
                    case UnicodeCategory.SpaceSeparator:
                    case UnicodeCategory.ConnectorPunctuation:
                    case UnicodeCategory.DashPunctuation:
                        stringBuilder.Append('-');
                        break;
                }
            }
            string result = stringBuilder.ToString();
            return String.Join("-", result.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries)); // remove duplicate underscores
        }
    }
}