using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 账单详情
    /// </summary>
    public class MoneyDetail : BasePage
    {

        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            //默认打开，GET方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                Song.Entities.MoneyAccount money = Business.Do<IAccounts>().MoneySingle(id);
                if (money == null) return;
                this.Document.SetValue("money", money);
            }
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = WeiSha.Common.Request.Form["action"].String;
                switch (action)
                {
                    case "remark":
                        modify_remark();
                        break;
                    case "delete":
                        modify_delete();
                        break;
                }
                
            }
        }
        /// <summary>
        /// 修改资金流水记录的备注
        /// </summary>
        private void modify_remark()
        {
            string remark = WeiSha.Common.Request.Form["remark"].String;
            Song.Entities.MoneyAccount money = Business.Do<IAccounts>().MoneySingle(id);
            money.Ma_Remark = remark;
            Business.Do<IAccounts>().MoneySave(money);
            Response.Write("1");
            Response.End();
        }
        /// <summary>
        /// 删除资金流水
        /// </summary>
        private void modify_delete()
        {
            try
            {
                Business.Do<IAccounts>().MoneyDelete(id);
                Response.Write("1");               
            }
            catch
            {
            }
            Response.End();
        }
    }
}