// <copyright file="CsvOperation.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>muxf</author>
// <date> 2013-11-28 14:10:02 </date>
// <summary> Excel,Cev操作类 </summary>
// <modify>
//      修改人：muxf
//      修改时间：2013-11-28 14:09:56
//      修改描述：新建 CsvOperation.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows;
using Style = Aspose.Cells.Style;

namespace Infrastructure.Utils
{
    /// <summary>
    /// Excel,CSV操作
    /// </summary>
    public class CsvOperation
    {
        /// <summary>
        /// 从Csv获取数据
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="filePath">文件完整路径</param>
        /// <returns>对象列表</returns>
        public List<T> GetObjectList<T>(string filePath) where T : new()
        {
            List<T> list = new List<T>();
            if (!filePath.Trim().EndsWith("csv") && !filePath.Trim().EndsWith("xlsx"))
            {
                return list;
            }

            Type type = typeof(T);
            Workbook workbook = new Workbook(filePath);
            Worksheet sheet = workbook.Worksheets[0];

            // 获取标题列表
            var titleDic = this.GetTitleDic(sheet);

            // 循环每行数据
            for (int i = 1; i < int.MaxValue; i++)
            {
                // 行为空时结束
                if (string.IsNullOrEmpty(sheet.Cells[i, 0].StringValue))
                {
                    break;
                }

                T instance = new T();

                // 循环赋值每个属性
                foreach (var item in type.GetProperties())
                {
                    if (titleDic.ContainsKey(item.Name))
                    {
                        string str = sheet.Cells[i, titleDic[item.Name]].StringValue;
                        if (!string.IsNullOrEmpty(str))
                        {
                            try
                            {
                                // 根据类型进行转换赋值
                                if (item.PropertyType == typeof(string))
                                {
                                    item.SetValue(instance, str, null);
                                }
                                else if (item.PropertyType.IsEnum)
                                {
                                    item.SetValue(instance, int.Parse(str), null);
                                }
                                else
                                {
                                    MethodInfo method = item.PropertyType.GetMethod("Parse", new Type[] { typeof(string) });
                                    object obj = null;
                                    if (method != null)
                                    {
                                        obj = method.Invoke(null, new object[] { str });
                                        item.SetValue(instance, obj, null);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                // 获取错误
                            }
                        }
                    }
                }

                list.Add(instance);
            }

            return list;
        }

        /// <summary>
        /// 根据DataTable内容保存文件
        /// </summary>
        /// <param name="table">DataTable内容</param>
        /// <param name="filePath">待保存文件路径</param>
        public void SetCsvListByDataTable(DataTable table, string filePath)
        {
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];

            // 获取保存文件的后缀
            string aLastName = filePath.Substring(filePath.LastIndexOf(".") + 1, (filePath.Length - filePath.LastIndexOf(".") - 1)); 

            // 列头样式
            Style headerStyle = workbook.Styles[workbook.Styles.Add()];
            headerStyle.Font.Size = 6;
            headerStyle.IsTextWrapped = true;
            headerStyle.HorizontalAlignment = TextAlignmentType.Center;
            headerStyle.VerticalAlignment = TextAlignmentType.Center;

            headerStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            headerStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            headerStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            headerStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            headerStyle.Borders.SetColor(System.Drawing.Color.Black);

            // 行样式
            Style rowStyle = workbook.Styles[workbook.Styles.Add()];
            rowStyle.Font.Size = 6;
            rowStyle.IsTextWrapped = true;
            rowStyle.HorizontalAlignment = TextAlignmentType.Center;
            rowStyle.VerticalAlignment = TextAlignmentType.Center;

            rowStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            rowStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            rowStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            rowStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            rowStyle.Borders.SetColor(System.Drawing.Color.Black);

            // 冻结第一行
            sheet.FreezePanes(1, 1, 1, 0);

            sheet.Cells.SetRowHeight(0, 20);
            // 保存第一行列头
            for (int i = 0; i < table.Columns.Count; i++)
            {                
                sheet.Cells[0, i].PutValue(table.Columns[i].ColumnName);
                sheet.Cells[0, i].SetStyle(headerStyle);
            }

            // 保存各个行内容
            for (int j = 0; j < table.Rows.Count; j++)
            {
                for (int k = 0; k < table.Rows[j].ItemArray.Length; k++)
                {
                    sheet.Cells.SetRowHeight(j + 1, 18);
                    sheet.Cells[j + 1, k].PutValue(table.Rows[j].ItemArray[k].ToString());
                    sheet.Cells[j + 1, k].SetStyle(rowStyle);
                }
            }

            if (aLastName == "pdf" || aLastName == "PDF")
            {
                PdfSaveOptions options = new PdfSaveOptions(SaveFormat.Pdf);

                options.AllColumnsInOnePagePerSheet = true;
                options.Compliance = Aspose.Cells.Rendering.PdfCompliance.PdfA1b;
                options.PrintingPageType = PrintingPageType.Default;

                // 保存文件
                workbook.Save(filePath, options);
            }
            else
            {
                // 保存文件
                workbook.Save(filePath);
            }
        }

        /// <summary>
        /// 根据DataTable内容保存文件
        /// </summary>
        /// <param name="table">DataTable内容</param>
        /// <param name="filePath">待保存文件路径</param>
        /// <param name="title">文件名称</param>
        /// <param name="createDate">报表生成时间</param>
        /// <param name="queryTerms">查询条件</param>
        public void SaveByDataTableAll(DataTable table, string filePath, string title, string createDate, List<QueryTermsKvp> queryTerms)
        {
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];

            // 获取保存文件的后缀
            string aLastName = filePath.Substring(filePath.LastIndexOf(".") + 1, (filePath.Length - filePath.LastIndexOf(".") - 1));

            // 标题样式
            Style titleStyle = workbook.Styles[workbook.Styles.Add()];
            titleStyle.HorizontalAlignment = TextAlignmentType.Center;
            titleStyle.VerticalAlignment = TextAlignmentType.Center;
            titleStyle.Font.Size = 14;
            titleStyle.Font.IsBold = true;

            // 报表生成日期样式
            Style dateStyle = workbook.Styles[workbook.Styles.Add()];
            dateStyle.HorizontalAlignment = TextAlignmentType.Right;
            dateStyle.VerticalAlignment = TextAlignmentType.Center;
            dateStyle.Font.Size = 6;

            // 查询条件名称样式
            Style queryNameStyle = workbook.Styles[workbook.Styles.Add()];
            queryNameStyle.HorizontalAlignment = TextAlignmentType.Right;
            queryNameStyle.VerticalAlignment = TextAlignmentType.Center;
            queryNameStyle.Font.Size = 8;
            queryNameStyle.Font.IsBold = true;
            //queryNameStyle.IsTextWrapped = true;

            // 查询条件内容样式
            Style queryDataStyle = workbook.Styles[workbook.Styles.Add()];
            queryDataStyle.HorizontalAlignment = TextAlignmentType.Left;
            queryDataStyle.VerticalAlignment = TextAlignmentType.Center;
            queryDataStyle.Font.Size = 6;
            queryDataStyle.IsTextWrapped = true;

            // 列头样式
            Style headerStyle = workbook.Styles[workbook.Styles.Add()];
            headerStyle.Font.Size = 8;
            headerStyle.Font.IsBold = true;
            headerStyle.IsTextWrapped = true;
            headerStyle.HorizontalAlignment = TextAlignmentType.Center;
            headerStyle.VerticalAlignment = TextAlignmentType.Center;
            headerStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            headerStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            headerStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            headerStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            headerStyle.Borders.SetColor(System.Drawing.Color.Black);

            // 行样式
            Style rowStyle = workbook.Styles[workbook.Styles.Add()];
            rowStyle.Font.Size = 6;
            rowStyle.IsTextWrapped = true;
            rowStyle.HorizontalAlignment = TextAlignmentType.Center;
            rowStyle.VerticalAlignment = TextAlignmentType.Center;
            rowStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            rowStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            rowStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            rowStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            rowStyle.Borders.SetColor(System.Drawing.Color.Black);

            // 表格列数
            int colunm = table.Columns.Count;
            if (colunm < 12)
            {
                // 最小列数 11列
                colunm = 12;
            }

            // 表格行数
            int rownum = table.Rows.Count;

            // 数据动态定位行
            int rowPoint = 0;

            // 标题
            sheet.Cells.Merge(0, 0, 1, colunm);
            sheet.Cells[0, 0].PutValue(title);
            sheet.Cells[0, 0].SetStyle(titleStyle);
            sheet.Cells.SetRowHeight(0, 30);
            rowPoint++;

            // 报表生产日期
            string reportDate = "ReportDate : " + createDate;
            sheet.Cells.Merge(1, 0, 1, colunm);
            sheet.Cells[1, 0].PutValue(reportDate);
            sheet.Cells[1, 0].SetStyle(dateStyle);
            sheet.Cells.SetRowHeight(1, 15);
            rowPoint++;

            // 空行
            sheet.Cells.SetRowHeight(rowPoint, 10);
            rowPoint++;

            // 查询条件
            if (queryTerms != null || queryTerms.Count > 0)
            {
                int termsNum = queryTerms.Count;

                // 查询条件行数
                int queryRownum = queryTerms.Count / 3;
                if (queryTerms.Count % 3 > 0)
                {
                    queryRownum++;
                }

                for (int r = 0; r < queryRownum; r++)
                {
                    // 此行最多放三条查询条件
                    int rowLimit = (r + 1) * 3;
                    if (rowLimit >= termsNum)
                    {
                        rowLimit = termsNum;
                    }

                    for (int c = r * 3; c < rowLimit; c++)
                    {
                        // 构造查询条件单元格
                        sheet.Cells[rowPoint, ((c % 3) * 4) + 1].PutValue(queryTerms[c].Key + " : ");
                        sheet.Cells[rowPoint, ((c % 3) * 4) + 1].SetStyle(queryNameStyle);

                        sheet.Cells.Merge(rowPoint, ((c % 3) * 4) + 2, 1, 2);
                        sheet.Cells[rowPoint, ((c % 3) * 4) + 2].PutValue(queryTerms[c].Value);
                        sheet.Cells[rowPoint, ((c % 3) * 4) + 2].SetStyle(queryDataStyle);

                        sheet.Cells.SetRowHeight(rowPoint, 25);
                    }

                    rowPoint++;
                }

                // 空行
                sheet.Cells.SetRowHeight(rowPoint, 10);
                rowPoint++;
            }

            // 保存第一行列头
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sheet.Cells[rowPoint, i].PutValue(table.Columns[i].ColumnName);
                sheet.Cells[rowPoint, i].SetStyle(headerStyle);
                sheet.Cells.SetRowHeight(rowPoint, 25);
            }
            rowPoint++;

