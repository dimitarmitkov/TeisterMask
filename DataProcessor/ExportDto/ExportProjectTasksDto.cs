﻿using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ExportProjectTasksDto
    {
        [XmlAttribute("TasksCount")]
        public int TasksCount { get; set; }

        [XmlElement("ProjectName")]
        public string ProjectName { get; set; }

        [XmlElement("HasEndDate")]
        public string HasEndDate { get; set; }

        [XmlArray("Tasks")]
        public TaskExportDto[] Tasks { get; set; }
    }

    [XmlType("Task")]
    public class TaskExportDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Label")]
        public string Label { get; set; }
    }
}
//<Project TasksCount = "10" >
//    < ProjectName > Hyster - Yale </ ProjectName >
//    < HasEndDate > No </ HasEndDate >
//    < Tasks >
//      < Task >
//        < Name > Broadleaf </ Name >
//        < Label > JavaAdvanced </ Label >
//      </ Task >
//    </Tasks>
//  </Project>