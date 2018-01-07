using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MainForm
{
    public class Params
    {
        public GroupClass Group { get; set; }
        public GroupInfoClass GroupInfo { get; set; }
        public List<LocalProgramClass> LocalProgramList { get; set; }
        public List<LocalRunClass> LocalRunList { get; set; }
        public StudentClass Student { get; set; }
        public UserClass User { get; set; }
    }

    public class GroupClass
    {
        public string id { get; set; }

        [XmlElement(ElementName = "分组名称")]
        public string name { get; set; }

        [XmlElement(ElementName = "分组内容")]
        public string content { get; set; }

        [XmlElement(ElementName = "模型名称")]
        public string model { get; set; }

        [XmlElement(ElementName = "驱动器")]
        public string drive { get; set; }

        public string starid { get; set; }
        public string staruser { get; set; }
        public string starpassword { get; set; }

        [XmlElement(ElementName = "IP地址")]
        public string ip { get; set; }

        public string port { get; set; }
        public string remark { get; set; }
        public string link { get; set; }

        [XmlElement(ElementName = "条件号")]
        public string stateno { get; set; }

        [XmlElement(ElementName = "学员个数")]
        public string studentCount { get; set; }

        [XmlElement(ElementName = "组个数")]
        public string groupCount { get; set; }

        [XmlElement(ElementName = "分组情况")]
        public string groupInfo { get; set; }

        [XmlElement(ElementName = "一机多模中本模型序号")]
        public string mdlIndex { get; set; }

        public GroupClass()
        {
        }

        public GroupClass(string sqlLine)
        {
            string line = sqlLine.Remove(0, sqlLine.IndexOf("(") + 1);
            line = line.Remove(line.LastIndexOf(")"));
            string[] strList = line.Split(',');
            this.id = strList[0];
            this.name = strList[1];
            this.content = strList[2];
            this.model = strList[3];
            this.drive = strList[4];
            this.starid = strList[5];
            this.staruser = strList[6];
            this.starpassword = strList[7];
            this.ip = strList[8];
            this.port = strList[9];
            this.remark = strList[10];
            this.link = strList[11];
            this.stateno = strList[12];
        }

        public string GetText()
        {
            return string.Format("INSERT INTO `tb_group` VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12});",
                this.id, this.name, this.content, this.model, this.drive, this.starid, this.staruser, this.starpassword, this.ip, this.port,
                this.remark, this.link, this.stateno);
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

    public class GroupInfoClass
    {
        public string id { get; set; }
        public string fk_gid { get; set; }
        public string fk_sid { get; set; }
        public string remark { get; set; }

        public GroupInfoClass()
        { }

        public GroupInfoClass(string sqlLine)
        {
            string line = sqlLine.Remove(0, sqlLine.IndexOf("(") + 1);
            line = line.Remove(line.LastIndexOf(")"));
            string[] strList = line.Split(',');
            this.id = strList[0];
            this.fk_gid = strList[1];
            this.fk_sid = strList[2];
            this.remark = strList[3];
        }

        public string GetText()
        {
            return string.Format("INSERT INTO `tb_groupinfo` VALUES ({0},{1},{2},{3});",
                this.id, this.fk_gid, this.fk_sid, this.remark);
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

    public class LocalProgramClass
    {
        public string id { get; set; }
        public string fk_lrid { get; set; }
        public string program { get; set; }
        public string path { get; set; }
        public string parameter { get; set; }
        public string monnetpc { get; set; }
        public string port { get; set; }
        public string maxpva { get; set; }
        public string maxpvd { get; set; }
        public string netdisp { get; set; }
        public string sysname { get; set; }

        public LocalProgramClass()
        { }

        public LocalProgramClass(string sqlLine)
        {
            string line = sqlLine.Remove(0, sqlLine.IndexOf("(") + 1);
            line = line.Remove(line.LastIndexOf(")"));
            string[] strList = line.Split(',');
            this.id = strList[0];
            this.fk_lrid = strList[1];
            this.program = strList[2];
            this.path = strList[3];
            this.parameter = strList[4];
            this.monnetpc = strList[5];
            this.port = strList[6];
            this.maxpva = strList[7];
            this.maxpvd = strList[8];
            this.netdisp = strList[9];
            this.sysname = strList[10];
        }

        public string GetText()
        {
            return string.Format("INSERT INTO `tb_localprogram` VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10});",
                this.id, this.fk_lrid, this.program, this.path, this.parameter, this.monnetpc, this.port, this.maxpva, this.maxpvd,
                this.netdisp, this.sysname);
        }
    }

    public class LocalRunClass
    {
        public string id { get; set; }
        public string fk_gid { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public LocalRunClass()
        { }

        public LocalRunClass(string sqlLine)
        {
            string line = sqlLine.Remove(0, sqlLine.IndexOf("(") + 1);
            line = line.Remove(line.LastIndexOf(")"));
            string[] strList = line.Split(',');
            this.id = strList[0];
            this.fk_gid = strList[1];
            this.name = strList[2];
            this.description = strList[3];
        }

        public string GetText()
        {
            return string.Format("INSERT INTO `tb_localrun` VALUES ({0},{1},{2},{3});",
                this.id, this.fk_gid, this.name, this.description);
        }
    }

    public class StudentClass
    {
        public string id { get; set; }

        [XmlElement(ElementName = "用户名")]
        public string fk_uid { get; set; }

        [XmlElement(ElementName = "学员名")]
        public string name { get; set; }

        [XmlElement(ElementName = "年龄")]
        public string age { get; set; }

        [XmlElement(ElementName = "性别")]
        public string gender { get; set; }

        public string company { get; set; }
        public string idcard { get; set; }
        public string fk_cid { get; set; }
        public string notes { get; set; }
        public string grouped { get; set; }

        public StudentClass()
        { }

        public StudentClass(string sqlLine)
        {
            string line = sqlLine.Remove(0, sqlLine.IndexOf("(") + 1);
            line = line.Remove(line.LastIndexOf(")"));
            string[] strList = line.Split(',');
            this.id = strList[0];
            this.fk_uid = strList[1];
            this.name = strList[2];
            this.age = strList[3];
            this.gender = strList[4];
            this.company = strList[5];
            this.idcard = strList[6];
            this.fk_cid = strList[7];
            this.notes = strList[8];
            this.grouped = strList[9];
        }

        public string GetText()
        {
            return string.Format("INSERT INTO `tb_student` VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9});",
                this.id, this.fk_uid, this.name, this.age, this.gender, this.company, this.idcard, this.fk_cid, this.notes, this.grouped);
        }
    }

    public class UserClass
    {
        public string id { get; set; }
        public string uid { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string email { get; set; }
        public string status { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string remark { get; set; }
        public string online { get; set; }

        public UserClass()
        { }

        public UserClass(string sqlLine)
        {
            string line = sqlLine.Remove(0, sqlLine.IndexOf("(") + 1);
            line = line.Remove(line.LastIndexOf(")"));
            string[] strList = line.Split(',');
            this.id = strList[0];
            this.uid = strList[1];
            this.password = strList[2];
            this.role = strList[3];
            this.email = strList[4];
            this.status = strList[5];
            this.question = strList[6];
            this.answer = strList[7];
            this.remark = strList[8];
            this.online = strList[9];
        }

        public string GetText()
        {
            return string.Format("INSERT INTO `tb_user` VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9});",
                this.id, this.uid, this.password, this.role, this.email, this.status, this.question, this.answer, this.remark, this.online);
        }
    }

    public class UserConfigClass
    {
        public string loginId { get; set; }
        public string name { get; set; }
        public string modelName { get; set; }
        public int sid { get; set; }

        public UserConfigClass()
        { }
    }

    public class DcsClass
    {
        [XmlElement(ElementName = "类型")]
        public string type { get; set; }

        [XmlElement(ElementName = "描述")]
        public string desc { get; set; }

        [XmlElement(ElementName = "执行程序")]
        public string program { get; set; }

        [XmlElement(ElementName = "执行程序所在路径")]
        public string pathPro { get; set; }

        [XmlElement(ElementName = "monetpc路径")]
        public string pathMonetpc { get; set; }

        [XmlElement(ElementName = "端口号")]
        public string port { get; set; }

        [XmlElement(ElementName = "pva最大值")]
        public string maxPva { get; set; }

        [XmlElement(ElementName = "pvd最大值")]
        public string maxPvd { get; set; }

        [XmlElement(ElementName = "系统名称")]
        public string systemName { get; set; }

        [XmlElement(ElementName = "运行参数")]
        public string runPara { get; set; }
    }
}