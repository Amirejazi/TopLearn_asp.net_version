using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TopLearn.Core.DTOs.Question;
using TopLearn.Core.Services.interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Question;

namespace TopLearn.Core.Services
{
    public class ForumService: IForumService
    {
        private TopLearnContext _context;

        public ForumService(TopLearnContext context)
        {
            _context = context;
        }


        public int AddQuestion(Question question)
        {
            question.CreateDate = DateTime.Now;
            question.Modified = DateTime.Now;
            _context.Add(question);
            _context.SaveChanges();
            return question.QuestionId;
        }

        public ShowQuestionViewModel ShowQuestion(int questionId)
        {
            var questionVM = new ShowQuestionViewModel();
            questionVM.Question = _context.Questions.Include(q => q.User).FirstOrDefault(q => q.QuestionId==questionId);
            questionVM.Answers = _context.Answers.Where(q => q.QuestionId == questionId).Include(a => a.User).ToList();
            return questionVM;
        }

        public IEnumerable<Question> GetQuestions(int? courseId = null, string filter = "")
        {
            //IQueryable<Question> result = _context.Questions.Where(q => EF.Functions.Like(q.Title, $"%{filter}%"));
            IQueryable<Question> result = _context.Questions.Where(q => q.Title.Contains(filter));

            if (courseId != null)
            {
                result = result.Where(q => q.CourseId == courseId);
            }
            return result.Include(q=>q.User).Include(q=>q.Course).ToList();
        }

        public void AddAnswer(Answer answer)
        {
            _context.Answers.Add(answer);
            _context.SaveChanges();
        }

        public void ChangeIsTrueAnswer(int answerId, int questionId)
        {
            var answers = _context.Answers.Where(a => a.QuestionId == questionId);
            foreach (var answer in answers)
            {
                answer.IsTrue = false;
                if (answer.AnswerId == answerId)
                {
                    answer.IsTrue = true;
                }
            }
            _context.Answers.UpdateRange(answers);
            _context.SaveChanges();

        }
    }
}
