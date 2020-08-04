namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using TeisterMask.XMLHelper;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {

            var sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportProjectTasksDto[]), new XmlRootAttribute("Projects"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            namespaces.Add(string.Empty, string.Empty);

            using (StringWriter stringWriter = new StringWriter(sb))
            {
                var projects = context.Projects
                    .ToArray()
                    .Where(p => p.Tasks.Count > 0)
                    .Select(p => new ExportProjectTasksDto
                    {
                        ProjectName = p.Name,
                        TasksCount = p.Tasks.Count,
                        HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
                        Tasks = p.Tasks.Select(t => new TaskExportDto
                        {
                            Name = t.Name,
                            Label = t.LabelType.ToString()
                        })
                        .OrderBy(t => t.Name)
                        .ToArray()
                    })
                    .OrderByDescending(p => p.TasksCount)
                    .ThenBy(p => p.ProjectName)
                    .ToArray();

                xmlSerializer.Serialize(stringWriter, projects, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context.Employees
                .ToArray()
                .Where(e => e.EmployeesTasks.Any(et => et.Task.OpenDate >= date))
               .Select(e => new
               {
                   Username = e.Username,
                   Tasks = e.EmployeesTasks
                   .Where(t => t.Task.OpenDate >= date)
                   .OrderByDescending(t => t.Task.DueDate)
                   .ThenBy(t => t.Task.Name)
                   .Select(t => new
                   {
                       TaskName = t.Task.Name,
                       OpenDate = t.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                       DueDate = t.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                       LabelType = t.Task.LabelType.ToString(),
                       ExecutionType = t.Task.ExecutionType.ToString()
                   })
                   .ToArray()
               })
               .OrderByDescending(e => e.Tasks.Length)
               .ThenBy(e => e.Username)
               .Take(10)
               .ToArray();

            string jsonResult = JsonConvert.SerializeObject(employees, Formatting.Indented);

            return jsonResult;
        }
    }
}