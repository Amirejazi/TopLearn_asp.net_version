using System.Security.Claims;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.DTOs.Question;
using TopLearn.Core.Services.interfaces;
using TopLearn.DataLayer.Entities.Question;

namespace TopLearn.Web.Controllers
{
    public class ForumController : Controller
    {
        private IForumService _forumService;

        public ForumController(IForumService forumService)
        {
            _forumService = forumService;
        }

        public IActionResult Index(int? courseId, string filter="")
        {
            ViewBag.courseId = courseId;
            return View(_forumService.GetQuestions(courseId, filter));
        }

        #region CreateQuestion

        [Authorize]
        public IActionResult CreateQuestion(int id)
        {
            QuestionViewModel questionViewModel = new QuestionViewModel()
            {
                CourseId = id
            };
            return View(questionViewModel);
        }

        [HttpPost]
        public IActionResult CreateQuestion(QuestionViewModel questionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(questionViewModel);
            }
            var sanitizer = new HtmlSanitizer();
            questionViewModel.Body = sanitizer.Sanitize(questionViewModel.Body);
            Question question = new Question()
            {
                CourseId = questionViewModel.CourseId,
                UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString()),
                Body = questionViewModel.Body,
                Title = questionViewModel.Title
            };
            int questionId = _forumService.AddQuestion(question);
            return Redirect($"/Forum/ShowQuestion/{questionId}");
        }

        #endregion

        #region ShowQuestion

        public IActionResult ShowQuestion(int id)
        {
            return View(_forumService.ShowQuestion(id));
        }

        #endregion

        #region Answer

        [Authorize]
        public IActionResult Answer(int id, string sendAnswer)
        {
            if (!string.IsNullOrEmpty(sendAnswer))
            {
                var sanitizer = new HtmlSanitizer();
                sendAnswer = sanitizer.Sanitize(sendAnswer);
                _forumService.AddAnswer(new Answer()
                {
                    BodyAnswer = sendAnswer,
                    CreateDate = DateTime.Now,
                    UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString()),
                    QuestionId = id
                });
            }
            return RedirectToAction("ShowQuestion", new { id = id });
        }

        [Authorize]
        public IActionResult SelectIsTrueAnswer(int questionId, int answerId)
        {
            int currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var question = _forumService.ShowQuestion(questionId);
            if (question.Question.UserId == currentUserId)
            {
                _forumService.ChangeIsTrueAnswer(answerId, questionId);
            }
            return RedirectToAction("ShowQuestion", new { id = questionId });
        }
        #endregion
    }
}
