using Sitecore.Data;

namespace Feature.Wealth.Component.Models.Calculate
{
    public class Template
    {
        public struct Calculate
        {
            public static readonly ID Id = new ID("{E2B44A81-7D97-4231-8932-78381DC5776F}");

            public struct Fields
            {
                public static readonly ID MainTitle = new ID("{BBAD1259-2A5C-406C-ACA1-7723E8DE27D6}");
                public static readonly ID MainContent = new ID("{E35BD6E6-8C20-49A5-B929-5E4FD5A3F33E}");
                public static readonly ID Image = new ID("{8BB3CEFC-DF8D-411D-BD60-297DDF34538A}");
                public static readonly ID AnticipatedInvestmentTipTitle = new ID("{7F9176BC-7967-4448-BAE2-2FA8CE706CEE}");
                public static readonly ID AnticipatedInvestmentTipContent = new ID("{593B2249-BED8-4774-967B-0F44799F55C0}");
                public static readonly ID LifeExpectancyTipTitle = new ID("{5EC4170A-8B59-4D02-8713-30C250B97C4B}");
                public static readonly ID LifeExpectancyTipContent = new ID("{A67FD7D3-CA7E-4B9C-A806-9EFE906DA0E9}");
                public static readonly ID RetirementExpensesTipTitle = new ID("{874941F5-6971-4A3C-9789-FCE41B7559FA}");
                public static readonly ID RetirementExpensesTipContent = new ID("{9CC4FC0B-2F69-43E5-9AD2-3470F0B4611A}");
                public static readonly ID EstimatedPensionTipTitle = new ID("{2204FCD4-B159-44A0-9193-D77E81D558A0}");
                public static readonly ID EstimatedPensionTipContent = new ID("{CE40CEBF-BBC6-45D4-AD9C-FF6298E22551}");
                public static readonly ID SuccessImage = new ID("{928BA457-1FA5-46D7-9A6B-E98B592DE925}");
                public static readonly ID SuccessContent = new ID("{BB7D3CDD-33AB-4F8A-900D-169015C03CAE}");
                public static readonly ID UnsuccessfulImage = new ID("{AF7DE8E1-36DA-49AD-AE76-50C3621F8EED}");
                public static readonly ID UnsuccessfulContent = new ID("{DB09C138-CB42-4485-992C-D5A01240BE20}");
                public static readonly ID ExpectedReturnRemarks = new ID("{94FFECEE-3112-4AE4-8DED-B94870DC3885}");
                public static readonly ID DefaultRiskAttributes = new ID("{54721E54-E73F-480E-B5D9-1B4A03CD6594}");
                public static readonly ID RecommendedProductContent = new ID("{88C88486-D726-4E69-8DE7-0EAFEACF9436}");
                public static readonly ID ConservativeRiskImage = new ID("{061C9AD5-DEB5-4871-972C-58B9C5979DF9}");
                public static readonly ID ConservativeRiskStockAllocation = new ID("{84CB4BBC-6E9F-4F5B-AB75-6A6E76F54A1C}");
                public static readonly ID ConservativeRiskBondAllocation = new ID("{FEF9B7DA-2A2F-4899-B36C-3804C676C969}");
                public static readonly ID ConservativeRiskCurrencyAllocation = new ID("{0F0DF087-72F2-4097-9C22-C853CB0885B5}");
                public static readonly ID ConservativeRiskStockAllocationFieldName = new ID("{878476DB-8034-429B-8656-56CE56899345}");
                public static readonly ID ConservativeRiskBondAllocationFieldName = new ID("{A481E8B9-09D8-47B6-A131-C78CDBF3321B}");
                public static readonly ID ConservativeRiskCurrencyAllocationFieldName = new ID("{B49301CA-921B-4507-AC0D-53184DC62DDC}");
                public static readonly ID ConservativeRiskStockAllocationText = new ID("{139256DA-D009-47B9-803D-F1BC5BC2C9E6}");
                public static readonly ID ConservativeRiskBondAllocationText = new ID("{3E4130C7-C78B-4740-9ED6-A9329414D0BB}");
                public static readonly ID ConservativeRiskCurrencyAllocationText = new ID("{6196D994-DE64-43A0-ABAB-4058268A0A4C}");
                public static readonly ID RobustRiskImage = new ID("{C3253D5F-1B0A-4C77-83C3-99D8C560268B}");
                public static readonly ID RobustRiskStockAllocation = new ID("{98929709-1E61-4054-AFE7-81A5AF4C6ABA}");
                public static readonly ID RobustRiskBondAllocation = new ID("{4587B775-DF49-4A19-805D-EC0151FB5AC3}");
                public static readonly ID RobustRiskCurrencyAllocation = new ID("{A3B5DBA3-AAD1-4D75-8F2C-F12AB3D6304B}");
                public static readonly ID RobustRiskStockAllocationFieldName = new ID("{06E79885-A29E-4BF7-B6F2-4E080E59D6C6}");
                public static readonly ID RobustRiskBondAllocationFieldName = new ID("{1F268895-2B52-48AB-8D56-8D87214235C1}");
                public static readonly ID RobustRiskCurrencyAllocationFieldName = new ID("{88B18829-ED61-4D0B-B378-D6E14CA719E7}");
                public static readonly ID RobustRiskStockAllocationText = new ID("{3854DF59-FE34-4883-AC14-84CA6CA3BC4C}");
                public static readonly ID RobustRiskBondAllocationText = new ID("{B04DBECA-C495-40DA-BD4E-EC7C60ACE35D}");
                public static readonly ID RobustRiskCurrencyAllocationText = new ID("{A28877E5-303F-43D0-A476-DAB5DAFC9975}");
                public static readonly ID PositiveRiskImage = new ID("{E5C995A9-FD2B-441A-B918-52785CF105A0}");
                public static readonly ID PositiveRiskStockAllocation = new ID("{59CB2B82-C489-4EC5-9DF2-FE3D2939DC80}");
                public static readonly ID PositiveRiskBondAllocation = new ID("{A399858F-42FC-42C0-BB9E-F7BF82A5A69E}");
                public static readonly ID PositiveRiskCurrencyAllocation = new ID("{3B2B3013-E4F7-440D-A3DE-4B6443B054B8}");
                public static readonly ID PositiveRiskStockAllocationFieldName = new ID("{5C0A9EDD-0CA7-4C7A-A47D-14999A10DC0E}");
                public static readonly ID PositiveRiskBondAllocationFieldName = new ID("{3E4252A5-DE05-4A0D-AB48-C4B5782CD6A1}");
                public static readonly ID PositiveRiskCurrencyAllocationFieldName = new ID("{5A5320D4-F5DF-4FAE-946C-7F4F73E47499}");
                public static readonly ID PositiveRiskStockAllocationText = new ID("{13A2867F-2816-42CD-A92F-1408A501AF36}");
                public static readonly ID PositiveRiskBondAllocationText = new ID("{92F8359F-1390-45E4-87E7-132A81068B12}");
                public static readonly ID PositiveRiskCurrencyAllocationText = new ID("{4E0AC92B-4366-4D1B-88B7-15D97DBA284B}");
                public static readonly ID RemoteConsultationSuccessTitle = new ID("{FFEE7E90-EFEC-42EE-B102-2C60766C12E1}");
                public static readonly ID RemoteConsultationSuccessContent = new ID("{07158773-FDEB-4207-8AF3-660A0F51D9D3}");
                public static readonly ID RemoteConsultationSuccessButtonText = new ID("{39C7F9EE-E9B2-47F1-B81F-54A262B295B8}");
                public static readonly ID RemoteConsultationSuccessButtonLink = new ID("{57B825FF-6BBA-4384-8CC8-27661DDE841E}");
                public static readonly ID RemoteConsultationSuccessImage = new ID("{7DA96E92-17D2-439B-927B-094232C2E3E0}");
                public static readonly ID RemoteConsultationSuccessfulTitle = new ID("{9585CB46-221E-4F00-90FE-0E44D8CBEDD0}");
                public static readonly ID RemoteConsultationSuccessfulContent = new ID("{98A900FB-5A82-4825-84DF-74437798FAD7}");
                public static readonly ID RemoteConsultationSuccessfulButtonText = new ID("{F9C1F3EE-FB81-4190-A443-350B86FE0844}");
                public static readonly ID RemoteConsultationSuccessfulButtonLink = new ID("{5DBEBF93-25DE-48EA-9C22-F8AADE31D00C}");
                public static readonly ID RemoteConsultationSuccessfulImage = new ID("{226055AC-83B9-4DC0-86FB-7B428BB791E0}");
                public static readonly ID FundID = new ID("{48CDA75C-20FA-4074-9BDB-0A90AE89EEB6}");
            }
        }
    }
}