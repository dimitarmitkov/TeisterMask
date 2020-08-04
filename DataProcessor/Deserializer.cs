namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using System.Text;
    using TeisterMask.XMLHelper;
    using TeisterMask.DataProcessor.ImportDto;
    using TeisterMask.Data.Models;
    using System.Globalization;
    using System.Linq;
    using TeisterMask.Data.Models.Enums;
    using Newtonsoft.Json;
    using Castle.Core.Internal;
    using System.Drawing;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var sb = new StringBuilder();

            const string rootElement = "Projects";

            var projectsResultDto = XMLConverter.Deserializer<ImportProjectsDto>(xmlString, rootElement);

            var projectList = new List<Project>();

            foreach (var projectDto in projectsResultDto)
            {
                if (!IsValid(projectDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var projectOpenDate = DateTime.ParseExact(projectDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                var projectDueDate = !projectDto.DueDate.IsNullOrEmpty() ? DateTime.ParseExact(projectDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;

                var project = new Project
                {
                    Name = projectDto.Name,
                    OpenDate = projectOpenDate,
                    DueDate = projectDueDate,
                };

                foreach (var taskDto in projectDto.Task)
                {
                    var taskOpenDate = DateTime.ParseExact(taskDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var taskDueDate = DateTime.ParseExact(taskDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (!IsValid(taskDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (taskOpenDate < projectOpenDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (projectDueDate.HasValue)
                    {
                        if (taskDueDate > projectDueDate.Value)
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }
                    }

                    var task = new Task
                    {
                        Name = taskDto.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDueDate,
                        ExecutionType = (ExecutionType)taskDto.ExecutionType,
                        LabelType = (LabelType)taskDto.ExecutionType
                    };
                    project.Tasks.Add(task);
                }

                projectList.Add(project);
                sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }

            context.Projects.AddRange(projectList);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var employeesResultsDto = JsonConvert
                .DeserializeObject<ImportEmplyeesDto[]>(jsonString);


            var epmployeesList = new List<Employee>();

            foreach (var employeeDto in employeesResultsDto)
            {

                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool hasCorrectMail = epmployeesList.FirstOrDefault(e => e.Email == employeeDto.Email) != null;

                bool hasCorrectPhone = epmployeesList.FirstOrDefault(e => e.Phone == employeeDto.Phone) != null;

                if (hasCorrectMail && hasCorrectPhone)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var employee = new Employee
                {
                    Username = employeeDto.Username,
                    Email = employeeDto.Email,
                    Phone = employeeDto.Phone,
                };

                foreach (var taskId in employeeDto.Tasks.Distinct())
                {
                    if (context.Tasks.Find(taskId) == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    employee.EmployeesTasks.Add(new EmployeeTask
                    {
                        Employee = employee,
                        TaskId = taskId
                    });
                }


                epmployeesList.Add(employee);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
            }
            context.Employees.AddRange(epmployeesList);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}