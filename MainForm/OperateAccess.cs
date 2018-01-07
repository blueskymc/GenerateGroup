using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace MainForm
{
    public class OperateAccess
    {
        #region 数据库操作
        public OleDbDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, List<OleDbParameter> commandParameters)
        {
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                    OleDbDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    return rdr;
                }
            }
            catch
            {
                throw;
            }
        }
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, List<OleDbParameter> commandParameters)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }
        public static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, CommandType cmdType, string cmdText, List<OleDbParameter> cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (OleDbParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
        #endregion
    }
}
