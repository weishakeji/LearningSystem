﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.ViewData
{
    /// <summary>
    /// 接口的父类
    /// </summary>
    public abstract class ViewMethod
    {
        /// <summary>
        /// 客户端传来的参数
        /// </summary>
        public Letter Letter
        {
            get
            {
                System.Web.HttpContext _context = System.Web.HttpContext.Current;
                Letter letter = new Letter(_context);
                return letter;
            }
        }
        /// <summary>
        /// 当前登录学员
        /// </summary>
        public Song.Entities.Accounts Student
        {
            get
            {
                return Song.Extend.LoginState.Accounts.CurrentUser; 
            }
        }
    }
}
