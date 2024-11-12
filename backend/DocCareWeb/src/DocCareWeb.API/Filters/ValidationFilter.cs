using DocCareWeb.Application.Notifications;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DocCareWeb.API.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly INotificator _notificator;
        private readonly IServiceProvider _serviceProvider;

        public ValidationFilter(IServiceProvider serviceProvider, INotificator notificator)
        {
            _serviceProvider = serviceProvider;
            _notificator = notificator;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var argument in context.ActionArguments)
            {
                if (argument.Value == null)
                    continue;

                var argumentType = argument.Value.GetType();
                var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
                var validator = _serviceProvider.GetService(validatorType) as IValidator;

                if (validator != null)
                {
                    var validateAsyncMethod = typeof(IValidator).GetMethod("ValidateAsync", new[] { typeof(IValidationContext), typeof(CancellationToken) });

                    ValidationResult validationResult;

                    if (validateAsyncMethod != null)
                    {
                        var task = validateAsyncMethod.Invoke(validator, new object[] { new ValidationContext<object>(argument.Value), CancellationToken.None }) as Task<ValidationResult>;
                        validationResult = task != null ? await task : new ValidationResult();
                    }
                    else
                    {
                        validationResult = validator.Validate(new ValidationContext<object>(argument.Value));
                    }

                    if (!validationResult.IsValid)
                    {
                        AddNotifications(validationResult);
                        CreateResponse(context);                       
                        return;
                    }
                }
            }

            await next(); 
        }

        private void AddNotifications(FluentValidation.Results.ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                _notificator.AddNotification(new Notification(error.ErrorMessage));
            }
        }

        private void CreateResponse(ActionExecutingContext context)
        {
            var errors = _notificator.GetNotifications().Select(n => n.Message);

            var response = new
            {
                success = false,
                errors = errors
            };

            context.Result = new BadRequestObjectResult(response);
        }
    }
}