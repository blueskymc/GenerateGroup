using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MainForm
{
    public class Tables
    {
        public DataTable dtGroup { get; set; }
        public DataTable dtGroupInfo { get; set; }
        public DataTable dtLocalProgram { get; set; }
        public DataTable dtLocalRun { get; set; }
        public DataTable dtStudent { get; set; }
        public DataTable dtUser { get; set; }
        public DataTable dtUserConfig { get; set; }

        public DataTable dtLocalSetting { get; set; }

        public Tables()
        {
            dtGroup = new DataTable();
            dtGroupInfo = new DataTable();
            dtLocalProgram = new DataTable();
            dtLocalRun = new DataTable();
            dtStudent = new DataTable();
            dtUser = new DataTable();
            dtUserConfig = new DataTable();
            dtLocalSetting = new DataTable();

            DataColumn column;
            foreach (var info in typeof(GroupClass).GetProperties())
            {
                column = new DataColumn(info.Name);
                dtGroup.Columns.Add(column);
            }
            foreach (var info in typeof(GroupInfoClass).GetProperties())
            {
                column = new DataColumn(info.Name);
                dtGroupInfo.Columns.Add(column);
            }
            foreach (var info in typeof(LocalProgramClass).GetProperties())
            {
                column = new DataColumn(info.Name);
                dtLocalProgram.Columns.Add(column);
            }
            foreach (var info in typeof(LocalRunClass).GetProperties())
            {
                column = new DataColumn(info.Name);
                dtLocalRun.Columns.Add(column);
            }
            foreach (var info in typeof(StudentClass).GetProperties())
            {
                column = new DataColumn(info.Name);
                dtStudent.Columns.Add(column);
            }
            foreach (var info in typeof(UserClass).GetProperties())
            {
                column = new DataColumn(info.Name);
                dtUser.Columns.Add(column);
            }
            foreach (var info in typeof(UserConfigClass).GetProperties())
            {
                column = new DataColumn(info.Name);
                dtUserConfig.Columns.Add(column);
            }

            column = new DataColumn("类型");
            dtLocalSetting.Columns.Add(column);
            column = new DataColumn("描述");
            dtLocalSetting.Columns.Add(column);
            column = new DataColumn("执行程序");
            dtLocalSetting.Columns.Add(column);
            column = new DataColumn("执行程序所在路径");
            dtLocalSetting.Columns.Add(column);
            column = new DataColumn("monnetpc路径");
            dtLocalSetting.Columns.Add(column);
            column = new DataColumn("端口号");
            dtLocalSetting.Columns.Add(column);
            column = new DataColumn("pva最大值");
            dtLocalSetting.Columns.Add(column);
            column = new DataColumn("pvd最大值");
            dtLocalSetting.Columns.Add(column);
            column = new DataColumn("系统名称");
            dtLocalSetting.Columns.Add(column);
            column = new DataColumn("运行参数");
            dtLocalSetting.Columns.Add(column);
        }

        public void AddGroup(string sqlLine)
        {
            string line = sqlLine.Remove(0, sqlLine.IndexOf("(") + 1);
            line = line.Remove(line.LastIndexOf(")"));
            string[] strList = line.Split(',');
            DataRow dr = dtGroup.NewRow();
            for (int i = 0; i < strList.Count(); i++)
            {
                dr[i] = deleteUnuse(strList[i]);
            }
            dtGroup.Rows.Add(dr);
        }

        public void AddGroupInfo(string sqlLine)
        {
            string line = sqlLine.Remove(0, sqlLine.IndexOf("(") + 1);
            line = line.Remove(line.LastIndexOf(")"));
            string[] strList = line.Split(',');
            DataRow dr = dtGroupInfo.NewRow();
            for (int i = 0; i < strList.Count(); i++)
            {
                dr[i] = deleteUnuse(strList[i]);
            }
            dtGroupInfo.Rows.Add(dr);
        }

        public void AddLocalProgram(string sqlLine)
        {
            string line = sqlLine.Remove(0, sqlLine.IndexOf("(") + 1);
            line = line.Remove(line.LastIndexOf(")"));
            string[] strList = line.Split(',');
            DataRow dr = dtLocalProgram.NewRow();
            for (int i = 0; i < strList.Count(); i++)
            {
                dr[i] = deleteUnuse(strList[i]);
            }
            dtLocalProgram.Rows.Add(dr);
        }

        public void AddLocalRun(string sqlLine)
        {
            string line = sqlLine.Remove(0, sqlLine.IndexOf("(") + 1);
            line = line.Remove(line.LastIndexOf(")"));
            string[] strList = line.Split(',');
            DataRow dr = dtLocalRun.NewRow();
            for (int i = 0; i < strList.Count(); i++)
            {
                dr[i] = deleteUnuse(strList[i]);
            }
            dtLocalRun.Rows.Add(dr);
        }

        public void AddStudent(string sqlLine)
        {
            string line = sqlLine.Remove(0, sqlLine.IndexOf("(") + 1);
            line = line.Remove(line.LastIndexOf(")"));
            string[] strList = line.Split(',');
            DataRow dr = dtStudent.NewRow();
            for (int i = 0; i < strList.Count(); i++)
            {
                dr[i] = deleteUnuse(strList[i]);
            }
            dtStudent.Rows.Add(dr);
        }

        public void AddUser(string sqlLine)
        {
            string line = sqlLine.Remove(0, sqlLine.IndexOf("(") + 1);
            line = line.Remove(line.LastIndexOf(")"));
            string[] strList = line.Split(',');
            DataRow dr = dtUser.NewRow();
            for (int i = 0; i < strList.Count(); i++)
            {
                dr[i] = deleteUnuse(strList[i]);
            }
            dtUser.Rows.Add(dr);
        }

        public void AddUserConfig(UserConfigClass uc)
        {
            DataRow dr = dtUserConfig.NewRow();
            dr[0] = uc.loginId;
            dr[1] = uc.name;
            dtUserConfig.Rows.Add(dr);
        }

        public void AddLocalSetting(string sqlLine)
        {
            string line = sqlLine.Remove(0, sqlLine.IndexOf("(") + 1);
            line = line.Remove(line.LastIndexOf(")"));
            string[] strList = line.Split(',');
            DataRow dr = dtLocalSetting.NewRow();
            for (int i = 0; i < strList.Count(); i++)
            {
                dr[i] = deleteUnuse(strList[i]);
            }
            dtLocalSetting.Rows.Add(dr);
            //addInfo(sqlLine, ref dtLocalSetting);
        }

        private void addInfo(string sqlLine, ref DataTable dt)
        {
            string line = sqlLine.Remove(0, sqlLine.IndexOf("(") + 1);
            line = line.Remove(line.LastIndexOf(")"));
            string[] strList = line.Split(',');
            DataRow dr = dt.NewRow();
            for (int i = 0; i < strList.Count(); i++)
            {
                dr[i] = deleteUnuse(strList[i]);
            }
            dt.Rows.Add(dr);
        }

        private string deleteUnuse(string str)
        {
            string value = str;
            while (value.Contains('\''))
            {
                value = value.Replace('\'', ' ');
            }
            return value.Trim();
        }
    }
}