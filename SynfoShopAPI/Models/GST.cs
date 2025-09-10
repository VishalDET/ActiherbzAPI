using System;
using System.Collections.Generic;

namespace SynfoShopAPI.Models
{
    public class GSTToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expiry { get; set; }
    }
    public class GSTTokenView
    {
        public int Id { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expiry { get; set; }
        public int Validity { get; set; }
    }

    public class GSTAddress 
    {
        public string Bnm { get; set; }
        public string Loc { get; set; }
        public string St { get; set; }
        public string Bno { get; set; }
        public string Dst { get; set; }
        public string Pncd { get; set; }
        public string Stcd { get; set; }
        public string geocodelvl { get; set; }
        public string flno { get; set; }
        public string lg { get; set; }
    }

    public class GSTPradr
    {
        public GSTAddress Addr { get; set; }
        public string Ntr { get; set; }
    }

    public class GSTData
    {
        public string StjCd { get; set; }
        public string Dty { get; set; }
        public string Stj { get; set; }
        public string Lgnm { get; set; }
        public List<string> Adadr { get; set; }
        public string Cxdt { get; set; }
        public List<string> Nba { get; set; }
        public string Gstin { get; set; }
        public string Lstupdt { get; set; }
        public string Ctb { get; set; }
        public string Rgdt { get; set; }
        public GSTPradr Pradr { get; set; }
        public string CtjCd { get; set; }
        public string TradeNam { get; set; }
        public string Sts { get; set; }
        public string Ctj { get; set; }
        public string EinvoiceStatus { get; set; }
    }

    public class GSTApiResponse
    {
        public int Code { get; set; }
        public long Timestamp { get; set; }
        public string TransactionId { get; set; }
        public GSTData Data { get; set; }
    }

    public class GSTApiResponseNew
    {
        public string Pncd { get; set; }
        public string Bno { get; set; }
        public string Bnm { get; set; }
        public string Loc { get; set; }
        public string St { get; set; }
        public string Lgnm { get; set; }
        public string Dst { get; set; }
    }

    public class Address
    {
        public string lg { get; set; }
        public string bnm { get; set; }
        public string stcd { get; set; }
        public string flno { get; set; }
        public string pncd { get; set; }
        public string city { get; set; }
        public string lt { get; set; }
        public string dst { get; set; }
        public string st { get; set; }
        public string bno { get; set; }
        public string loc { get; set; }
    }

    public class Pradr
    {
        public string ntr { get; set; }
        public Address addr { get; set; }
    }

    public class TaxpayerInfo
    {
        public string ctj { get; set; }
        public Pradr pradr { get; set; }
        public string tradeNam { get; set; }
        public string lgnm { get; set; }
        public string cxdt { get; set; }
        public string ctb { get; set; }
        public string rgdt { get; set; }
        public List<object> adadr { get; set; }
        public string dty { get; set; }
        public string lstupdt { get; set; }
        public List<string> nba { get; set; }
        public string stjCd { get; set; }
        public string stj { get; set; }
        public string gstin { get; set; }
        public string ctjCd { get; set; }
        public string sts { get; set; }
    }

    public class Compliance
    {
        public object filingFrequency { get; set; }
    }

    public class Root
    {
        public TaxpayerInfo taxpayerInfo { get; set; }
        public List<object> filing { get; set; }
        public Compliance compliance { get; set; }

        
    }
}
