using CiviAIReview.Interface;
using CiviAIReview.Models;

namespace CiviAIReview.Service
{
    public class MenuSvc : IMenuSvc
    {
        public List<MenuItem> GetMenuItems()
        {
            return new List<MenuItem>
            {
                new MenuItem { Name = "Home", Controller = "Home", Action = "Index", Icon = "/images/home.png" },
                new MenuItem { Name = "Users", Controller = "Users", Action = "UserAccount", Icon = "/images/users.png" },
                new MenuItem { Name = "Subjects", Controller = "Subjects", Action = "Index", Icon = "/images/subjects.png" },
                new MenuItem { Name = "Lessons", Controller = "Lessons", Action = "ProcessLessons", Icon = "/images/lessons.png" },
                new MenuItem { Name = "Exams", Controller = "Exams", Action = "Index", Icon = "/images/exam.png" },
                new MenuItem { Name = "Results", Controller = "Results", Action = "Index", Icon = "/images/result.png" },
                //new MenuItem { Name = "Settings", Controller = "Settings", Action = "Index", Icon = "images/settings.png" }
            };
        }
    }
}
