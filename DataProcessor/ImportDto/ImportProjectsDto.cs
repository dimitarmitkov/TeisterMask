namespace TeisterMask.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Project")]
    public class ImportProjectsDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(2), MaxLength(40)]
        public string Name { get; set; }

        [XmlElement("OpenDate")]
        [Required]
        public string OpenDate { get; set; }

        [XmlElement("DueDate")]
        public string DueDate { get; set; }

        [XmlArray("Tasks")]
        public ImportTasksDto[] Task { get; set; }
    }

    [XmlType("Task")]
    public class ImportTasksDto
    {
        [XmlElement("Name")]
        [MinLength(2), MaxLength(40)]
        public string Name { get; set; }

        [XmlElement("OpenDate")]
        [Required]
        public string OpenDate { get; set; }

        [XmlElement("DueDate")]
        [Required]
        public string DueDate { get; set; }

        [XmlElement("ExecutionType")]
        [Range(0, 3)]
        public int ExecutionType { get; set; }

        [XmlElement("LabelType")]
        [Range(0, 4)]
        public int LabelType { get; set; }
    }
}
//<Project>
//    <Name>S</Name>
//    <OpenDate>25/01/2018</OpenDate>
//    <DueDate>16/08/2019</DueDate>
//    <Tasks>
//      <Task>
//        <Name>Australian</Name>
//        <OpenDate>19/08/2018</OpenDate>
//        <DueDate>13/07/2019</DueDate>
//        <ExecutionType>2</ExecutionType>
//        <LabelType>0</LabelType>
//      </Task>
//    </Tasks>
//  </Project>