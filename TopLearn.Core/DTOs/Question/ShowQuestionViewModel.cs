using TopLearn.DataLayer.Entities.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.Core.DTOs.Question
{
    public class ShowQuestionViewModel
    {
        public DataLayer.Entities.Question.Question Question { get; set; }
        public System.Collections.Generic.List<Answer> Answers { get; set; }
    }
}
