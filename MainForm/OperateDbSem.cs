using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace MainForm
{
    public class OperateDbSem
    {
        public static string Conn = "Database='db_sem';Data Source='localhost';User Id='root';Password='sinosimu';charset='utf8';pooling=true";
        //public static string Conn = "Database='db_sem';Data Source='localhost';User Id='root';Password='123456';charset='utf8';pooling=true";

        public OperateDbSem(string password)
        {
            Conn = string.Format("Database='db_sem';Data Source='localhost';User Id='root';Password='{0}';charset='utf8';pooling=true", password);
        }

        public static List<string> GetModelNames()
        {
            List<string> strList = new List<string>();
            string sql = "SELECT name FROM tb_group;";
            var reader = MySqlHelper.ExecuteReader(Conn, CommandType.Text, sql, null);
            while (reader.HasRows)
            {
                if (reader.Read())
                {
                    strList.Add(reader[0].ToString());
                }
                else
                    break;
            }
            return strList;
        }

        public static List<string> GetUserIds()
        {
            List<string> strList = new List<string>();
            string sql = "SELECT uid FROM tb_user;";
            var reader = MySqlHelper.ExecuteReader(Conn, CommandType.Text, sql, null);
            while (reader.HasRows)
            {
                if (reader.Read())
                {
                    strList.Add(reader[0].ToString());
                }
                else
                    break;
            }
            return strList;
        }

        public static List<string> InsertUsersToSql(List<UserConfigClass> users)
        {
            List<string> errs = new List<string>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Conn))
                {
                    conn.Open();
                    MySqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        List<string> sqls = getSqls(users);
                        foreach (var sql in sqls)
                        {
                            int count = MySqlHelper.ExecuteNonQuery(transaction, CommandType.Text, sql, null);
                            errs.Add("插入语句后，影响行数：" + count);
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch
                        {
                            throw;
                        }
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errs;
        }

        private static List<string> getSqls(List<UserConfigClass> users)
        {
            List<string> sqls = new List<string>();
            int maxUserId = getMaxId("tb_user") + 1;
            string sqlUser = "INSERT INTO `tb_user` VALUES ";
            string sqlStudent = "INSERT INTO `tb_student` VALUES ";
            foreach (var usr in users)
            {
                sqlUser += string.Format("({0},'{1}','','STUDENT',NULL,1,NULL,NULL,NULL,'OFF'),",
                                                            maxUserId, usr.loginId);
                sqlStudent += string.Format("({0},'{1}','{2}',20,'MALE','-','',1,'-',0),",
                                                            maxUserId, usr.loginId, usr.name);
                maxUserId++;
            }
            sqlUser = sqlUser.Remove(sqlUser.Length - 1) + ";";
            sqlStudent = sqlStudent.Remove(sqlStudent.Length - 1) + ";";
            sqls.Add(sqlUser);
            sqls.Add(sqlStudent);
            return sqls;
        }

        public static List<string> InsertToSql(UserConfigClass uc)
        {
            List<string> errs = new List<string>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Conn))
                {
                    conn.Open();
                    MySqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        errs.AddRange(insertToUser(transaction, ref uc));
                        errs.AddRange(insertToStudent(transaction, ref uc));
                        errs.AddRange(insertToGroupInfo(transaction, ref uc));
                        transaction.Commit();

                        //MySqlCommand cmd = conn.CreateCommand();
                        //cmd.Transaction = transaction;
                        //cmd.CommandText = "INSERT INTO tbl_phonenumber VALUES('1','1','2','3')";
                        //int x = cmd.ExecuteNonQuery();
                        //cmd.CommandText = "INSERT INTO tbl_phonenumber VALUES('1','1','2','4','5')";
                        //int y = cmd.ExecuteNonQuery();
                        //transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch
                        {
                            throw;
                        }
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errs;
        }

        private static List<string> insertToUser(MySqlTransaction tran, ref UserConfigClass uc)
        {
            List<string> errs = new List<string>();
            int maxUserId = getMaxId("tb_user");
            if (maxUserId > 0)
            {
                string sql = string.Format("INSERT INTO `tb_user` VALUES ({0},'{1}','','STUDENT',NULL,1,NULL,NULL,NULL,'OFF');",
                                                            maxUserId + 1, uc.loginId);
                int count = MySqlHelper.ExecuteNonQuery(tran, CommandType.Text, sql, null);
                errs.Add("User表插入语句后，影响行数：" + count);
            }
            else
                throw new Exception("User表查询最大ID出现错误！");
            return errs;
        }

        private static List<string> insertToStudent(MySqlTransaction tran, ref UserConfigClass uc)
        {
            List<string> errs = new List<string>();
            int maxId = getMaxId("tb_student");
            uc.sid = maxId + 1;
            if (maxId > 0)
            {
                string sql = string.Format("INSERT INTO `tb_student` VALUES ({0},'{1}','{2}',20,'MALE','-','',1,'-',0);",
                                                            uc.sid, uc.loginId, uc.name);
                int count = MySqlHelper.ExecuteNonQuery(tran, CommandType.Text, sql, null);
                errs.Add("student表插入语句后，影响行数：" + count);
            }
            else
                throw new Exception("student表查询最大ID出现错误！");
            return errs;
        }

        private static List<string> insertToGroupInfo(MySqlTransaction tran, ref UserConfigClass uc)
        {
            List<string> errs = new List<string>();
            int groupId = getGroupIdByName(uc.modelName);
            if (string.IsNullOrEmpty(uc.modelName) || groupId < 0)
            {
                errs.Add(uc.name + "未配置分组");
                return errs;
                //throw new Exception("获取分组ID出现错误！");
            }
            int maxId = getMaxId("tb_groupinfo");
            if (maxId > 0)
            {
                string sql = string.Format("INSERT INTO `tb_groupinfo` VALUES ({0},'{1}','{2}','');",
                                                            maxId + 1, groupId, uc.sid);
                int count = MySqlHelper.ExecuteNonQuery(tran, CommandType.Text, sql, null);
                errs.Add("student表插入语句后，影响行数：" + count);
            }
            else
                throw new Exception("插入groupinfo出现错误！");
            return errs;
        }

        private static int getMaxId(string tableName)
        {
            string sql = "SELECT MAX(id) FROM " + tableName + ";";
            var reader = MySqlHelper.ExecuteReader(Conn, CommandType.Text, sql, null);
            while (reader.HasRows)
            {
                if (reader.Read())
                {
                    return int.Parse(reader[0].ToString());
                }
                else
                    break;
            }
            return -1;
        }

        private static int getGroupIdByName(string groupName)
        {
            string sql = string.Format("SELECT id FROM tb_group WHERE name='{0}';", groupName);
            var reader = MySqlHelper.ExecuteReader(Conn, CommandType.Text, sql, null);
            while (reader.HasRows)
            {
                if (reader.Read())
                {
                    return int.Parse(reader[0].ToString());
                }
                else
                    break;
            }
            return -1;
        }

        public static int GetStudentIdByName(string name)
        {
            string sql = string.Format("SELECT id FROM tb_student WHERE name='{0}';", name);
            var reader = MySqlHelper.ExecuteReader(Conn, CommandType.Text, sql, null);
            while (reader.HasRows)
            {
                if (reader.Read())
                {
                    return int.Parse(reader[0].ToString());
                }
                else
                    break;
            }
            return -1;
        }

        public static int GetUserIdByUid(string uid)
        {
            string sql = string.Format("SELECT id FROM tb_user WHERE uid='{0}';", uid);
            var reader = MySqlHelper.ExecuteReader(Conn, CommandType.Text, sql, null);
            while (reader.HasRows)
            {
                if (reader.Read())
                {
                    return int.Parse(reader[0].ToString());
                }
                else
                    break;
            }
            return -1;
        }
    }
}