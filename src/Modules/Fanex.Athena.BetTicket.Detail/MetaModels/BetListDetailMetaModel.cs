namespace Fanex.Athena.BetTicket.Detail.MetaModels
{
    using System;
    using Fanex.Core.Utils;

    public class BetListDetailMetaModel : IValidationMetaModel, IPlayerComMetaModel
    {
        public int SportTypeId { get; set; }

        public string SportTypeName { get; set; }

        public string BaseCurrency { get; set; }

        public DateTime Date { get; set; }

        public int CustId { get; set; }

        public long TransId { get; set; }

        public long TransType { get; set; }

        public long RefCode { get; set; }

        public bool HasPlayerCom { get; set; }

        public bool IsValid => true;
    }
}