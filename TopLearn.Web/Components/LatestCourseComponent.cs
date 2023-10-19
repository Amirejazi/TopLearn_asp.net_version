using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.Services.interfaces;

namespace TopLearn.Web.Components
{
    public class LatestCourseComponent: ViewComponent
    {
        private ICourseService _courseService;

        public LatestCourseComponent(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("/Views/Components/LatestCourseComponent.cshtml", _courseService.GetCourses().Item1));
        }
    }
}
