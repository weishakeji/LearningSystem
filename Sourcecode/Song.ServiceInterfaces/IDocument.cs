using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;
using WeiSha.Data;
using NPOI.SS.UserModel;
using Newtonsoft.Json.Linq;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 文档处理，例如Excel、Word、PDF的解析、输出、转换
    /// </summary>
    public interface IDocument : WeiSha.Core.IBusinessInterface
    {
        //#region Excel的处理
        ///// <summary>
        ///// Excel处理对象
        ///// </summary>
        ///// <param name="file"></param>
        ///// <returns></returns>
        //IWorkbook ExcelWorker(string file);
        ///// <summary>
        ///// Excel的工作表
        ///// </summary>
        ///// <param name="xlsFile">Excel的完整路径名</param>
        ///// <returns>工作表名与记录数：[{name:'',count:11}]</returns>
        //JArray ExcelSheets(string xlsFile);
        ///// <summary>
        ///// Excel的工作表
        ///// </summary>
        ///// <param name="workbook">Excel的操作对象</param>
        ///// <returns>工作表名与记录数：[{name:'',count:11}]</returns>
        //JArray ExcelSheets(IWorkbook workbook);
        ///// <summary>
        ///// Excel的列,工作表的第一行
        ///// <param name="xlsFile"> Excel的完整路径名</param>
        ///// <param name="sheetIndex">工作表索引</param>
        ///// </summary>
        //JObject ExcelColumns(string xlsFile, int sheetIndex);
        ///// <summary>
        ///// Excel的列,工作表的第一行
        ///// </summary>
        ///// <param name="workbook">Excel的操作对象</param>
        ///// <param name="sheetName">工作表的名称</param> 
        //JObject ExcelColumns(IWorkbook workbook, string sheetName);
        ///// <summary>
        ///// Excel的列,工作表的第一行
        ///// </summary>
        ///// <param name="workbook">Excel的操作对象</param>
        ///// <param name="sheetIndex">工作表索引</param> 
        //JObject ExcelColumns(IWorkbook workbook, int sheetIndex);

        //#endregion
    }
}
