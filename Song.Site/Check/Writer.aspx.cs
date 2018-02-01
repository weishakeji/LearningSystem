using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using WeiSha.Common;

namespace Song.Site.Check
{
    public partial class Writer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnWriter_Click(object sender, EventArgs e)
        {
            //默认上传文件夹
            string phy = WeiSha.Common.Upload.Get.Root.Physics;
            string file = phy + "test.txt";
            try
            {
                if (File.Exists(file)) File.Delete(file);
                StreamWriter sw = new StreamWriter(phy + "test.txt");
                sw.WriteLine("测试写入权限所生成的文件");
                sw.WriteLine(DateTime.Now.ToString());
                sw.Close();
                if (File.Exists(file)) File.Delete(file);
                plError.Visible = false;
                lbScuess.Visible = true;
            }
            catch (Exception ex)
            {
                ltError.Text = ex.Message;
                plError.Visible = true;
                lbScuess.Visible = false;
            }
        }
    }
}