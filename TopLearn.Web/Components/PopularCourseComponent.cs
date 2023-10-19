using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.Services.interfaces;

namespace TopLearn.Web.Components
{
    public class PopularCourseComponent : ViewComponent
    {
        private ICourseService _courseService;

        public PopularCourseComponent(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("/Views/Components/PopularCourseComponent.cshtml", _courseService.GetPopularCourse()));
        }
    }
}
