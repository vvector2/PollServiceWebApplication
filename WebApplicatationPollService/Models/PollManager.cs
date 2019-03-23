using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApplicatationPollService.Controllers;
using WebApplicatationPollService.Models.Entities;
using WebApplicatationPollService.Models.ViewModels;

namespace WebApplicatationPollService.Models {
    public class PollManager {
        //return query linq from filterModelOption
        public IEnumerable<PollEntity> GetPollsFromFilterOption(FilterOptionModelView filterModelOption,ApplicationDbContext db) {
            PaginationHandler<PollEntity> paginationHandler = new PaginationHandler<PollEntity>(GetProperSortExpression(filterModelOption.nameSort),
                GetFilterExpression(filterModelOption.phrase));
            return paginationHandler.GetEntityFromFilterOption(filterModelOption, db);
        }
        protected virtual Expression<Func<PollEntity,object>> GetProperSortExpression (string propertyName) {
            if (propertyName == "Question") return (x => x.Question);
            else if (propertyName == "View") return (x => x.View);
            else return (x => x.DateTime);
        }
        protected virtual Expression<Func<PollEntity, bool>> GetFilterExpression(string phrase) {
            return x => x.Question.ToLower().Contains(phrase);
        }
        public void Vote(VotePollModelView votePollModelView, ApplicationDbContext db) {
            var pollAnswer= db.PollAnswers.First(x => x.Id == votePollModelView.IdAnswer);
            pollAnswer.Votes++;
        }          
    }
    public class AdminPollManager : PollManager {
        protected override Expression<Func<PollEntity, object>> GetProperSortExpression(string propertyName) {
            if (propertyName == "Question") return (x => x.Question);
            else if (propertyName == "View") return (x => x.View);
            else if (propertyName == "Id") return (x => x.View);
            else if (propertyName == "User") return (x => x.View);
            else return (x => x.DateTime);
        }
        protected override Expression<Func<PollEntity, bool>> GetFilterExpression(string phrase) {
            return x => x.Question.ToLower().Contains(phrase) || x.Id.ToString().Contains(phrase) || x.UserCreator.ToString().Contains(phrase);
        }
    }



}