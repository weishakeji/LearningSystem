﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Data;

namespace Song.Site.Manage.Course
{
    public partial class Courses_Outlines : Extend.CustomPage
    {
        //课程id
        protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;      

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //隐藏确定按钮
                EnterButton btnEnter = (EnterButton)Master.FindControl("btnEnter");
                btnEnter.Visible = false; 
            }
        }        
    }
}