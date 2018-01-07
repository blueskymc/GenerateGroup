using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MainForm
{
    [XmlRoot(ElementName = "Configuration")]
    public class Config
    {
        [XmlElement(ElementName = "组设置")]
        public GroupClass Group { get; set; }

        [XmlElement(ElementName = "监控画面个数")]
        public string hmiCount { get; set; }

        [XmlElement(ElementName = "监控画面设置")]
        public List<DcsClass> dcsList { get; set; }

        [XmlElement(ElementName = "学员设置")]
        public StudentClass Student { get; set; }

        public Config()
        {
            dcsList = new List<DcsClass>();
            Group = new GroupClass();
            Student = new StudentClass();
        }
    }
}
