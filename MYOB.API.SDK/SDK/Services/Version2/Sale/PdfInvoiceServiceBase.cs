﻿using System;
using System.IO;
using System.Net;
#if ASYNC
using System.Threading.Tasks;
#endif
using MYOB.AccountRight.SDK.Communication;
using MYOB.AccountRight.SDK.Contracts;
using MYOB.AccountRight.SDK.Contracts.Version2;
using MYOB.AccountRight.SDK.Contracts.Version2.Sale;

namespace MYOB.AccountRight.SDK.Services.Sale
{
    /// <summary>
    /// A service that provides access to the PDF generated by an <see cref="Invoice"/> resource
    /// </summary>
    public abstract class PdfInvoiceServiceBase<T> : MutableService<T>, IPdfService where T : BaseEntity
    {
        const string PdfMimetype = "application/pdf";

        /// <summary>
        /// Initialise a base instance
        /// </summary>
        /// <param name="configuration">The configuration required to communicate with the API service</param>
        /// <param name="webRequestFactory">A custom implementation of the <see cref="WebRequestFactory"/>, if one is not supplied a default <see cref="WebRequestFactory"/> will be used.</param>
        /// <param name="keyService">An implementation of a service that will store/persist the OAuth tokens required to communicate with the cloud based API at http://api.myob.com/accountright </param>
        protected PdfInvoiceServiceBase(IApiConfiguration configuration, IWebRequestFactory webRequestFactory, IOAuthKeyService keyService) 
            : base(configuration, webRequestFactory, keyService)
        {
        }

        /// <summary>
        /// Gets an Invoice as Pdf
        /// </summary>
        /// <param name="cf">A company file reference that has been retrieved</param>
        /// <param name="invoiceUid">The identifier of the entity to retrieve</param>
        /// <param name="credentials">The credentials to access the company file</param>
        /// <param name="template">The Template Name</param>
        /// <returns></returns>
        public Stream GetInvoiceFormAsPdf(CompanyFile cf, Guid invoiceUid, ICompanyFileCredentials credentials, string template)
        {
            return MakeApiGetRequestSyncPdf(BuildUri(cf, invoiceUid, template), PdfMimetype, credentials);
        }

        /// <summary>
        /// Gets an Invoice as Pdf
        /// </summary>
        /// <param name="cf">A company file reference that has been retrieved</param>
        /// <param name="invoiceUid">The identifier of the entity to retrieve</param>
        /// <param name="credentials">The credentials to access the company file</param>
        /// <param name="template">The Template Name</param>
        /// <param name="onComplete">The action to call when the operation is complete</param>
        /// <param name="onError">The action to call when the operation has an error</param>
        public void GetInvoiceFormAsPdf(CompanyFile cf, Guid invoiceUid, ICompanyFileCredentials credentials, string template, Action<HttpStatusCode, Stream> onComplete, Action<Uri, Exception> onError)
        {
            MakeApiGetRequestDelegateStream(BuildUri(cf, invoiceUid, template), PdfMimetype, credentials, onComplete, onError);
        }

#if ASYNC
        /// <summary>
        /// Gets an Invoice as Pdf
        /// </summary>
        /// <param name="cf">A company file reference that has been retrieved</param>
        /// <param name="invoiceUid">The identifier of the entity to retrieve</param>
        /// <param name="credentials">The credentials to access the company file</param>
        /// <param name="template">The Template Name</param>
        /// <returns></returns>
        public Task<Stream> GetInvoiceFormAsPdfAsync(CompanyFile cf, Guid invoiceUid, ICompanyFileCredentials credentials, string template)
        {
            return MakeApiGetRequestAsyncStream(BuildUri(cf, invoiceUid, template), PdfMimetype, credentials);
        }
#endif

        private Uri BuildUri(CompanyFile cf, Guid invoiceUid, string template)
        {
            return BuildUri(cf, invoiceUid, queryString: string.Format("templatename={0}", template));
        }
    }
}