            // 保存各个行内容
            for (int j = 0; j < rownum; j++)
            {
                for (int k = 0; k < table.Rows[j].ItemArray.Length; k++)
                {
                    sheet.Cells.SetRowHeight(j + rowPoint, 18);
                    sheet.Cells[j + rowPoint, k].PutValue(table.Rows[j].ItemArray[k].ToString());
                    sheet.Cells[j + rowPoint, k].SetStyle(rowStyle);
                }
            }

            if (aLastName == "pdf" || aLastName == "PDF")
            {
                PdfSaveOptions options = new PdfSaveOptions(SaveFormat.Pdf);

                options.AllColumnsInOnePagePerSheet = true;
                options.Compliance = Aspose.Cells.Rendering.PdfCompliance.PdfA1b;
                options.PrintingPageType = PrintingPageType.Default;

                // 保存文件
                workbook.Save(filePath, options);
            }
            else
            {
                // 保存文件
                workbook.Save(filePath);
            }
        }

        /// <summary>
        /// 获取标题行数据
        /// </summary>
        /// <param name="sheet">sheet</param>
        /// <returns>标题行数据</returns>
        private Dictionary<string, int> GetTitleDic(Worksheet sheet)
        {
            Dictionary<string, int> titList = new Dictionary<string, int>();
            for (int i = 0; i < int.MaxValue; i++)
            {
                if (sheet.Cells[0, i].StringValue == string.Empty)
                {
                    return titList;
                }

                titList.Add(sheet.Cells[0, i].StringValue, i);
            }

            return titList;
        }
    }
}
