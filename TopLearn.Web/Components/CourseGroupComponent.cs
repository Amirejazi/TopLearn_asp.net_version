using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.Services.interfaces;

namespace TopLearn.Web.Components
{
    public class CourseGroupComponent: ViewComponent
    {
        private ICourseService _courseService;

        public CourseGroupComponent(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("/Views/Components/CourseGroupComponent.cshtml", _courseService.getAllGroups()));
        }
    }
}
