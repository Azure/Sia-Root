using Microsoft.Extensions.Logging;
using Sia.Shared.Authentication.Certificates;
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
        private readonly string _certName;
        private readonly AzureSecretVault _vault;

        public KeyVaultCertificateRetriever(
            AzureSecretVault certificateVault,
            string certificateName,
            ILoggerFactory loggerFactory
        ) : base(loggerFactory.CreateLogger<KeyVaultCertificateRetriever>())
        {
            _certName = ThrowIf.NullOrWhiteSpace(certificateName, nameof(certificateName));
            _vault = ThrowIf.Null(certificateVault, nameof(certificateVault));
        }

        protected override X509Certificate2 RetrieveCertificate()
        {
            var certTask = _vault.GetCertificate(_certName);
            Task.WaitAll(new Task[] { certTask });
            if (certTask.IsCompleted)
            {
                var certificate = certTask.Result;
                _logger.LogDebug($"Certificate retrieved successfully: {certificate.FriendlyName}");
                return certificate;
            }
            else
            {
                if (certTask.Exception is null)
                {
                    _logger.LogError($"Error with no exception when"
                        + " attempting to load certificate from vault");
                    throw new CertificateRetrievalException();
                }
                else
                {
                    _logger.LogError(
                       certTask.Exception,
                       "Exception when attempting to load certificate from vault"
                    );
                    throw new CertificateRetrievalException(certTask.Exception);
                }
            }
        }
    }
}
