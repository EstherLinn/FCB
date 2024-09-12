using System;
using System.Collections.Generic;

namespace Feature.Wealth.Account.Models.OAuth
{
    public class FcbMemberModel
    {
        /// <summary>
        /// 網銀識別碼
        /// </summary>
        public string WebBankId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string MemberName { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string MemberEmail { get; set; }
        /// <summary>
        /// 生日 from cif
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 風險屬性 from cif
        /// </summary>
        public string Risk { get; set; }
        /// <summary>
        /// 性別 from cif
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 薪轉戶註記 from cif
        /// </summary>
        public string SalFlag { get; set; }
        /// <summary>
        /// 理專姓名 from hris
        /// </summary>
        public string Advisror { get; set; }
        /// <summary>
        /// 理專代號 From cif
        /// </summary>
        public string AdvisrorID { get; set; }
        /// <summary>
        /// 視訊通知
        /// </summary>
        public bool VideoInfoOpen { get; set; }
        /// <summary>
        /// 到價通知
        /// </summary>
        public bool ArrivedInfoOpen { get; set; }
        /// <summary>
        /// 常用功能
        /// </summary>
        public string CommonFunctions { get; set; }
        /// <summary>
        /// 漲跌顏色
        /// </summary>
        public QuoteChangeEunm StockShowColor { get; set; }
        /// <summary>
        /// 登入平台 Line、FB、WebBank(個網、APP)
        /// </summary>
        public PlatFormEunm PlatForm { get; set; }
        /// <summary>
        /// 平台ID
        /// </summary>
        public string PlatFormId { get; set; }

        /// <summary>
        /// 主要往來分行代號
        /// </summary>
        public string MainBranchCode { get; set; }

        /// <summary>
        /// 是否為理顧
        /// </summary>
        public bool IsEmployee { get; set; } = false;

        /// <summary>
        /// 是否為理顧主管
        /// </summary>
        public bool IsManager { get; set; } = false;

        public string CIF_ID { get; set; }

        public FcbMemberModel()
        {
        }

        public FcbMemberModel(string memberName, string memberEmail, bool videoInfoOpen,
            bool arrivedInfoOpen, QuoteChangeEunm stockShowColor, PlatFormEunm platForm, string platFormId)
        {
            this.MemberName = memberName;
            this.MemberEmail = memberEmail;
            this.VideoInfoOpen = videoInfoOpen;
            this.ArrivedInfoOpen = arrivedInfoOpen;
            this.StockShowColor = stockShowColor;
            this.PlatForm = platForm;
            this.PlatFormId = platFormId;
        }

        public FcbMemberModel(string webBankId, string memberName, string memberEmail,
            string risk, string advisror, string advisrorID, bool videoInfoOpen,
            bool arrivedInfoOpen, QuoteChangeEunm stockShowColor, PlatFormEunm platForm,
            string platFormId, DateTime? birthday, string gender, string salFlag, string mainBranchCode,
            bool isEmployee = false, bool isManager = false)
        {
            this.WebBankId = webBankId;
            this.MemberName = memberName;
            this.MemberEmail = memberEmail;
            this.Risk = risk;
            this.Advisror = advisror;
            this.AdvisrorID = advisrorID;
            this.VideoInfoOpen = videoInfoOpen;
            this.ArrivedInfoOpen = arrivedInfoOpen;
            this.StockShowColor = stockShowColor;
            this.PlatForm = platForm;
            this.PlatFormId = platFormId;
            this.Birthday = birthday;
            this.Gender = gender;
            this.SalFlag = salFlag;
            this.IsEmployee = isEmployee;
            this.IsManager = isManager;
            this.MainBranchCode = mainBranchCode;
        }

    }
}
