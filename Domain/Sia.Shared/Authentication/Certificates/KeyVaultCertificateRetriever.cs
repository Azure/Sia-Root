﻿using Microsoft.Extensions.Logging;
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

        protected override async Task<X509Certificate2> RetrieveCertificateAsync()
        {
            try
            {
                return await _vault.GetCertificateAsync(_certName);
            }
            catch(Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Exception when attempting to load certificate from vault"
                );
                throw new CertificateRetrievalException(ex);
            }
        }
    }
}
