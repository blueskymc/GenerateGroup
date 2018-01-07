using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace MainForm
{
    public class DBExcelUser
    {
        public DBExcelUser(string sqlConn)
        {
            connectionString = sqlConn;
        }
        private string connectionString;

        #region 修改施耐德映射表

        public List<UserModel> GetAllData()
        {
            try
            {
                List<UserModel> modelList = new List<UserModel>();
                modelList.AddRange(GetAllData("[Sheet1$]"));
                //modelList.AddRange(GetAllData("2空压机"));
                //modelList.AddRange(GetAllData("2循环泵"));
                return modelList;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ;
            }
        }

        public List<UserModel> GetAllData(string tableName)
        {
            List<UserModel> modelList = new List<UserModel>();
            string sqlString = "SELECT 工号,电厂,名字,值次,机组 FROM " + tableName;
            OleDbCommand cmd = new OleDbCommand();
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                OperateAccess.PrepareCommand(cmd, conn, null, CommandType.Text, sqlString, null);
                OleDbDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                while (rdr.Read())
                {
                    UserModel model = new UserModel();
                    //model.Index = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
                    model.工号 = rdr.IsDBNull(0) ? "" : rdr.GetString(0).Trim();
                    model.电厂 = rdr.IsDBNull(1) ? "" : rdr.GetString(1).Trim();
                    model.名字 = rdr.IsDBNull(2) ? "" : rdr.GetString(2).Trim();
                    string zhici = rdr.IsDBNull(3) ? "" : rdr.GetString(3).Trim();
                    model.机组 = rdr.IsDBNull(4) ? "" : rdr.GetString(4).Trim();
                    if (string.IsNullOrEmpty(zhici) || zhici == "0")
                        model.值次 = "7";
                    else
                    {
                        int z = 1;
                        if (int.TryParse(zhici, out z))
                        {
                            model.值次 = (z + 1).ToString();
                        }
                        else
                        {
                            model.值次 = "2";
                        }
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }
        #endregion
    }
    public class UserModel
    {
        public UserModel()
        {
        }
        public string 工号 { get; set; }
        public string 电厂 { get; set; }
        public string 名字 { get; set; }
        public string 值次 { get; set; }
        public string 机组 { get; set; }

    }
}
