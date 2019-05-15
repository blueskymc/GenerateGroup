using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace MainForm
{
    public partial class 生成分组 : Form
    {
        private List<string> textList;
        private List<string> firstList;
        private List<int> stuCountList;
        private Params lastParam;//读取文件，获得的参数
        private int addGroupCount = 10;
        private int addStudentCount = 50;
        private int mdlIndex = 1;
        private string fileName;
        private Encoding encode;
        private Config config;
        private string mdlName;
        private string path;

        private Tables tables;
        private Tables tablesTmp;//监控画面配置表

        #region 后一部分插入数据列表
        private string frontFileName = "";
        private string backFileName = "";
        private List<string> backGroupList;
        private List<string> backGroupInfoList;
        private List<string> backLocalProgramList;
        private List<string> backLocalRunList;
        private List<string> backStudentList;
        private List<string> backUserList;
        #endregion 后一部分插入数据列表

        public 生成分组()
        {
            InitializeComponent();
            setIcons();
            backGroupList = new List<string>();
            backGroupInfoList = new List<string>();
            backLocalProgramList = new List<string>();
            backLocalRunList = new List<string>();
            backStudentList = new List<string>();
            backUserList = new List<string>();
            comboBoxItem1.SelectedIndex = 2;
            encode = Encoding.GetEncoding("gbk");
        }

        private void setIcons()
        {
            //打开SQL
            if (File.Exists("icon\\6.png"))
            {
                buttonItem2.Image = getImageFromFile("icon\\6.png");
            }
            //导出SQL
            if (File.Exists("icon\\7.png"))
            {
                buttonItem3.Image = getImageFromFile("icon\\7.png");
            }
            //保存SQL
            if (File.Exists("icon\\1.png"))
            {
                buttonItem4.Image = getImageFromFile("icon\\1.png");
            }
            //初始化
            if (File.Exists("icon\\2.png"))
            {
                buttonItem5.Image = getImageFromFile("icon\\2.png");
            }
            //打开前一部分
            if (File.Exists("icon\\0.png"))
            {
                buttonItem6.Image = getImageFromFile("icon\\0.png");
            }
            //打开后一部分
            if (File.Exists("icon\\1.png"))
            {
                buttonItem7.Image = getImageFromFile("icon\\1.png");
            }
            //打开多个sql
            if (File.Exists("icon\\1.png"))
            {
                buttonItem14.Image = getImageFromFile("icon\\1.png");
            }
            //生成学员
            if (File.Exists("icon\\4.png"))
            {
                buttonItem17.Image = getImageFromFile("icon\\4.png");
            }
            //查看配置
            if (File.Exists("icon\\5.png"))
            {
                buttonItem16.Image = getImageFromFile("icon\\5.png");
            }
            //生成新文件
            if (File.Exists("icon\\2.png"))
            {
                buttonItem8.Image = getImageFromFile("icon\\2.png");
            }
            //保存配置
            if (File.Exists("icon\\3.png"))
            {
                buttonItem9.Image = getImageFromFile("icon\\3.png");
            }
            //生成目录
            if (File.Exists("icon\\4.png"))
            {
                buttonItem10.Image = getImageFromFile("icon\\4.png");
            }
            //读取配置
            if (File.Exists("icon\\5.png"))
            {
                buttonItem11.Image = getImageFromFile("icon\\5.png");
            }
            if (File.Exists("icon\\5.png"))
            {
                buttonItem12.Image = getImageFromFile("icon\\5.png");
            }
            //生成新data
            if (File.Exists("icon\\5.png"))
            {
                buttonItem15.Image = getImageFromFile("icon\\3.png");
            }
            //连接数据库
            if (File.Exists("icon\\1.png"))
            {
                buttonItem18.Image = getImageFromFile("icon\\1.png");
            }
            //导入文本
            if (File.Exists("icon\\2.png"))
            {
                buttonItem20.Image = getImageFromFile("icon\\2.png");
            }
            //保存
            if (File.Exists("icon\\3.png"))
            {
                buttonItem21.Image = getImageFromFile("icon\\3.png");
            }
            //生成登录账号
            if (File.Exists("icon\\4.png"))
            {
                buttonItem22.Image = getImageFromFile("icon\\4.png");
            }
        }

        private Image getImageFromFile(string filePath)
        {
            Image iconImage = Image.FromFile(filePath);
            Bitmap bm = new Bitmap(iconImage, 60, 60);
            return bm;
        }

        #region 其他私有函数

        /// <summary>
        /// TAB1-打开SQL文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem2_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";
            open.FilterIndex = 1;
            open.RestoreDirectory = true;
            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    fileName = open.FileName;
                    if (readSql(open.FileName))
                        bindData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("TAB1-打开SQL文本-读取SQL出错: " + ex.Message);
                }
            }
        }

        private void buttonItem3_Click(object sender, EventArgs e)
        {
            string newFileName;
            if (fileName.Contains(".sql"))
                newFileName = fileName.Replace(".sql", "_new.sql");
            else if (fileName.Contains(".SQL"))
                newFileName = fileName.Replace(".SQL", "_new.sql");
            else
                newFileName = "c:\\new.sql";
            using (StreamWriter sw = new StreamWriter(newFileName, false, encode))
            {
                foreach (string str in textList)
                {
                    sw.WriteLine(str);
                }
            }
            MessageBox.Show("文件保存在：" + newFileName);
        }

        private void getStudentCountList(string str)
        {
            stuCountList = new List<int>();
            string[] strList = str.Split(',');
            int count = 0;
            int countAdded = 0;
            foreach (var cnt in strList)
            {
                if (!int.TryParse(cnt, out count))
                {
                    MessageBox.Show("分组情况输入不合法");
                    return;
                }
                else
                {
                    if (count == 0)
                    {
                        MessageBox.Show("分组情况不能有0");
                        return;
                    }
                    countAdded += count;
                    stuCountList.Add(count);
                }
            }
            if (countAdded != addStudentCount)
            {
                MessageBox.Show("分组数量和不等于学员个数，重新输入");
                return;
            }
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

        private string addUnuse(string str)
        {
            return "\'" + str + "\'";
        }

        private string getGroup()
        {
            if (addGroupCount > addStudentCount)
                return "组数大于学生数，重新输入";
            string value = "";
            float avg = (float)addStudentCount / addGroupCount;
            int avgInt = addStudentCount / addGroupCount;
            int reminder = addStudentCount % addGroupCount;

            for (int i = 0; i < addGroupCount - 1; i++)
            {
                if (i < reminder)
                {
                    value += avgInt + 1 + ",";
                }
                else
                {
                    value += avgInt + ",";
                }
            }
            value += avgInt;
            #region
            /*
            if ((avg - avgInt) == 0)
            {
                for (int i = 0; i < addGroupCount - 1; i++)
                {
                    value += avgInt + ",";
                }
                value += avgInt;
            }
            else if ((avg - avgInt) > 0.5)
            {
                for (int i = 0; i < addGroupCount - 1; i++)
                {
                    value += avgInt + 1 + ",";
                }
                value += addStudentCount - ((avgInt + 1) * (addGroupCount - 1));
            }
            else
            {
                for (int i = 0; i < addGroupCount - 1; i++)
                {
                    value += avgInt + ",";
                }
                value += addStudentCount - (avgInt * (addGroupCount - 1));
            }*/
            #endregion 其他私有函数
            return value;
        }

        #endregion

        #region 根据第一个配置生成文件

        private bool readSql(string fileName)
        {
            getStudentCountList(textBoxItem5.Text);
            using (StreamReader sr = new StreamReader(fileName, encode))
            {
                string line;
                textList = new List<string>();
                lastParam = new Params();
                lastParam.LocalProgramList = new List<LocalProgramClass>();
                lastParam.LocalRunList = new List<LocalRunClass>();

                tables = new Tables();

                while ((line = sr.ReadLine()) != null)
                {
                    textList.Add(line);

                    #region group表处理
                    if (line.StartsWith("CREATE TABLE `tb_group`"))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            textList.Add(line);
                            if (line.StartsWith(") ENGINE=InnoDB AUTO_INCREMENT="))
                            {
                                string noTmp = line.Remove(0, line.IndexOf("AUTO_INCREMENT=") + 15);
                                noTmp = noTmp.Remove(noTmp.IndexOf(" "));
                                int no = int.Parse(noTmp);
                                textList[textList.Count - 1] = string.Format(") ENGINE=InnoDB AUTO_INCREMENT={0} DEFAULT CHARSET=gbk;",
                                    int.Parse(noTmp) + addGroupCount - 1);
                            }
                            if (line.StartsWith("INSERT INTO `tb_group` VALUES"))
                            {
                                lastParam.Group = new GroupClass(line);
                                tables.AddGroup(line);
                                while ((line = sr.ReadLine()) != null)
                                {
                                    if (line == "")
                                    {
                                        addGroup();
                                        textList.Add(line);
                                        break;
                                    }
                                    else
                                    {
                                        lastParam.Group = new GroupClass(line);
                                        tables.AddGroup(line);
                                    }
                                    textList.Add(line);
                                }
                                break;
                            }
                        }
                        continue;
                    }
                    #endregion

                    #region groupInfo表处理
                    if (line.StartsWith("CREATE TABLE `tb_groupinfo`"))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            textList.Add(line);
                            if (line.StartsWith(") ENGINE=InnoDB AUTO_INCREMENT="))
                            {
                                string noTmp = line.Remove(0, line.IndexOf("AUTO_INCREMENT=") + 15);
                                noTmp = noTmp.Remove(noTmp.IndexOf(" "));
                                int no = int.Parse(noTmp);
                                textList[textList.Count - 1] = string.Format(") ENGINE=InnoDB AUTO_INCREMENT={0} DEFAULT CHARSET=gbk;",
                                    int.Parse(noTmp) + addStudentCount - 1);
                            }
                            if (line.StartsWith("INSERT INTO `tb_groupinfo` VALUES"))
                            {
                                lastParam.GroupInfo = new GroupInfoClass(line);
                                tables.AddGroupInfo(line);
                                while ((line = sr.ReadLine()) != null)
                                {
                                    if (line == "")
                                    {
                                        addGroupInfo();
                                        textList.Add(line);
                                        break;
                                    }
                                    else
                                    {
                                        lastParam.GroupInfo = new GroupInfoClass(line);
                                        tables.AddGroupInfo(line);
                                    }
                                    textList.Add(line);
                                }
                                break;
                            }
                        }
                        continue;
                    }
                    #endregion

                    #region localprogram表处理
                    if (line.StartsWith("CREATE TABLE `tb_localprogram`"))
                    {
                        bool findInsert = false;
                        while ((line = sr.ReadLine()) != null)
                        {
                            textList.Add(line);
                            if (line.StartsWith(") ENGINE=InnoDB AUTO_INCREMENT="))
                            {
                                string noTmp = line.Remove(0, line.IndexOf("AUTO_INCREMENT=") + 15);
                                noTmp = noTmp.Remove(noTmp.IndexOf(" "));
                                int no = int.Parse(noTmp);
                                textList[textList.Count - 1] = string.Format(") ENGINE=InnoDB AUTO_INCREMENT={0} DEFAULT CHARSET=gbk;",
                                    (int.Parse(noTmp) - 1) * addGroupCount + 1);
                            }
                            if (line.StartsWith("INSERT INTO `tb_localprogram` VALUES"))
                            {
                                lastParam.LocalProgramList.Add(new LocalProgramClass(line));
                                tables.AddLocalProgram(line);
                                findInsert = true;
                            }
                            else if (findInsert)
                            {
                                textList.RemoveAt(textList.Count - 1);
                                addLocalProgram();
                                textList.Add("");
                                break;
                            }
                        }
                        continue;
                    }
                    #endregion

                    #region localrun表处理
                    if (line.StartsWith("CREATE TABLE `tb_localrun`"))
                    {
                        bool findInsert = false;
                        while ((line = sr.ReadLine()) != null)
                        {
                            textList.Add(line);
                            if (line.StartsWith(") ENGINE=InnoDB AUTO_INCREMENT="))
                            {
                                string noTmp = line.Remove(0, line.IndexOf("AUTO_INCREMENT=") + 15);
                                noTmp = noTmp.Remove(noTmp.IndexOf(" "));
                                int no = int.Parse(noTmp);
                                textList[textList.Count - 1] = string.Format(") ENGINE=InnoDB AUTO_INCREMENT={0} DEFAULT CHARSET=gbk;",
                                    (int.Parse(noTmp) - 1) * addGroupCount + 1);
                            }
                            if (line.StartsWith("INSERT INTO `tb_localrun` VALUES"))
                            {
                                lastParam.LocalRunList.Add(new LocalRunClass(line));
                                tables.AddLocalRun(line);
                                findInsert = true;
                            }
                            else if (findInsert)
                            {
                                textList.RemoveAt(textList.Count - 1);
                                addLocalRun();
                                textList.Add("");
                                break;
                            }
                        }
                        continue;
                    }
                    #endregion

                    #region tb_student表处理
                    if (line.StartsWith("CREATE TABLE `tb_student`"))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            textList.Add(line);
                            if (line.StartsWith(") ENGINE=InnoDB AUTO_INCREMENT="))
                            {
                                string noTmp = line.Remove(0, line.IndexOf("AUTO_INCREMENT=") + 15);
                                noTmp = noTmp.Remove(noTmp.IndexOf(" "));
                                int no = int.Parse(noTmp);
                                textList[textList.Count - 1] = string.Format(") ENGINE=InnoDB AUTO_INCREMENT={0} DEFAULT CHARSET=gbk;",
                                    int.Parse(noTmp) + addStudentCount - 1);
                            }
                            if (line.StartsWith("INSERT INTO `tb_student` VALUES"))
                            {
                                lastParam.Student = new StudentClass(line);
                                tables.AddStudent(line);
                                while ((line = sr.ReadLine()) != null)
                                {
                                    if (line == "")
                                    {
                                        addStudent();
                                        textList.Add(line);
                                        break;
                                    }
                                    else
                                    {
                                        lastParam.Student = new StudentClass(line);
                                        tables.AddStudent(line);
                                    }
                                    textList.Add(line);
                                }
                                break;
                            }
                        }
                    }
                    #endregion

                    #region tb_user表处理
                    if (line.StartsWith("CREATE TABLE `tb_user`"))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            textList.Add(line);
                            if (line.StartsWith(") ENGINE=InnoDB AUTO_INCREMENT="))
                            {
                                string noTmp = line.Remove(0, line.IndexOf("AUTO_INCREMENT=") + 15);
                                noTmp = noTmp.Remove(noTmp.IndexOf(" "));
                                int no = int.Parse(noTmp);
                                textList[textList.Count - 1] = string.Format(") ENGINE=InnoDB AUTO_INCREMENT={0} DEFAULT CHARSET=gbk;",
                                    int.Parse(noTmp) + addStudentCount - 1);
                            }
                            if (line.StartsWith("INSERT INTO `tb_user` VALUES"))
                            {
                                lastParam.User = new UserClass(line);
                                tables.AddUser(line);
                                while ((line = sr.ReadLine()) != null)
                                {
                                    if (line == "")
                                    {
                                        addUser();
                                        textList.Add(line);
                                        break;
                                    }
                                    else
                                    {
                                        lastParam.User = new UserClass(line);
                                        tables.AddUser(line);
                                    }
                                    textList.Add(line);
                                }
                                break;
                            }
                        }
                        continue;
                    }
                    #endregion
                }
            }
            return true;
        }

        private void addGroup()
        {
            for (int i = 0; i < addGroupCount - 1; i++)
            {
                GroupClass group = new GroupClass();
                group.id = addSelf(lastParam.Group.id, i + 1);
                group.name = addSelfGroupName(lastParam.Group.name, i + 1);
                group.content = lastParam.Group.content;
                group.model = lastParam.Group.model;
                group.drive = lastParam.Group.drive;
                group.starid = lastParam.Group.starid;
                group.staruser = lastParam.Group.staruser;
                group.starpassword = lastParam.Group.starpassword;
                group.ip = addIP(deleteUnuse(lastParam.Group.ip), i + 1);
                group.port = lastParam.Group.port;
                group.remark = lastParam.Group.remark;
                group.link = lastParam.Group.link;
                group.stateno = lastParam.Group.stateno;
                string sqlString = group.GetText();
                textList.Add(sqlString);
                tables.AddGroup(sqlString);
            }
        }

        private void addGroupInfo()
        {
            int index = 2;
            for (int i = 0; i < addGroupCount; i++)
            {
                int j;
                if (i == 0)
                    j = 1;
                else
                    j = 0;
                for (; j < stuCountList[i]; j++)
                {
                    GroupInfoClass group = new GroupInfoClass();
                    group.id = index.ToString();
                    group.fk_gid = addSelf(lastParam.GroupInfo.fk_gid, i);
                    group.fk_sid = index.ToString();
                    group.remark = lastParam.GroupInfo.remark;
                    string sqlString = group.GetText();
                    textList.Add(sqlString);
                    index++;
                }
            }
        }

        private void addLocalProgram()
        {
            for (int i = 0; i < addGroupCount - 1; i++)
            {
                int rowIndex = 0;
                foreach (LocalProgramClass lpLast in lastParam.LocalProgramList)
                {
                    LocalProgramClass lp = new LocalProgramClass();
                    lp.id = addSelf(lpLast.id, (i + 1) * lastParam.LocalProgramList.Count);
                    lp.fk_lrid = addSelf(lpLast.fk_lrid, (i + 1) * lastParam.LocalProgramList.Count);
                    lp.program = lpLast.program;
                    lp.path = lpLast.path;
                    lp.parameter = lpLast.parameter;
                    lp.monnetpc = lpLast.monnetpc;
                    lp.port = addPort(lpLast.port, i + 1, rowIndex++);
                    lp.maxpva = lpLast.maxpva;
                    lp.maxpvd = lpLast.maxpvd;
                    lp.netdisp = lpLast.netdisp;
                    lp.sysname = lpLast.sysname;
                    string sqlString = lp.GetText();
                    textList.Add(sqlString);
                    tables.AddLocalProgram(sqlString);
                }
            }
        }

        private void addLocalRun()
        {
            for (int i = 0; i < addGroupCount - 1; i++)
            {
                foreach (LocalRunClass lrLast in lastParam.LocalRunList)
                {
                    LocalRunClass lr = new LocalRunClass();
                    lr.id = addSelf(lrLast.id, (i + 1) * lastParam.LocalRunList.Count);
                    lr.fk_gid = addSelf(lrLast.fk_gid, i + 1);
                    lr.name = lrLast.name;
                    lr.description = addSelf(lrLast.description, i + 1);
                    string sqlString = lr.GetText();
                    textList.Add(sqlString);
                    tables.AddLocalRun(sqlString);
                }
            }
        }

        private void addStudent()
        {
            for (int i = 0; i < addStudentCount - 1; i++)
            {
                StudentClass stu = new StudentClass();
                stu.id = addSelf(lastParam.Student.id, i + 1);
                stu.fk_uid = addSelf(lastParam.Student.fk_uid, i + 1);
                stu.name = addSelf(lastParam.Student.name, i + 1);
                stu.age = lastParam.Student.age;
                stu.gender = lastParam.Student.gender;
                stu.company = lastParam.Student.company;
                stu.idcard = lastParam.Student.idcard;
                stu.fk_cid = lastParam.Student.fk_cid;
                stu.notes = lastParam.Student.notes;
                stu.grouped = lastParam.Student.grouped;
                string sqlString = stu.GetText();
                textList.Add(sqlString);
                tables.AddStudent(sqlString);
            }
        }

        private void addUser()
        {
            for (int i = 0; i < addStudentCount - 1; i++)
            {
                UserClass user = new UserClass();
                user.id = addSelf(lastParam.User.id, i + 1);
                user.uid = addSelf(lastParam.User.uid, i + 1);
                user.password = lastParam.User.password;
                user.role = lastParam.User.role;
                user.email = lastParam.User.email;
                user.status = lastParam.User.status;
                user.question = lastParam.User.question;
                user.answer = lastParam.User.answer;
                user.remark = lastParam.User.remark;
                user.online = lastParam.User.online;
                string sqlString = user.GetText();
                textList.Add(sqlString);
                tables.AddUser(sqlString);
            }
        }

        private string addSelfGroupName(string str, int count)
        {
            string value = "";
            string rtnValue = "";
            int num = 0;
            bool findInt = false;
            if (str.ToUpper().Contains("MW"))
            {
                int pos = str.ToUpper().LastIndexOf("MW");
                rtnValue = str.Remove(pos + 2);
                str = str.ToUpper().Substring(pos + 2);
            }
            foreach (char ch in str)
            {
                if (int.TryParse(ch.ToString(), out num))
                {
                    value += ch;
                    findInt = true;
                }
                else if (findInt)
                    break;
            }
            if (findInt)
            {
                int addedNum = int.Parse(value) + count;
                rtnValue += str.Replace(value, addedNum.ToString());
            }
            else
                rtnValue += str;
            return rtnValue;
        }

        private string addSelf(string str, int count)
        {
            string value = "";
            string rtnValue = "";
            int num = 0;
            bool findInt = false;
            foreach (char ch in str)
            {
                if (int.TryParse(ch.ToString(), out num))
                {
                    value += ch;
                    findInt = true;
                }
                else if (findInt)
                    break;
            }
            if (findInt)
            {
                int addedNum = int.Parse(value) + count;
                rtnValue = str.Replace(value, addedNum.ToString());
            }
            else
                rtnValue = str;
            return rtnValue;
        }

        private string addIP(string str, int groupIndex)
        {
            int ipLast = int.Parse(str.Remove(0, str.LastIndexOf('.') + 1));
            for (int i = 0; i < groupIndex; i++)
            {
                ipLast += stuCountList[i];
            }
            if (ipLast > 255)
                MessageBox.Show("IP段设置出错，累加后大于255");
            string value = "\'" + str.Remove(str.LastIndexOf('.') + 1);
            value += ipLast.ToString() + "\'";
            return value;
        }

        private string addPort(string str, int groupIndex, int rowIndex)
        {
            if (checkBoxUnadd.Checked)
            {
                return str;
            }
            bool 达旗辅网添加就地 = false;
            if (string.IsNullOrEmpty(str))
                throw new Exception("存在空端口号或空行，请重新配置");
            if (checkBoxAdd2.Checked)
            {
                //if (达旗辅网添加就地)
                //{
                //    if (rowIndex < 3)
                //    {
                //        int num = int.Parse(str.Substring(1, 2));
                //        num += groupIndex;
                //        if (num > 99)//限定增长序号不能大于99
                //        {
                //            num -= 100;
                //        }
                //        return mdlIndex.ToString() + num.ToString("00") + str.Remove(0, 3);
                //    }
                //    else
                //    {
                //        int num = groupIndex * 2;
                //        int oldPort = int.Parse(str);
                //        return (num + oldPort).ToString();
                //    }
                //}
                //else
                {
                    int num = groupIndex * 2 * dataGridViewX6.Rows.Count;
                    int oldPort = int.Parse(str);
                    return (num + oldPort).ToString();
                }
            }
            else
            {
                int num = int.Parse(str.Substring(1, 2));
                num += groupIndex;
                if (num > 99)//限定增长序号不能大于99
                {
                    num -= 100;
                }
                return mdlIndex.ToString() + num.ToString("00") + str.Remove(0, 3);
            }
        }

        private void bindData()
        {
            dataGridViewX1.DataSource = tables.dtGroup;
            dataGridViewX2.DataSource = tables.dtLocalProgram;
            dataGridViewX3.DataSource = tables.dtLocalRun;
            dataGridViewX4.DataSource = tables.dtStudent;
            dataGridViewX5.DataSource = tables.dtUser;
        }

        private void textBoxItem1_TextChanged(object sender, EventArgs e)
        {
            textBoxItem4_TextChanged(sender, e);
        }

        private void textBoxItem4_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxItem1.Text, out addGroupCount))
            {
                MessageBox.Show("输入组个数");
                return;
            }
            if (!int.TryParse(textBoxItem4.Text, out addStudentCount))
            {
                MessageBox.Show("输入学员个数");
                return;
            }
            textBoxItem5.Text = getGroup();
        }

        private void textBoxItem5_TextChanged(object sender, EventArgs e)
        {
        }

        #endregion

        #region 根据base.sql以及文本框内容生成文件

        //初始化按钮
        private void buttonItem5_Click(object sender, EventArgs e)
        {
            superTabItem1.Visible = false;
            superTabItem2.Visible = false;
            superTabItem3.Visible = false;
            superTabItem4.Visible = false;
            superTabItem5.Visible = false;

            superTabItem6.Visible = true;
            //buttonItem10.Visible = true;
            textBoxItem8.Text = singleToDouble(textBoxItem8.Text);
            textBoxItem9.Text = singleToDouble(textBoxItem9.Text);

            if (initializeSql())
            {
                dataGridViewX6.DataSource = tablesTmp.dtLocalSetting;
                dataGridViewX6.Columns[0].Width = 80;
                dataGridViewX6.Columns[2].Width = 200;
                dataGridViewX6.Columns[3].Width = 140;
                dataGridViewX6.Columns[4].Width = 200;
                dataGridViewX6.Columns[5].Width = 200;
                dataGridViewX6.Columns[9].Width = 200;
            }
        }

        //保存
        private void buttonItem4_Click(object sender, EventArgs e)
        {
            dataGridViewX6.EndEdit();
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";
            save.FilterIndex = 1;

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string path = System.Windows.Forms.Application.StartupPath;
                    //generateNewSqlByBaseSql(path + "\\base.sql");
                    generateNewSqlOnlyInsert();
                    bindData();
                    superTabItem1.Visible = true;
                    superTabItem2.Visible = true;
                    superTabItem3.Visible = true;
                    superTabItem4.Visible = true;
                    superTabItem5.Visible = true;
                    superTabItem6.Visible = false;
                    writeSql(save.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存时出错：" + ex.Message);
                }
            }
        }

        //保存配置
        private void buttonItem9_Click(object sender, EventArgs e)
        {
            dataGridViewX6.EndEdit();
            string fileSave = "config.xml";
            getConfigurations();
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileSave = saveFileDialog1.FileName;
                XmlHelper.SerializeToXml(config, config.GetType(), fileSave, "");
                MessageBox.Show("配置文件保存在：" + Path.GetFullPath(fileSave));
            }
        }

        //读取配置
        private void buttonItem11_Click(object sender, EventArgs e)
        {
            string fileConfig = "config.xml";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    fileConfig = openFileDialog1.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            else
                return;

            /*if (!File.Exists("config.xml"))
            {
                MessageBox.Show("不存在配置文件config.xml");
                return;
            }*/
            config = new Config();
            //config = XmlHelper.DeserializeFromXml("config.xml", config.GetType()) as Config;
            config = XmlHelper.DeserializeFromXml(fileConfig, config.GetType()) as Config;
            setConfigurations();
            superTabItem1.Visible = false;
            superTabItem2.Visible = false;
            superTabItem3.Visible = false;
            superTabItem4.Visible = false;
            superTabItem5.Visible = false;
            superTabItem6.Visible = true;
            //buttonItem10.Visible = true;
            dataGridViewX6.DataSource = tablesTmp.dtLocalSetting;
            dataGridViewX6.Columns[0].Width = 80;
            dataGridViewX6.Columns[3].Width = 140;
            dataGridViewX6.Columns[4].Width = 200;
            dataGridViewX6.Columns[5].Width = 200;
            this.buttonItem5.Enabled = false;
        }

        //生成data
        private void buttonItem10_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridViewX6.EndEdit();
                if (tablesTmp == null)
                {
                    MessageBox.Show("需要先配置各个监控画面再生成该文件夹");
                    return;
                }
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "请选择mdl路径--注意:是mdl文件夹！！！！！！！！！！！";

                fbd.SelectedPath = System.IO.Path.GetFullPath("d:\\star");
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    path = fbd.SelectedPath;
                    if (Directory.Exists(path))
                    {
                        List<string> mdlList = getFileNames(path, "*.mdl");
                        if (mdlList.Count < 1)
                        {
                            MessageBox.Show("该star的mdl文件夹内无模型文件");
                            return;
                        }
                        else if (mdlList.Count > 1)
                        {
                            MdlListForm listForm = new MdlListForm(mdlList);
                            listForm.Show();
                            listForm.EventMdlSelected += new EventHandler(listForm_EventMdlSelected);
                            //makeDataFolder(path);
                        }
                        else
                        {
                            mdlName = mdlList[0].Remove(0, mdlList[0].LastIndexOf("\\") + 1);
                            mdlName = mdlName.Remove(mdlName.IndexOf('.'));
                            makeDataFolder(path);
                        }
                    }
                    else
                    {
                        MessageBox.Show("请选择star文件夹");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void listForm_EventMdlSelected(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (sender == null)
            {
                MessageBox.Show("需要选择一个模型");
                return;
            }
            mdlName = sender as string;
            mdlName = mdlName.Remove(0, mdlName.LastIndexOf("\\") + 1);
            mdlName = mdlName.Remove(mdlName.IndexOf('.'));
            makeDataFolder(path);
        }

        private void writeSql(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName, false, encode))
            {
                string lastLine = string.Empty;
                foreach (string str in textList)
                {
                    if (!str.Equals(lastLine, StringComparison.OrdinalIgnoreCase))
                    {
                        sw.WriteLine(str);
                        lastLine = str;
                    }
                }
            }
        }

        /// <summary>
        /// 生成数据库结构及数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool generateNewSqlByBaseSql(string fileName)
        {
            textList = new List<string>();
            tables = new Tables();
            using (StreamReader sr = new StreamReader(fileName, encode))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    textList.Add(line);
                    if (line.ToLower().Contains("use `db_sem`;"))
                        textList.Add(line);
                    #region group表处理
                    if (line.StartsWith("CREATE TABLE `tb_group`"))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.StartsWith("INSERT INTO `tb_group` VALUES"))
                            {
                                for (int i = 0; i < addGroupCount; i++)
                                {
                                    string sqlLine = getGroupParams(i + 1);
                                    tables.AddGroup(sqlLine);
                                    textList.Add(sqlLine);
                                }
                                break;
                            }
                            else
                                textList.Add(line);
                            if (line.StartsWith(") ENGINE=InnoDB AUTO_INCREMENT="))
                            {
                                string noTmp = line.Remove(0, line.IndexOf("AUTO_INCREMENT=") + 15);
                                noTmp = noTmp.Remove(noTmp.IndexOf(" "));
                                int no = int.Parse(noTmp);
                                textList[textList.Count - 1] = string.Format(") ENGINE=InnoDB AUTO_INCREMENT={0} DEFAULT CHARSET=gbk;",
                                    int.Parse(noTmp) + addGroupCount - 1);
                            }
                        }
                        continue;
                    }
                    #endregion

                    #region groupInfo表处理
                    if (line.StartsWith("CREATE TABLE `tb_groupinfo`"))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.StartsWith("INSERT INTO `tb_groupinfo` VALUES"))
                            {
                                int index = 1;
                                for (int i = 0; i < addGroupCount; i++)
                                {
                                    for (int j = 0; j < stuCountList[i]; j++)
                                    {
                                        string sqlLine = getGroupInfoParams(index++, i + 1);
                                        tables.AddGroupInfo(sqlLine);
                                        textList.Add(sqlLine);
                                    }
                                }
                                break;
                            }
                            else
                                textList.Add(line);
                            if (line.StartsWith(") ENGINE=InnoDB AUTO_INCREMENT="))
                            {
                                string noTmp = line.Remove(0, line.IndexOf("AUTO_INCREMENT=") + 15);
                                noTmp = noTmp.Remove(noTmp.IndexOf(" "));
                                int no = int.Parse(noTmp);
                                textList[textList.Count - 1] = string.Format(") ENGINE=InnoDB AUTO_INCREMENT={0} DEFAULT CHARSET=gbk;",
                                    int.Parse(noTmp) + addStudentCount - 1);
                            }
                        }
                        continue;
                    }
                    #endregion

                    #region localprogram表处理
                    if (line.StartsWith("CREATE TABLE `tb_localprogram`"))
                    {
                        bool findInsert = false;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.StartsWith("INSERT INTO `tb_localprogram` VALUES"))
                            {
                                findInsert = true;
                            }
                            else if (findInsert)
                            {
                                getLocalProgramParams();
                                textList.Add("");
                                break;
                            }
                            else
                                textList.Add(line);
                            if (line.StartsWith(") ENGINE=InnoDB AUTO_INCREMENT="))
                            {
                                string noTmp = line.Remove(0, line.IndexOf("AUTO_INCREMENT=") + 15);
                                noTmp = noTmp.Remove(noTmp.IndexOf(" "));
                                int no = int.Parse(noTmp);
                                textList[textList.Count - 1] = string.Format(") ENGINE=InnoDB AUTO_INCREMENT={0} DEFAULT CHARSET=gbk;",
                                    (int.Parse(noTmp) - 1) * addGroupCount + 1);
                            }
                        }
                        continue;
                    }
                    #endregion

                    #region localrun表处理
                    if (line.StartsWith("CREATE TABLE `tb_localrun`"))
                    {
                        bool findInsert = false;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.StartsWith("INSERT INTO `tb_localrun` VALUES"))
                            {
                                findInsert = true;
                            }
                            else if (findInsert)
                            {
                                getLocalRunParams();
                                textList.Add("");
                                break;
                            }
                            else
                                textList.Add(line);
                            if (line.StartsWith(") ENGINE=InnoDB AUTO_INCREMENT="))
                            {
                                string noTmp = line.Remove(0, line.IndexOf("AUTO_INCREMENT=") + 15);
                                noTmp = noTmp.Remove(noTmp.IndexOf(" "));
                                int no = int.Parse(noTmp);
                                textList[textList.Count - 1] = string.Format(") ENGINE=InnoDB AUTO_INCREMENT={0} DEFAULT CHARSET=gbk;",
                                    (int.Parse(noTmp) - 1) * addGroupCount + 1);
                            }
                        }
                        continue;
                    }
                    #endregion

                    #region tb_student表处理
                    if (line.StartsWith("CREATE TABLE `tb_student`"))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.StartsWith("INSERT INTO `tb_student` VALUES"))
                            {
                                for (int i = 0; i < addStudentCount; i++)
                                {
                                    string sqlLine = getStudentParams(i + 1);
                                    tables.AddStudent(sqlLine);
                                    textList.Add(sqlLine);
                                }
                                break;
                            }
                            else
                                textList.Add(line);
                            if (line.StartsWith(") ENGINE=InnoDB AUTO_INCREMENT="))
                            {
                                string noTmp = line.Remove(0, line.IndexOf("AUTO_INCREMENT=") + 15);
                                noTmp = noTmp.Remove(noTmp.IndexOf(" "));
                                int no = int.Parse(noTmp);
                                textList[textList.Count - 1] = string.Format(") ENGINE=InnoDB AUTO_INCREMENT={0} DEFAULT CHARSET=gbk;",
                                    int.Parse(noTmp) + addStudentCount - 1);
                            }
                        }
                    }
                    #endregion

                    #region tb_user表处理
                    if (line.StartsWith("CREATE TABLE `tb_user`"))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.StartsWith("INSERT INTO `tb_user` VALUES"))
                            {
                                string sqlAdmin = @"INSERT INTO `tb_user` VALUES (1,'10000','ADMIN','ADMIN',NULL,1,NULL,NULL,NULL,'OFF');";
                                tables.AddUser(sqlAdmin);
                                textList.Add(sqlAdmin);
                                for (int i = 0; i < addStudentCount; i++)
                                {
                                    string sqlLine = getUserParams(i + 1);
                                    tables.AddUser(sqlLine);
                                    textList.Add(sqlLine);
                                }
                                break;
                            }
                            else
                                textList.Add(line);
                            if (line.StartsWith(") ENGINE=InnoDB AUTO_INCREMENT="))
                            {
                                string noTmp = line.Remove(0, line.IndexOf("AUTO_INCREMENT=") + 15);
                                noTmp = noTmp.Remove(noTmp.IndexOf(" "));
                                int no = int.Parse(noTmp);
                                textList[textList.Count - 1] = string.Format(") ENGINE=InnoDB AUTO_INCREMENT={0} DEFAULT CHARSET=gbk;",
                                    int.Parse(noTmp) + addStudentCount - 1);
                            }
                        }
                        continue;
                    }
                    #endregion
                }
            }
            return true;
        }

        /// <summary>
        /// 只生成数据
        /// </summary>
        /// <returns></returns>
        private bool generateNewSqlOnlyInsert()
        {
            textList = new List<string>();
            tables = new Tables();
            textList.Add("USE `db_sem`;\n");
            textList.Add("SET FOREIGN_KEY_CHECKS = 0;");
            textList.Add("TRUNCATE TABLE tb_group;");
            textList.Add("TRUNCATE TABLE tb_groupinfo;");
            textList.Add("TRUNCATE TABLE tb_localprogram;");
            textList.Add("TRUNCATE TABLE tb_localrun;");
            textList.Add("TRUNCATE TABLE tb_student;");
            textList.Add("TRUNCATE TABLE tb_user;\n");
            #region group表处理
            for (int i = 0; i < addGroupCount; i++)
            {
                string sqlLine = getGroupParams(i + 1);
                tables.AddGroup(sqlLine);
                textList.Add(sqlLine);
            }
            textList.Add("");
            #endregion

            #region groupInfo表处理
            int index = 1;
            for (int i = 0; i < addGroupCount; i++)
            {
                for (int j = 0; j < stuCountList[i]; j++)
                {
                    string sqlLine = getGroupInfoParams(index++, i + 1);
                    tables.AddGroupInfo(sqlLine);
                    textList.Add(sqlLine);
                }
            }
            textList.Add("");
            #endregion

            #region localprogram表处理
            getLocalProgramParams();
            textList.Add("");
            #endregion

            #region localrun表处理
            getLocalRunParams();
            textList.Add("");
            #endregion

            #region tb_student表处理
            for (int i = 0; i < addStudentCount; i++)
            {
                string sqlLine = getStudentParams(i + 1);
                tables.AddStudent(sqlLine);
                textList.Add(sqlLine);
            }
            textList.Add("");
            #endregion

            #region tb_user表处理
            string sqlAdmin = @"INSERT INTO `tb_user` VALUES (1,'10000','ADMIN','ADMIN',NULL,1,NULL,NULL,NULL,'OFF');";
            tables.AddUser(sqlAdmin);
            textList.Add(sqlAdmin);
            for (int i = 0; i < addStudentCount; i++)
            {
                string sqlLine = getUserParams(i + 1);
                tables.AddUser(sqlLine);
                textList.Add(sqlLine);
            }
            textList.Add("");
            #endregion
            textList.Add("SET FOREIGN_KEY_CHECKS = 1;\n");
            return true;
        }

        private bool initializeSql()
        {
            List<string> localSettingList = new List<string>();
            localSettingList.Add(string.Format(@" (DCS,{0}DCS,'E:\\江南DCS\\江南DCS.exe','E:\\江南DCS','E:\\江南DCS\\monnetpc.exe',{1}0110,50000,50000,'JN01DCS','');",
                textBoxItem6.Text, textBoxItem14.Text));
            localSettingList.Add(string.Format(@" (LOC,{0}LOC,'E:\\江南LOC\\bin\\View.exe','E:\\江南LOC','E:\\江南LOC\\net\\monnetpc.exe',{1}0120,20000,20000,'JN01LOC','E:\\江南LOC\\江南LOC.mcp');",
                textBoxItem6.Text, textBoxItem14.Text));
            localSettingList.Add(string.Format(@" (ECS,{0}ECS,'E:\\江南ECS\\Exe\\江南ECS.exe','E:\\江南ECS\\Exe','E:\\江南ECS\\PC32\\monnetpc.exe',{1}0130,20000,20000,'JN01ECS','');",
                textBoxItem6.Text, textBoxItem14.Text));
            localSettingList.Add(string.Format(@" (DEH,{0}DEH,'E:\\江南DEH\\江南DEH.exe','E:\\江南DEH','E:\\江南DEH\\monnetpc.exe',{1}0140,50000,50000,'JN01DEH','');",
                textBoxItem6.Text, textBoxItem14.Text));
            localSettingList.Add(string.Format(@" (FGD,{0}FGD,'E:\\江南FGD\\江南FGD.exe','E:\\江南FGD','E:\\江南FGD\\monnetpc.exe',{1}0150,10000,10000,'JN01FGD','');",
                textBoxItem6.Text, textBoxItem14.Text));
            localSettingList.Add(string.Format(@" (FW,{0}FW,'E:\\江南FW\\江南FW.exe','E:\\江南FW','E:\\江南FW\\monnetpc.exe',{1}0160,10000,10000,'JN01FW','');",
                textBoxItem6.Text, textBoxItem14.Text));
            localSettingList.Add(string.Format(@" (XNP,{0}XNP,'E:\\江南XNP\\江南XNP.exe','E:\\江南XNP','E:\\江南XNP\\monnetpc.exe',{1}0170,10000,10000,'JN01XNP','');",
                textBoxItem6.Text, textBoxItem14.Text));
            localSettingList.Add(string.Format(@" (SIS,{0}SIS,'E:\\江南SIS\\江南SIS.exe','E:\\江南SIS','E:\\江南SIS\\monnetpc.exe',{1}0180,10000,10000,'JN01SIS','');",
                textBoxItem6.Text, textBoxItem14.Text));
            getStudentCountList(textBoxItem13.Text);
            tablesTmp = new Tables();
            int hmiCount = int.Parse(textBoxHmiCount.Text);
            for (int i = 0; i < hmiCount; i++)
            {
                if (i > 7)
                {
                    tablesTmp.AddLocalSetting(localSettingList[0]);
                }
                else
                {
                    tablesTmp.AddLocalSetting(localSettingList[i]);
                }
            }
            return true;
        }

        private void getConfigurations()
        {
            config = new Config();
            config.Group.name = textBoxItem6.Text;
            config.Group.content = textBoxItem7.Text;
            config.Group.model = textBoxItem8.Text;
            config.Group.drive = textBoxItem9.Text;
            config.Group.ip = textBoxItem29.Text;
            config.Group.stateno = textBoxItem10.Text;
            config.Group.studentCount = textBoxItem12.Text;
            config.Group.groupCount = textBoxItem11.Text;
            config.Group.groupInfo = textBoxItem13.Text;
            config.Group.mdlIndex = textBoxItem14.Text;
            config.Group.portadd2 = checkBoxAdd2.Checked;

            config.hmiCount = textBoxHmiCount.Text;
            if (tablesTmp != null)
                foreach (DataRow dr in tablesTmp.dtLocalSetting.Rows)
                {
                    DcsClass dcs = new DcsClass();

                    dcs.type = dr[0].ToString();
                    dcs.desc = dr[1].ToString();
                    dcs.program = dr[2].ToString();
                    dcs.pathPro = dr[3].ToString();
                    dcs.pathMonetpc = dr[4].ToString();
                    dcs.port = dr[5].ToString();
                    dcs.maxPva = dr[6].ToString();
                    dcs.maxPvd = dr[7].ToString();
                    dcs.systemName = dr[8].ToString();
                    dcs.runPara = dr[9].ToString();
                    config.dcsList.Add(dcs);
                }

            config.Student.fk_uid = textBoxItem18.Text;
            config.Student.name = textBoxItem19.Text;
            config.Student.age = textBoxItem20.Text;
            config.Student.gender = textBoxItem21.Text;
        }

        private void setConfigurations()
        {
            textBoxItem6.Text = config.Group.name;
            textBoxItem7.Text = config.Group.content;
            textBoxItem8.Text = config.Group.model;
            textBoxItem9.Text = config.Group.drive;
            textBoxItem29.Text = config.Group.ip;
            textBoxItem10.Text = config.Group.stateno;
            textBoxItem12.Text = config.Group.studentCount;
            textBoxItem11.Text = config.Group.groupCount;
            textBoxItem13.Text = config.Group.groupInfo;
            textBoxItem14.Text = config.Group.mdlIndex;
            checkBoxAdd2.Checked = config.Group.portadd2;

            textBoxHmiCount.Text = config.hmiCount;
            tablesTmp = new Tables();
            foreach (DcsClass dcs in config.dcsList)
            {
                tablesTmp.AddLocalSetting(string.Format(@" ({0},{1},'{2}','{3}','{4}',{5},{6},{7},'{8}','{9}');",
                    dcs.type, dcs.desc, dcs.program, dcs.pathPro, dcs.pathMonetpc, dcs.port, dcs.maxPva,
                    dcs.maxPvd, dcs.systemName, dcs.runPara));
            }

            textBoxItem18.Text = config.Student.fk_uid;
            textBoxItem19.Text = config.Student.name;
            textBoxItem20.Text = config.Student.age;
            textBoxItem21.Text = config.Student.gender;

            getStudentCountList(textBoxItem13.Text);
        }

        private void makeDataFolder(string path)
        {
            string pathData = System.Windows.Forms.Application.StartupPath + "\\data";
            string pathEmpty = System.Windows.Forms.Application.StartupPath + "\\空文件\\";
            if (Directory.Exists(pathData))
            {
                DirectoryInfo dInfo = new DirectoryInfo(pathData);
                dInfo.Delete(true);
                Thread.Sleep(200);
                dInfo.Create();
            }
            else
            {
                DirectoryInfo dInfo = new DirectoryInfo(pathData);
                dInfo.Create();
            }

            copyFile(path + "\\" + mdlName + ".ft", pathData, pathEmpty + "++++.ft");
            copyFile(path + "\\" + mdlName + ".mf", pathData, "");
            copyFile(path + "\\" + mdlName + "mf.ini", pathData, pathEmpty + "++++mf.ini");
            copyFile(path + "\\" + mdlName + "000.ini", pathData, "");
            copyFile(path + "\\" + mdlName + ".scn", pathData, pathEmpty + "++++.scn");
            copyFile(path + "\\" + mdlName + "head.exam", pathData, pathEmpty + "++++head.exam");
            try
            {
                makeNetFolder(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            MessageBox.Show("生成的文件保存在：" + pathData + "目录下！");
        }

        private void makeNetFolder(string path)
        {
            string pathNet = System.Windows.Forms.Application.StartupPath + "\\net";
            if (Directory.Exists(pathNet))
            {
                DirectoryInfo dInfo = new DirectoryInfo(pathNet);
                dInfo.Delete(true);
                Thread.Sleep(200);
                dInfo.Create();
            }
            else
            {
                DirectoryInfo dInfo = new DirectoryInfo(pathNet);
                dInfo.Create();
            }
            string netFile = path + "\\" + mdlName + ".net";
            using (BinaryReader br = new BinaryReader(new FileStream(netFile, FileMode.Open), Encoding.ASCII))
            {
                Net net = new Net(br);
                foreach (NetStruct ns in net.NetList)
                {
                    string typeNet = "";
                    foreach (char ch in ns.Type)
                    {
                        if (ch > 0)
                            typeNet += ch;
                        else
                            break;
                    }
                    //typeNet = typeNet.Remove(typeNet.IndexOf("\\0"));
                    bool findType = false;
                    bool portGood = false;
                    foreach (DataRow row in tablesTmp.dtLocalSetting.Rows)
                    {
                        if (string.Equals(typeNet.Trim(), row[0].ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            findType = true;
                            int portConfig = int.Parse(row[5].ToString());
                            if (ns.NetPort == portConfig)
                                portGood = true;
                            break;
                        }
                    }
                    if (!findType)
                    {
                        MessageBox.Show(string.Format("在监控画面配置列表中未找到网络类型{0}，未能生成网络配置文件", typeNet));
                        return;
                    }
                    if (!portGood)
                    {
                        string message = string.Format("监控画面配置列表中的网络类型{0}的端口号与star网络配置文件不一致，是否以本配置文件为准",
                            typeNet);
                        string caption = "网络配置不一致，是否以列表中配置为准";
                        MessageBoxButtons buttons = MessageBoxButtons.YesNo;

                        if (MessageBox.Show(message, caption, buttons) == System.Windows.Forms.DialogResult.Yes)
                        {
                            //this.Close();
                            break;
                        }
                        else
                        {
                            MessageBox.Show("由于选择了否，未能生成网络配置文件");
                            return;
                        }
                    }
                }
                for (int i = 0; i < addGroupCount; i++)
                {
                    for (int j = 0; j < net.NetList.Count; j++)
                    {
                        NetStruct ns = net.NetList[j];
                        string typeNet = "";
                        foreach (char ch in ns.Type)
                        {
                            if (ch > 0)
                                typeNet += ch;
                            else
                                break;
                        }
                        int rowIndex = 0;
                        foreach (DataRow row in tablesTmp.dtLocalSetting.Rows)
                        {
                            if (string.Equals(typeNet.Trim(), row[0].ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                int portTmp = ns.NetPort;

                                if (!int.TryParse(addPort(row[5].ToString(), i, rowIndex), out portTmp))
                                {
                                    throw new Exception("端口号配置有问题，请检查");
                                }
                                ns.NetPort = portTmp;
                            }
                            rowIndex++;
                        }
                        net.NetList[j] = ns;
                    }
                    copyNetFile(pathNet, net, i + 1);
                }
            }
        }

        private void copyNetFile(string pathNet, Net net, int groupIndex)
        {
            DirectoryInfo dInfo = new DirectoryInfo(pathNet + "\\" + groupIndex + "组网络配置");
            dInfo.Create();
            string destFileName = dInfo.FullName + "\\" + mdlName + ".net";
            using (BinaryWriter bw = new BinaryWriter(new FileStream(destFileName, FileMode.Create)))
            {
                byte[] bts = net.GetBytes();
                bw.Write(bts);
            }
        }

        private void copyFile(string sourceFile, string pathData, string emptyFile)
        {
            if (File.Exists(sourceFile))
            {
                string destFileName = sourceFile.Remove(0, sourceFile.LastIndexOf("\\") + 1);
                for (int i = 0; i < addGroupCount; i++)
                {
                    File.Copy(sourceFile,
                        string.Format("{2}\\{0}.{1}", i + 1, destFileName, pathData),
                        true);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(emptyFile))
                {
                    return;
                }
                string destFileName = emptyFile.Remove(0, emptyFile.LastIndexOf("\\") + 1);
                destFileName = destFileName.Replace("++++", mdlName);
                for (int i = 0; i < addGroupCount; i++)
                {
                    File.Copy(emptyFile,
                        string.Format("{2}\\{0}.{1}", i + 1, destFileName, pathData),
                        true);
                }
            }
        }

        private List<string> getFileNames(string strPath, string filter)
        {
            List<string> nameList = new List<string>();
            if (!string.IsNullOrEmpty(strPath))
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(strPath);
                    if (di.Exists)
                    {
                        foreach (FileInfo fi in di.GetFiles(filter))
                        {
                            nameList.Add(fi.FullName);
                        }
                    }
                }
                catch (IOException) { }
                catch (ArgumentException) { }
            }
            return nameList;
        }

        private string getGroupParams(int index)
        {
            GroupClass group = new GroupClass();
            group.id = index.ToString();
            group.name = addSelfGroupName(addUnuse(textBoxItem6.Text), index - 1);
            group.content = addUnuse(textBoxItem7.Text);
            group.model = addUnuse(textBoxItem8.Text);
            group.drive = addUnuse(textBoxItem9.Text);
            group.starid = "1";
            group.staruser = "\'star\'";
            group.starpassword = "\'star\'";
            group.ip = addIP(textBoxItem29.Text, index - 1);
            group.port = "9999";
            group.remark = "\'\'";
            group.link = "\'NO\'";
            group.stateno = addUnuse(textBoxItem10.Text);
            return group.GetText();
        }

        private string getGroupInfoParams(int index, int groupIndex)
        {
            GroupInfoClass group = new GroupInfoClass();
            group.id = index.ToString();
            group.fk_gid = groupIndex.ToString();
            group.fk_sid = index.ToString();
            group.remark = "\'\'";
            return group.GetText();
        }

        private void getLocalProgramParams()
        {
            int index = 1;
            for (int i = 0; i < addGroupCount; i++)
            {
                int rowIndex = 0;
                foreach (DataRow dr in tablesTmp.dtLocalSetting.Rows)
                {
                    LocalProgramClass lp = new LocalProgramClass();

                    lp.id = index.ToString();
                    lp.fk_lrid = index.ToString();
                    lp.program = addUnuse(dr[2].ToString());
                    lp.path = addUnuse(dr[3].ToString());
                    lp.parameter = addUnuse(dr[9].ToString());
                    lp.monnetpc = addUnuse(dr[4].ToString());
                    lp.port = addPort(dr[5].ToString(), i, rowIndex++);
                    lp.maxpva = dr[6].ToString();
                    lp.maxpvd = dr[7].ToString();
                    lp.netdisp = "\'NO\'";
                    lp.sysname = addUnuse(dr[8].ToString());
                    string sqlString = lp.GetText();
                    textList.Add(sqlString);
                    tables.AddLocalProgram(sqlString);
                    index++;
                }
            }
        }

        private void getLocalRunParams()
        {
            int index = 1;
            for (int i = 0; i < addGroupCount; i++)
            {
                foreach (DataRow dr in tablesTmp.dtLocalSetting.Rows)
                {
                    LocalRunClass lr = new LocalRunClass();
                    lr.id = index.ToString();
                    lr.fk_gid = (i + 1).ToString();
                    //SayHi的调用DCS名称里用的是name字段 mc 2018.06.19
                    //lr.name = addUnuse(lr.fk_gid.ToString() + "组" + dr[0].ToString());
                    lr.description = addSelfGroupName(addUnuse(dr[1].ToString()), i);
                    lr.name = lr.description;
                    string sqlString = lr.GetText();
                    textList.Add(sqlString);
                    tables.AddLocalRun(sqlString);
                    index++;
                }
            }
        }

        private string getStudentParams(int index)
        {
            StudentClass stu = new StudentClass();
            stu.id = index.ToString();
            stu.fk_uid = addSelf(addUnuse(textBoxItem18.Text), index - 1);
            stu.name = addSelfGroupName(addUnuse(textBoxItem19.Text), index - 1);
            stu.age = textBoxItem20.Text;
            stu.gender = addUnuse(textBoxItem21.Text);
            stu.company = "\'-\'";
            stu.idcard = "\'\'";
            stu.fk_cid = "1";
            stu.notes = "\'-\'";
            stu.grouped = "1";
            return stu.GetText();
        }

        private string getUserParams(int index)
        {
            UserClass user = new UserClass();
            user.id = (index + 1).ToString();
            user.uid = addSelf(addUnuse(textBoxItem18.Text), index - 1);
            user.password = "\'\'";
            user.role = "\'STUDENT\'";
            user.email = "\'\'";
            user.status = "1";
            user.question = "\'\'";
            user.answer = "\'\'";
            user.remark = "\'\'";
            user.online = "\'OFF\'";
            return user.GetText();
        }

        #endregion

        #region 根据两个SQL生成新文件

        /// <summary>
        /// 打开前一个SQL文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem6_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";
            open.FilterIndex = 1;
            open.RestoreDirectory = true;
            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    frontFileName = open.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("打开前一个SQL文件-读取SQL出错: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// 打开后一个SQL文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem7_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";
            open.FilterIndex = 1;
            open.RestoreDirectory = true;
            if (open.ShowDialog() == DialogResult.OK)
            {
                //try
                {
                    backFileName = open.FileName;
                    getBackInsertList();
                    generateNewSqlByFrontSql(frontFileName);
                }
                //catch (Exception ex)
                //{
                //    MessageBox.Show("打开后一个SQL文件-读取SQL出错: " + ex.Message);
                //}
            }
        }

        private void buttonItem8_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";
            save.FilterIndex = 1;

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //encode = Encoding.GetEncoding(comboBoxItem1.SelectedItem.ToString());
                    bindData();
                    superTabItem1.Visible = true;
                    superTabItem2.Visible = true;
                    superTabItem3.Visible = true;
                    superTabItem4.Visible = true;
                    superTabItem5.Visible = true;
                    superTabItem6.Visible = false;
                    writeSql(save.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存时出错：" + ex.Message);
                }
            }
        }

        //选择多个SQL文件
        private void buttonItem14_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";
            open.FilterIndex = 1;
            open.Multiselect = true;
            open.RestoreDirectory = true;
            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (open.FileNames.Length == 1)
                    {
                        MessageBox.Show("只选择了一个文件，将不会合并任何SQL文件！");
                    }
                    else if (open.FileNames.Length > 1)
                    {
                        for (int i = 1; i < open.FileNames.Length; i++)
                        {
                            backFileName = open.FileNames[i];
                            getBackInsertList();
                        }
                        frontFileName = open.FileNames[0];
                        generateNewSqlByFrontSql(frontFileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("读取SQL出错: " + ex.Message);
                }
            }
        }

        //查看已配置文件
        private void buttonItem16_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";
            open.FilterIndex = 1;
            open.Multiselect = false;
            open.RestoreDirectory = true;
            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (open.FileNames.Length == 1)
                    {
                        sqlToTable(open.FileName);
                        bindData();
                        superTabItem1.Visible = true;
                        superTabItem2.Visible = true;
                        superTabItem3.Visible = true;
                        superTabItem4.Visible = true;
                        superTabItem5.Visible = true;
                        superTabItem6.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("读取SQL出错: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// 将SQL填充到各个表中
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool sqlToTable(string fileName)
        {
            tables = new Tables();
            using (StreamReader sr = new StreamReader(fileName, encode))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("INSERT INTO `tb_group` VALUES"))
                    {
                        tables.AddGroup(line);
                    }
                    if (line.StartsWith("INSERT INTO `tb_groupinfo` VALUES"))
                    {
                        tables.AddGroupInfo(line);
                    }
                    if (line.StartsWith("INSERT INTO `tb_localprogram` VALUES"))
                    {
                        tables.AddLocalProgram(line);
                    }
                    if (line.StartsWith("INSERT INTO `tb_localrun` VALUES"))
                    {
                        tables.AddLocalRun(line);
                    }
                    if (line.StartsWith("INSERT INTO `tb_student` VALUES"))
                    {
                        tables.AddStudent(line);
                    }
                    if (line.StartsWith("INSERT INTO `tb_user` VALUES"))
                    {
                        tables.AddUser(line);
                    }
                }
            }
            return true;
        }

        private void buttonItem12_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";
            open.FilterIndex = 1;
            open.RestoreDirectory = true;
            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<string> strList = new List<string>();
                    using (StreamReader sr = new StreamReader(open.FileName, Encoding.GetEncoding("gbk")))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            strList.Add(gbk2utf8(line));
                        }
                    }
                    string saveFileName = open.FileName.ToUpper().Replace(".SQL", "_new.sql");
                    using (StreamWriter sw = new StreamWriter(saveFileName, false, Encoding.UTF8))
                    {
                        foreach (string str in strList)
                        {
                            sw.WriteLine(str);
                        }
                    }
                    MessageBox.Show("文件保存在：" + saveFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("读取SQL出错: " + ex.Message);
                }
            }
        }

        public static string ConvertEncodingTest(string str)
        {
            string value = "";
            Encoding big5 = Encoding.GetEncoding("big5");
            Encoding gb2312 = Encoding.GetEncoding("gb2312");
            Encoding utf8 = Encoding.GetEncoding("utf-8");
            Encoding ascii = Encoding.GetEncoding("ascii");
            Encoding gbk = Encoding.GetEncoding("gbk");
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            byte[] big5b = big5.GetBytes(str);
            byte[] gb2312b = gb2312.GetBytes(str);
            byte[] utf8b = utf8.GetBytes(str);
            byte[] asciib = ascii.GetBytes(str);
            byte[] isob = iso.GetBytes(str);
            //关键也就是这句了
            //byte[] gb2312c= Encoding.Convert(big5,gb2312,big5b);
            byte[] utf8c = Encoding.Convert(gb2312, utf8, gb2312b);
            byte[] utf8d = Encoding.Convert(iso, utf8, isob);
            byte[] gb2312d = Encoding.Convert(iso, gb2312, isob);
            byte[] gbkd = Encoding.Convert(iso, gbk, isob);
            value = utf8.GetString(utf8c);
            value = utf8.GetString(utf8d);
            value = gbk.GetString(gbkd);
            value = gb2312.GetString(gb2312d);

            return value;
        }

        private string gbk2utf8(string str)
        {
            string value = str;
            Encoding utf8 = Encoding.GetEncoding("utf-8");
            Encoding gbk = Encoding.GetEncoding("gbk");
            byte[] gbkStr = gbk.GetBytes(str);
            byte[] utf8Str = Encoding.Convert(gbk, utf8, gbkStr);
            value = utf8.GetString(utf8Str);
            return value;
        }

        private int getMaxId(int curIndex, string insertLine)
        {
            if (insertLine.Contains("("))
            {
                string str = insertLine.Substring(insertLine.IndexOf("(") + 1, insertLine.IndexOf(",") - insertLine.IndexOf("(") - 1).Trim();
                int rtnValue = curIndex;
                if (int.TryParse(str, out rtnValue))
                    return rtnValue + 1;
            }
            return curIndex + 1;
        }

        private bool generateNewSqlByFrontSql(string fileName)
        {
            textList = new List<string>();
            tables = new Tables();
            using (StreamReader sr = new StreamReader(fileName, encode))
            {
                string line;
                int addGroupIndex = 1;//后一部分组开始位置
                while ((line = sr.ReadLine()) != null)
                {
                    textList.Add(line);

                    #region group表处理
                    if (line.StartsWith("INSERT INTO `tb_group` VALUES"))
                    {
                        bool findInsert = true;
                        int curIndex = 1;
                        curIndex = getMaxId(curIndex, line);
                        tables.AddGroup(line);
                        textList.Add(line);
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.StartsWith("INSERT INTO `tb_group` VALUES"))
                            {
                                //curIndex++;
                                curIndex = getMaxId(curIndex, line);
                                findInsert = true;
                                tables.AddGroup(line);
                                textList.Add(line);
                            }
                            else if (findInsert)
                            {
                                addGroupIndex = curIndex - 1;
                                foreach (string str in backGroupList)
                                {
                                    string strTmp = modifyIndex(curIndex++, str);
                                    textList.Add(strTmp);
                                    tables.AddGroup(strTmp);
                                }
                                textList.Add(line);
                                break;
                            }
                            else
                                textList.Add(line);
                        }
                        continue;
                    }
                    #endregion

                    #region groupInfo表处理
                    if (line.StartsWith("INSERT INTO `tb_groupinfo` VALUES"))
                    {
                        bool findInsert = true;
                        int curIndex = 1;
                        curIndex = getMaxId(curIndex, line);
                        tables.AddGroupInfo(line);
                        textList.Add(line);
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.StartsWith("INSERT INTO `tb_groupinfo` VALUES"))
                            {
                                //curIndex++;
                                curIndex = getMaxId(curIndex, line);
                                findInsert = true;
                                tables.AddGroupInfo(line);
                                textList.Add(line);
                            }
                            else if (findInsert)
                            {
                                foreach (string str in backGroupInfoList)
                                {
                                    string strTmp = modifyGroupInfoIndex(curIndex, str, addGroupIndex);
                                    textList.Add(strTmp);
                                    tables.AddGroupInfo(strTmp);
                                    curIndex++;
                                }
                                textList.Add(line);
                                break;
                            }
                            else
                                textList.Add(line);
                        }
                        continue;
                    }
                    #endregion

                    #region localprogram表处理
                    if (line.StartsWith("INSERT INTO `tb_localprogram` VALUES"))
                    {
                        bool findInsert = true;
                        int curIndex = 1;
                        curIndex = getMaxId(curIndex, line);
                        tables.AddLocalProgram(line);
                        textList.Add(line);
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.StartsWith("INSERT INTO `tb_localprogram` VALUES"))
                            {
                                curIndex = getMaxId(curIndex, line);
                                findInsert = true;
                                tables.AddLocalProgram(line);
                                textList.Add(line);
                            }
                            else if (findInsert)
                            {
                                foreach (string str in backLocalProgramList)
                                {
                                    string strTmp = modify2IndexForLocalProgram(curIndex, str);
                                    textList.Add(strTmp);
                                    tables.AddLocalProgram(strTmp);
                                    curIndex++;
                                }
                                textList.Add(line);
                                break;
                            }
                            else
                                textList.Add(line);
                        }
                        continue;
                    }
                    #endregion

                    #region localrun表处理
                    if (line.StartsWith("INSERT INTO `tb_localrun` VALUES"))
                    {
                        bool findInsert = true;
                        int curIndex = 1;
                        curIndex = getMaxId(curIndex, line);
                        tables.AddLocalRun(line);
                        textList.Add(line);
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.StartsWith("INSERT INTO `tb_localrun` VALUES"))
                            {
                                //curIndex++;
                                curIndex = getMaxId(curIndex, line);
                                findInsert = true;
                                tables.AddLocalRun(line);
                                textList.Add(line);
                            }
                            else if (findInsert)
                            {
                                foreach (string str in backLocalRunList)
                                {
                                    string strTmp = modify2Index(curIndex, str, addGroupIndex);
                                    textList.Add(strTmp);
                                    tables.AddLocalRun(strTmp);
                                    curIndex++;
                                }
                                textList.Add(line);
                                break;
                            }
                            else
                                textList.Add(line);
                        }
                        continue;
                    }
                    #endregion

                    #region tb_student表处理
                    if (line.StartsWith("INSERT INTO `tb_student` VALUES"))
                    {
                        bool findInsert = true;
                        int curIndex = 1;
                        curIndex = getMaxId(curIndex, line);
                        tables.AddStudent(line);
                        textList.Add(line);
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.StartsWith("INSERT INTO `tb_student` VALUES"))
                            {
                                //curIndex++;
                                curIndex = getMaxId(curIndex, line);
                                findInsert = true;
                                tables.AddStudent(line);
                                textList.Add(line);
                            }
                            else if (findInsert)
                            {
                                foreach (string str in backStudentList)
                                {
                                    string strTmp = modifyIndex(curIndex++, str);
                                    textList.Add(strTmp);
                                    tables.AddStudent(strTmp);
                                }
                                textList.Add(line);
                                break;
                            }
                            else
                                textList.Add(line);
                        }
                    }
                    #endregion

                    #region tb_user表处理
                    if (line.StartsWith("INSERT INTO `tb_user` VALUES"))
                    {
                        bool findInsert = true;
                        int curIndex = 1;
                        curIndex = getMaxId(curIndex, line);
                        tables.AddUser(line);
                        textList.Add(line);
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.StartsWith("INSERT INTO `tb_user` VALUES"))
                            {
                                //curIndex++;
                                curIndex = getMaxId(curIndex, line);
                                findInsert = true;
                                tables.AddUser(line);
                                textList.Add(line);
                            }
                            else if (findInsert)
                            {
                                foreach (string str in backUserList)
                                {
                                    string strTmp = modifyIndex(curIndex++, str);
                                    textList.Add(strTmp);
                                    tables.AddUser(strTmp);
                                }
                                textList.Add(line);
                                break;
                            }
                            else
                                textList.Add(line);
                        }
                        continue;
                    }
                    #endregion
                }
            }
            return true;
        }

        private void getBackInsertList()
        {
            using (StreamReader sr = new StreamReader(backFileName, encode))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("INSERT INTO `tb_group` VALUES"))
                    {
                        backGroupList.Add(line);
                    }
                    else if (line.StartsWith("INSERT INTO `tb_groupinfo` VALUES"))
                    {
                        backGroupInfoList.Add(line);
                    }
                    else if (line.StartsWith("INSERT INTO `tb_localprogram` VALUES"))
                    {
                        backLocalProgramList.Add(line);
                    }
                    else if (line.StartsWith("INSERT INTO `tb_localrun` VALUES"))
                    {
                        backLocalRunList.Add(line);
                    }
                    else if (line.StartsWith("INSERT INTO `tb_student` VALUES"))
                    {
                        backStudentList.Add(line);
                    }
                    else if (line.StartsWith("INSERT INTO `tb_user` VALUES") && !line.Contains("INSERT INTO `tb_user` VALUES (1,'10000'"))
                    {
                        backUserList.Add(line);
                    }
                }
            }
        }

        private string modifyIndex(int index, string str)
        {
            string head = str.Remove(str.IndexOf("(") + 1);
            string end = str.Remove(0, str.IndexOf(","));
            return head + index.ToString() + end;
        }

        private string modify2Index(int index, string str, int addGroupIndex)
        {
            string head = str.Remove(str.IndexOf("(") + 1);
            string end = str.Remove(0, str.IndexOf(",") + 1);
            int groupId = int.Parse(end.Remove(end.IndexOf(",")));
            end = end.Remove(0, end.IndexOf(","));
            return head + index.ToString() + "," + (groupId + addGroupIndex).ToString() + end;
        }

        private string modify2IndexForLocalProgram(int index, string str)
        {
            string head = str.Remove(str.IndexOf("(") + 1);
            string end = str.Remove(0, str.IndexOf(",") + 1);
            int groupId = int.Parse(end.Remove(end.IndexOf(",")));
            end = end.Remove(0, end.IndexOf(","));
            return head + index.ToString() + "," + index.ToString() + end;
        }

        private string modifyGroupInfoIndex(int index, string str, int addGroupIndex)
        {
            string head = str.Remove(str.IndexOf("(") + 1);
            string end = str.Remove(0, str.IndexOf(")"));
            string value = str.Substring(str.IndexOf("(") + 1, str.LastIndexOf(")") - str.IndexOf("(") - 1);
            string[] strList = value.Split(',');
            strList[0] = index.ToString();
            strList[1] = (int.Parse(strList[1]) + addGroupIndex).ToString();
            strList[2] = index.ToString();
            string strs = strList[0] + "," + strList[1] + "," + strList[2] + "," + strList[3];
            value = head + strs + end;
            return value;
        }

        #endregion

        #region 控件事件

        private void textBoxItem12_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxItem11.Text, out addGroupCount))
            {
                MessageBox.Show("输入正确的组个数");
                return;
            }
            if (!int.TryParse(textBoxItem12.Text, out addStudentCount))
            {
                MessageBox.Show("输入正确的学员个数");
                return;
            }
            textBoxItem13.Text = getGroup();
        }

        private void textBoxItem11_TextChanged(object sender, EventArgs e)
        {
            textBoxItem12_TextChanged(sender, e);
        }

        private void comboBoxItem1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxItem1.SelectedText != "")
                encode = Encoding.GetEncoding(comboBoxItem1.SelectedItem.ToString());
        }

        private void textBoxItem14_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxItem14.Text, out mdlIndex))
            {
                MessageBox.Show("模型序号输入了非法字符");
            }
        }

        #endregion

        private void dataGridViewX6_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 9)
            {
                string value = dataGridViewX6.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                dataGridViewX6.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = singleToDouble(value);
            }
        }

        private string singleToDouble(string str)
        {
            string value = "";
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (i != str.Length - 1 && i > 0)
                {
                    if (c == '\\' && str[i + 1] != '\\' && str[i - 1] != '\\')
                    {
                        value += "\\";
                    }
                }
                value += c;
            }
            return value;
        }

        private void textBoxItem13_TextChanged(object sender, EventArgs e)
        {
            getStudentCountList(textBoxItem13.Text);
        }

        //选择DATA目录
        private void buttonItem13_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "请选择data路径--注意:是该工具生成的data文件夹";

                fbd.SelectedPath = System.IO.Path.GetFullPath("data");
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBoxItem16.Text = fbd.SelectedPath;

                    if (Directory.Exists(fbd.SelectedPath))
                    {
                        List<string> dataList = getFileNames(fbd.SelectedPath, "*.*");
                        firstList = new List<string>();
                        if (dataList.Count < 1)
                        {
                            MessageBox.Show("该文件夹无文件");
                            return;
                        }
                        else
                        {
                            foreach (string fileName in dataList)
                            {
                                string strTmp = Path.GetFileNameWithoutExtension(fileName);
                                if (strTmp.StartsWith("1."))
                                {
                                    firstList.Add(fileName);
                                }
                            }
                        }
                        if (firstList.Count < 6)
                            MessageBox.Show("选择的data文件夹内以1.开头的文件数目小于6，请检查");
                    }
                    else
                    {
                        MessageBox.Show("请选择data文件夹");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        //生成目录
        private void buttonItem15_Click(object sender, EventArgs e)
        {
            if (firstList == null || firstList.Count == 0)
            {
                MessageBox.Show("请先选择data文件夹");
                return;
            }
            int startIndex = 0;
            int endIndex = 0;
            int.TryParse(textBoxItem17.Text, out startIndex);
            int.TryParse(textBoxItem24.Text, out endIndex);
            if ((startIndex == endIndex && startIndex == 0) || startIndex > endIndex)
            {
                MessageBox.Show("起始序号和终止序号输入不合法");
                return;
            }
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "请选择新data路径-新生成的文件将保存在该文件夹";
                fbd.SelectedPath = System.IO.Path.GetFullPath("data");
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string newDataPath = fbd.SelectedPath;

                    if (Directory.Exists(fbd.SelectedPath))
                    {
                        foreach (string sourceFile in firstList)
                        {
                            string strTmp = Path.GetFileName(sourceFile);
                            for (int i = startIndex; i <= endIndex; i++)
                            {
                                File.Copy(sourceFile,
                                    string.Format("{2}\\{0}{1}", i, strTmp.Remove(0, 1), newDataPath),
                                    true);
                            }
                        }
                        MessageBox.Show("新文件已存储到：" + fbd.SelectedPath);
                    }
                    else
                    {
                        MessageBox.Show("请选择新data文件夹");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void copyNewDataFiles(string pathData)
        {
            if (firstList.Count > 0)
            {
            }
        }

        //根据excel生成学员和用户表
        private void buttonItem17_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;data source={0};Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'", openFileDialog1.FileName);
                DBExcelUser db = new DBExcelUser(connectionString);
                List<UserModel> mdList = db.GetAllData();
                string newFileName = "C:\\sql.txt";
                if (openFileDialog1.FileName.ToUpper().Contains(".XLSX"))
                {
                    newFileName = openFileDialog1.FileName.ToUpper().Replace(".XLSX", "_sql.TXT");
                }
                using (StreamWriter sw = new StreamWriter(newFileName, false))
                {
                    List<string> gonghao = new List<string>();
                    sw.WriteLine("-------以下内容拷贝替换到SQL的tb_student表-------");
                    int index = 1;
                    foreach (UserModel m in mdList)
                    {
                        if (gonghao.Contains(m.工号))
                        {
                            MessageBox.Show("存在相同的工号：" + m.工号 + ".请先修改excel");
                            return;
                        }
                        gonghao.Add(m.工号);
                        string s = string.Format("INSERT INTO `tb_student` VALUES ({0},'{1}','{2}',25,'MALE','{3}','',{4},'{5}号机组',1);", index, m.工号, m.名字, m.电厂, m.值次, m.机组);
                        sw.WriteLine(s);
                        index++;
                    }
                    sw.WriteLine("-------以下内容拷贝替换到SQL的tb_user表-------");
                    index = 2;
                    sw.WriteLine("INSERT INTO `tb_user` VALUES (1,'10000','ADMIN','ADMIN','',1,'','',NULL,'OFF');");
                    foreach (UserModel m in mdList)
                    {
                        string s = string.Format("INSERT INTO `tb_user` VALUES ({0},'{1}','','STUDENT','',1,'','','','');", index, m.工号);
                        sw.WriteLine(s);
                        index++;
                    }
                }
                MessageBox.Show("文件已保存在：" + newFileName);
            }
        }

        private void buttonItem18_Click(object sender, EventArgs e)
        {
            superTabItem1.Visible = false;
            superTabItem2.Visible = false;
            superTabItem3.Visible = false;
            superTabItem4.Visible = false;
            superTabItem5.Visible = false;
            superTabItem6.Visible = false;
            UserConfigTab.Visible = true;
            OperateDbSem sem = new OperateDbSem();
            List<string> nameList = OperateDbSem.GetModelNames();
            DataGridViewComboBoxColumn modelCombo = dataGvUserConfig.Columns[2] as DataGridViewComboBoxColumn;
            //modelCombo.Items.AddRange(nameList);
            foreach (var name in nameList)
                modelCombo.Items.Add(name);
        }

        private void buttonItem20_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "txt or csv files (*.txt)|*.txt|(*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName, encode))
                    {
                        string line = string.Empty;
                        List<UserConfigClass> ucList = new List<UserConfigClass>();
                        while ((line = sr.ReadLine()) != null)
                        {
                            UserConfigClass uc = new UserConfigClass();
                            if (line.Contains(","))//csv或者以英文，分隔的登录号和学员名
                            {
                                string[] strList = line.Split(',');
                                uc.loginId = strList[0].Trim().ToUpper();
                                uc.name = strList[1].Trim().ToUpper();
                            }
                            else if (line.Contains("，"))//以中文，分隔的登录号和学员名
                            {
                                string[] strList = line.Split(',');
                                uc.loginId = strList[0].Trim().ToUpper();
                                uc.name = strList[1].Trim().ToUpper();
                            }
                            else//只有学员名
                            {
                                uc.name = line.Trim().ToUpper();
                            }
                            ucList.Add(uc);
                        }
                        setToDatagrid(ucList);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导入文本出现错误：" + ex.Message);
                }
            }
        }

        private void setToDatagrid(List<UserConfigClass> list)
        {
            dataGvUserConfig.Rows.Clear();
            Tables tb = new Tables();
            int max = getTodayMax();
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.loginId))
                {
                    item.loginId = string.Format("{0}{1}", getTodayString(), (max++).ToString("000"));
                }
                tb.AddUserConfig(item);
                dataGvUserConfig.Rows.Add(item.loginId, item.name);
            }
            //dataGvUserConfig.DataSource = tb.dtUserConfig;
        }

        private int getTodayMax()
        {
            string head = getTodayString();
            List<string> ids = OperateDbSem.GetUserIds();
            int max = 0;
            foreach (string id in ids)
            {
                if (id.Length == 9)
                {
                    if (id.StartsWith(head))
                    {
                        string end = id.Substring(6);
                        int value = 0;
                        if (int.TryParse(end, out value))
                        {
                            if (value > max)
                                max = value;
                        }
                    }
                }
            }
            return (max + 1);
        }

        private string getTodayString()
        {
            DateTime dt = DateTime.Now;
            return string.Format("{0}{1}{2}", (dt.Year - 2000).ToString("00"), dt.Month.ToString("00"), dt.Day.ToString("00"));
        }

        private void buttonItem21_Click(object sender, EventArgs e)
        {
            dataGvUserConfig.EndEdit();
            string txtName = verifyInput();
            if (string.IsNullOrEmpty(txtName))
                return;
            using (StreamWriter sw = new StreamWriter(string.Format("学员\\{0}.csv", txtName), false, encode))
                foreach (DataGridViewRow row in dataGvUserConfig.Rows)
                {
                    if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                    {
                        string modelName = string.Empty;
                        if (row.Cells[2].Value != null)
                            modelName = row.Cells[2].Value.ToString();
                        UserConfigClass uc = new UserConfigClass();
                        uc.loginId = row.Cells[0].Value.ToString();
                        uc.name = row.Cells[1].Value.ToString();
                        uc.modelName = modelName;
                        int sid = OperateDbSem.GetStudentIdByName(uc.name);
                        if (sid > 0)
                        {
                            MessageBox.Show("已存在学员名：" + uc.name + "；请重新输入");
                        }
                        int uid = OperateDbSem.GetUserIdByUid(uc.loginId);
                        if (uid > 0)
                        {
                            MessageBox.Show("已存在学员登录号：" + uc.loginId + "；请重新输入");
                        }
                        try
                        {
                            OperateDbSem.InsertToSql(uc);
                            sw.WriteLine(string.Format("{0},{1},{2}", uc.loginId, uc.name, uc.modelName));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else if (dataGvUserConfig.Rows.IndexOf(row) != dataGvUserConfig.Rows.Count - 1)
                    {
                        MessageBox.Show("学员登录号和学员名不能为空");
                        row.Selected = true;
                    }
                }

            MessageBox.Show("已存入学员信息，请关闭该程序！");
        }

        private string verifyInput()
        {
            string headName = string.Empty;
            int index = 0;
            foreach (DataGridViewRow row in dataGvUserConfig.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                {
                    UserConfigClass uc = new UserConfigClass();
                    uc.loginId = row.Cells[0].Value.ToString();
                    uc.name = row.Cells[1].Value.ToString();
                    if (index == 0)
                        headName = uc.loginId;
                    int sid = OperateDbSem.GetStudentIdByName(uc.name);
                    if (sid > 0)
                    {
                        MessageBox.Show("已存在学员名：" + uc.name + "；请重新输入");
                        row.Selected = true;
                        headName = null;
                    }
                    int uid = OperateDbSem.GetUserIdByUid(uc.loginId);
                    if (uid > 0)
                    {
                        MessageBox.Show("已存在学员登录号：" + uc.loginId + "；请重新输入");
                        row.Selected = true;
                        headName = null;
                    }
                }
                else if (dataGvUserConfig.Rows.IndexOf(row) != dataGvUserConfig.Rows.Count - 1)
                {
                    MessageBox.Show("学员登录号和学员名不能为空");
                    row.Selected = true;
                    headName = null;
                }
                index++;
            }
            return headName;
        }

        private void buttonItem22_Click(object sender, EventArgs e)
        {
            dataGvUserConfig.EndEdit();
            int max = getTodayMax();
            int index = 0;
            foreach (DataGridViewRow row in dataGvUserConfig.Rows)
            {
                if (row.Cells[1].Value != null)
                {
                    row.Cells[0].Value = string.Format("{0}{1}", getTodayString(), (max + index).ToString("000"));
                }
                index++;
            }
        }

        private void buttonItem19_Click(object sender, EventArgs e)
        {
        }
    }
}