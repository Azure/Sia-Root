using Sia.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sia.Shared.Authentication
{
    public class KeyVaultCertificateRetriever
        : CertificateRetriever
    {
        private readonly X509Certificate2 _certificate;

        public KeyVaultCertificateRetriever(AzureSecretVault certificateVault, string certificateName)
        {
            ThrowIf.NullOrWhiteSpace(certificateName, nameof(certificateName));

            var certTask = certificateVault.GetCertificate(certificateName);
            Task.WaitAll(new Task[] { certTask });
            if (certTask.IsCompleted)
            {
                _certificate = certTask.Result;
            }
        }

        public override X509Certificate2 Certificate => _certificate;
    }
}
