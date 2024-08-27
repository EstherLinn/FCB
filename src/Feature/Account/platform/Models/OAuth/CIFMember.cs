using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Account.Models.OAuth
{
    public class CIFMember
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string CIF_CUST_NAME { get; set; }
        /// <summary>
        /// 性別
        /// </summary>
        public string CIF_CUST_ATTR { get; set; }
        /// <summary>
        /// 薪轉戶註記
        /// </summary>
        public string CIF_SAL_FLAG { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string CIF_E_MAIL_ADDRESS { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? CIF_ESTABL_BIRTH_DATE { get; set; }
        /// <summary>
        /// 6碼識別碼
        /// </summary>
        public string CIF_PROMO_CODE { get; set; }
        /// <summary>
        /// 身分證
        /// </summary>
        public string CIF_ID { get; set; }
        /// <summary>
        /// 風險屬性
        /// </summary>
        public string CIF_EMP_RISK { get; set; }
        /// <summary>
        /// 所屬理專ID
        /// </summary>
        public string CIF_AO_EMPNO { get; set; }
        /// <summary>
        /// 所屬理專姓名
        /// </summary>
        public string CIF_AO_EMPName { get; set; }
        /// <summary>
        /// HRIS 員工代號 對應 所屬理專ID
        /// </summary>
        public string HRIS_EmployeeCode { get; set; }

        /// <summary>
        /// 是否為員工
        /// </summary>
        public bool IsEmployee { get; set; } = false;

        /// <summary>
        /// 是否為主管
        /// </summary>
        public bool IsManager { get; set; } = false;
    }
}
