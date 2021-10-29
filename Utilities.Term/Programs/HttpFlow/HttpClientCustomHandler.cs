using System.Net;
using System.Net.Http;

namespace Utilities.Term.Programs.HttpFlow
{
    internal class HttpClientCustomHandler : HttpClientHandler
    {
        public HttpClientCustomHandler()
        {
            ServerCertificateCustomValidationCallback = delegate
            {
                //bool validationStatus = true;
                //if ((errors & SslPolicyErrors.RemoteCertificateChainErrors) != 0)
                //{
                //    Console.WriteLine("SslPolicyErrors.RemoteCertificateChainErrors");
                //    validationStatus = false;
                //}

                //if ((errors & SslPolicyErrors.RemoteCertificateNameMismatch) != 0)
                //{
                //    Console.WriteLine("SslPolicyErrors.RemoteCertificateNameMismatch");
                //    validationStatus = false;
                //}
                //else if ((errors & SslPolicyErrors.None) != 0)
                //{
                //    Console.WriteLine("SslPolicyErrors.None");
                //    validationStatus = true;
                //}

                //Console.WriteLine(x509Cer2.ToString(true));

                return true;
            };
        }
    }
}