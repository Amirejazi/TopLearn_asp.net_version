using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.DTOs.Question;
using TopLearn.DataLayer.Entities.Question;

namespace TopLearn.Core.Services.interfaces
{
    public interface IForumService
    {
        #region Question

        int AddQuestion(Question question);
        ShowQuestionViewModel ShowQuestion(int questionId);
        IEnumerable<Question> GetQuestions(int? courseId=null, string filter="");

        #endregion

        #region Answer

        void AddAnswer(Answer answer);
        void ChangeIsTrueAnswer(int answerId, int questionId);

        #endregion
    }
}
