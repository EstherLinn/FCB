namespace Feature.Wealth.Account.Models.SingleSignOn
{
    public interface ISsoUser
    {
        /// <summary>
        /// 使用者帳號名稱
        /// </summary>
        public abstract string LocalName { get; }
    }
}