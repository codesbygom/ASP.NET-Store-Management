using Microsoft.AspNetCore.Mvc.Filters;
using Store.Application.Exceptions;

namespace Store.Application.Filters
{
    public class ModelStateCheckFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            throw new ValidationException(errors);
        }
    }
}
