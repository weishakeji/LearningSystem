using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
//using NBear.Data;
using System.Data.SqlClient;

namespace Song.Site.Manage.Sys
{
    public partial class DataBaseBackup : Extend.CustomPage
    {
        //数据库完整物理物路径
        private string dataBaseHy = "";
        //数据库备份目录
        private string backDir = "";
        //数据库备份文件后缀名
        private string backExt = ".bak";
        //数据库类型，仅限access与sqlserver；
        private string dbType = "access";
        //要操作的数据库名称。
        private string dbName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //数据库类型
            dbType = WeiSha.Common.Server.DatabaseType;
            if (dbType == "access")
            {
                backDir = "accBackup";
                dataBaseHy = WeiSha.Common.Server.DatabaseFilePath;
            }
            else if (dbType == "sqlServer")
            {
                backDir = "sqlSBackup";
                backExt = ".sasp";
                ConnectionStringSettings connsett = ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1];
                string str = connsett.ConnectionString;

                int a = str.IndexOf("Initial Catalog=");
                a = a == -1 ? str.IndexOf("DataBase=") + 9 : a + 16;
                string strjie = str.Substring(a);
                dbName = str.Substring(a, strjie.IndexOf(";"));

            }
            else
            {
                Message.Prompt("此功能仅限Access版和SQL Server版。");
                return;
            }
            //获取参数
            getDataBase();
            if (!IsPostBack)
            {
                //绑定数据
                BindData();
            }
            //this.Response.Write(dataBaseHy);
        }
        /// <summary>
        /// 获取数据库的地址；
        /// </summary>
        private void getDataBase()
        {
            try
            {
                string backPath = Server.MapPath("~/App_Data/")+backDir;
                //备份目录
                DirectoryInfo di = new DirectoryInfo(backPath);
                if (!di.Exists)
                {
                    //如果该目录不存在，则创建
                    di.Create();
                }
                backDir = di.FullName;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        private void BindData()
        {
            try
            {
                //备份目录
                DirectoryInfo di = new DirectoryInfo(this.backDir);

                DataTable dt = new DataTable("DataBase");
                DataColumn dc;
                //文件名
                dc = new DataColumn("file", Type.GetType("System.String"));
                dt.Columns.Add(dc);
                //路径名
                dc = new DataColumn("path", Type.GetType("System.String"));
                dt.Columns.Add(dc);
                //创建时间
                dc = new DataColumn("time", Type.GetType("System.DateTime"));
                dt.Columns.Add(dc);
                //文件大小
                dc = new DataColumn("size", Type.GetType("System.Int32"));
                dt.Columns.Add(dc);
                //路径名
                dc = new DataColumn("type", Type.GetType("System.String"));
                dt.Columns.Add(dc);
                //备份目录下的所有文件
                FileInfo[] fi = di.GetFiles();
                foreach (FileInfo file in fi)
                {
                    if (file.Extension != backExt && file.Extension.ToLower() != ".master")
                    {
                        continue;
                    }
                    DataRow dr = dt.NewRow();
                    dr["file"] = file.Name.Substring(0, file.Name.LastIndexOf('.'));
                    dr["path"] = file.FullName;
                    dr["time"] = file.CreationTime;
                    dr["size"] = file.Length / 1024;
                    dr["type"] = file.Extension.ToLower() == ".backup" ? "系统" : "";
                    dt.Rows.Add(dr);
                }
                DataView dv = dt.DefaultView;
                dv.Sort = "time desc";
                GridView1.DataSource = dv;
                GridView1.DataKeyNames = new string[] { "path" };
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 增加备份
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddEvent(object sender, EventArgs e)
        {

            string backName = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
            if (dbType == "access")
            {
                try
                {
                    FileInfo fi = new FileInfo(dataBaseHy);
                    fi.CopyTo(backDir + "\\" + backName + "" + backExt, true);
                    BindData();
                }
                catch (Exception ex)
                {
                    Message.ExceptionShow(ex);
                }
            }
            else if (dbType == "sqlServer")
            {
                sqlServerBackup(dbName, backDir+"\\"+backName+backExt);
                BindData();
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            try
            {
                string keys = GridView1.GetKeyValues;
                foreach (string file in keys.Split(','))
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.Exists)
                    {
                        if (fi.Extension.ToLower() != ".backup")
                            fi.Delete();
                    }

                }
                BindData();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
                int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
                string file = this.GridView1.DataKeys[index].Value.ToString();
                FileInfo fi = new FileInfo(file);
                if (fi.Exists)
                {
                    if (fi.Extension.ToLower() != ".backup")
                        fi.Delete();
                }
                BindData();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 还原数据库，按钮栏的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RecoverEvent(object sender, EventArgs e)
        {
            try
            {
                string keys = GridView1.GetKeyValues;
                foreach (string file in keys.Split(','))
                {
                    FileInfo backDbase = new FileInfo(file);
                    if (dbType == "access")
                        backDbase.CopyTo(dataBaseHy, true);
                    else if (dbType == "sqlServer")
                        sqlServerRestore(dbName, backDbase.FullName);
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }          
        }
        /// <summary>
        /// 还原数据库，数据行的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRecover_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gr = (GridViewRow)((WeiSha.WebControl.RowRecover)sender).Parent.Parent;
                //备份文件
                string backfile = GridView1.DataKeys[gr.RowIndex].Value.ToString();
                FileInfo backDbase = new FileInfo(backfile);
                if(dbType == "access")
                    backDbase.CopyTo(dataBaseHy, true);
                else if (dbType == "sqlServer")
                {
                    sqlServerRestore(dbName, backDbase.FullName);
                }
                new Extend.Scripts(this).Alert("还原成功！");
            }
            catch (Exception ex)
            {
                new Extend.Scripts(this).Alert(ex.Message);
            }           
        }
        /// <summary>
        /// SQL Server数据库的备份。
        /// </summary>
        /// <param name="dbName">数据库名称</param>
        /// <param name="fileName">要备份的位置(绝对路径)</param>
        public void sqlServerBackup(string dbName, string fileName)
        {
            //拼接SQL语句。
            string sql = "backup database " + dbName + " to disk='" + fileName + "' WITH FORMAT";
            //Gateway.Default.fr.FromCustomSql(sql);
            zhixingSql(sql);
        }
        /// <summary>
        /// SQL Server数据库的恢复，生成SQL语句到函数中执行。
        /// </summary>
        /// <param name="baseName">数据库名称</param>
        /// <param name="fileName">要还原的备份文件的位置(绝对路径带文件名称和扩展名)</param>
        public void sqlServerRestore(string baseName, string fileName)
        {
            string sql = "use master " +
                         "declare @sql varchar(100) while 1=1 begin select top 1 @sql='kill '+cast(spid as varchar(3)) from master..sysprocesses where spid>50 and spid<>@@spid and dbid=db_id('" + baseName + "') " +
                         "if @@rowcount=0 break exec(@sql) end RESTORE DATABASE " + baseName + " FROM disk = '" + fileName + "' WITH REPLACE";
            zhixingSql(sql);
        }
        /// <summary>
        /// 执行生成的SQL语句。
        /// </summary>
        /// <param name="sql">要执行的SQL语句</param>
        public void zhixingSql(string sql)
        {
            try
            {
                //执行SQL语句。
                ConnectionStringSettings connsett = ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1];
                string ConnectString = connsett.ConnectionString;
                SqlConnection connection = new SqlConnection(ConnectString);
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                new Extend.Scripts(this).Alert(ex.Message);
            }
        }
    }
}
