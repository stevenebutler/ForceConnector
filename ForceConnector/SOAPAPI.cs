using System;
using ForceConnector.Partner;
using Microsoft.VisualBasic;

namespace ForceConnector
{
    static class SOAPAPI
    {
        private static SoapClient soapClient;
        private static SessionHeader soapSessionHeader;
        private static CallOptions callOptions;
        private static PackageVersion[] packageVersions;
        private static MiniMETA.MetadataPortTypeClient metaClient;
        private static MiniMETA.SessionHeader metaSessionHeader;
        private static MiniMETA.AllOrNoneHeader allOrNoneHeader;

        public static DescribeGlobalSObjectResult[] getSObjectList()
        {
            var dgr = new DescribeGlobalResult();
            LimitInfo[] limitInfo;
            try
            {
                if (setSoapBinding())
                {
                    limitInfo = soapClient.describeGlobal(soapSessionHeader, callOptions, packageVersions, out dgr);
                    return dgr.sobjects;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("getSObjectList Exception!" + Constants.vbCrLf + ex.Message);
            }

            return null;
        }

        public static DescribeSObjectResult DescribeSObject(string objname, string baseLang)
        {
            var dsr = new DescribeSObjectResult();
            LimitInfo[] limitInfo;
            try
            {
                if (setSoapBinding())
                {
                    var localeOptions = new LocaleOptions();
                    localeOptions.language = baseLang;
                    limitInfo = soapClient.describeSObject(soapSessionHeader, callOptions, packageVersions, localeOptions, objname, out dsr);
                    return dsr;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("getSObjectList Exception!" + Constants.vbCrLf + ex.Message);
            }

            return null;
        }




        // ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // ' Common Functions Block
        // ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        public static bool setSoapBinding()
        {
            if (!Util.checkSession())
            {
                if (!ForceConnector.LoginToSalesforce())
                    goto done;
            }

            if (Util.checkSession())
            {
                if (soapClient is null)
                {
                    soapClient = ThisAddIn.soapClient;
                }

                if (soapSessionHeader is null)
                {
                    soapSessionHeader = ThisAddIn.soapSessionHeader;
                }

                callOptions = new CallOptions();
                packageVersions = new[] { new PackageVersion() };
                return true;
            }

        done:
            ;
            return false;
        }
    }
}