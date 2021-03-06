﻿using System;

namespace Arta.Infrastructure.Logging
{
    //TableType and AccountType enums are taken from ResellerCore utilities
    public enum TableType
    {
        General = 0,
        CliPin = 1,
        Matrix = 2,
        ShortCode = 3,
        CommissionPlan = 4,
        Product = 5,
        CustomerProduct = 6,
        PriceVariation = 7,
        Numberport = 10,
        Destination = 11,
        DidCli = 12,
        Did = 13,
        Fraude = 14,
        RoutingTable = 15,
        ReportDefinition = 16,
        ReportRuntime = 17,
        Account = 18,
        Scheme = 19,
        ProfileCost = 20,
        ProfileContact = 21,
        ProfileCliPin = 22,
        DemDefinition = 23,
        DemRuntime = 24,
        Message = 25,
        Currency = 26,
        Language = 27,
        DocText = 28,
        CountryPrefix = 29,
        Country = 30,
        ZipCode = 31,
        ParameterDefinition = 32,
        ParameterSet = 33,
        Operator = 34,
        Region = 35,
        TrafficType = 36,
        TimeZone = 37,
        NtsBaseTable = 38,
        Invoice = 39,
        CommissionNote = 40,
        Bonus = 41,
        Contact = 42,
        BonusPoint = 43,
        SystemSettings = 44,
        BugTracing = 45,
        ConversionCode = 46,
        Promotion = 47,
        CustomerPromotion = 48,
        DocumentTemplate = 49,
        CustomerDocument = 50,
        ExcepProduct = 51,
        ExcepProductPrice = 52,
        ExcepCustomerPrice = 53,
        ExcepCliPin = 54,
        ExcepGlobal = 55,
        TrivialPincodes = 56,
        Distributors = 57,
        Cdrs = 58,
        ExcepCustomer = 59,
        Task = 60,
        VodslIad = 61,
        VodslMgc = 62,
        VodslAbbrNumbCmp = 63,
        VodslAbbrNumbGrp = 64,
        VodslAbbrNumbExt = 65,
        CustomReplication = 66,
        MobSim = 67,
        MobImsi = 68,
        MobMsisdn = 69,
        Package = 70,
        PackContact = 71,
        PackCliPin = 72,
        ExternalTransaction = 73,
        ReloadHistory = 74,
        CallMode = 75,
        VodslExtension = 76,
        Rerating = 77,
        Bundle = 78,
        Marketing = 79,
        RatingUnit = 80,
        Ussaccount = 81,
        ProfileUssaccount = 82,
        SmscProvider = 83,
        SmscCause = 84,
        RtVoice = 85,
        RtDid = 86,
        RtCountry = 87,
        RtTime = 88,
        RtRoute = 89,
        RtCarrier = 90,
        RtLcr = 91,
        RtIp = 92,
        RtAlarm = 93,
        RtCause = 94,
        RtMsi = 95,
        RtPredest = 96,
        RtPredid = 97,
        RtPrecli = 98,
        RtNts = 99,
        RtZone = 100,
        RtBearer = 101,
        RtVlr = 102,
        Premiumcontent = 103,
        JobCop = 104,
        Timeframe = 105,
        Pmnmapping = 106,
        Changesets = 107,
        Changesetdetail = 108,
        Concealeddest = 109,
        Lifecyclemanagement = 110,
        Lifecycleexception = 111,
        Serviceusage = 112,
        Externalbillingreference = 113,
        Wholesalebundlemapping = 114,
        Bundledefinition = 115,
        Bundleusage = 116,
        Bundleusageset = 117,
        SelfcareTranslation = 118,
        CreditThreshold = 119,
        SumPlan = 120,
        EventManagement = 121,
        BundleGrantedEndUser = 122,
        IotProductPlan = 123,
        IoTBulkAction = 124,
    }

    public enum AccountType
    {
        All = -1,
        NtUser = 0,
        NtGroup = 1,
        SqlUser = 2,
        SqlServerRole = 3,
        SqlDatabaseRole = 4,
        WebUser = 5,
        System = 100
    }

    public enum DbType
    {
        Central,
        Csp,
        Partner
    }

    public class CommandLogEvent
    {
        public DbType DbType { get; set; }
        public string CspOrPartner { get; set; }
        public DateTime DateTime { get; set; }
        public AccountType AccountType { get; set; }
        public string Account { get; set; }
        public TableType TableType { get; set; }
        public string TableKey { get; set; }
        public string Command { get; set; }
        public string Value { get; set; }
        public string Changed { get; set; }
    }
}
