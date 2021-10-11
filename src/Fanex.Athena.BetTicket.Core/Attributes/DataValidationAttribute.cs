using Fanex.Athena.BetTicket.Core.Extensions;
using Fanex.Athena.BetTicket.Core.Models;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.BetTicket.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class DataValidationAttribute : TypeFilterAttribute
    {
        public DataValidationAttribute(bool validateCustId = false)
            : base(typeof(DataValidationImplAttribute))
        {
            Arguments = new object[] { validateCustId };
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DataValidationImplAttribute : ActionFilterAttribute
    {
        private const string ErrorMsg = "Invalid Params";

        private readonly IContextService _contextService;
        private readonly ILicenseeService _licenseeService;

        private readonly bool _validateCustId;

        public DataValidationImplAttribute(IContextService contextService, ILicenseeService licenseeService,
            bool validateCustId
          )
        {
            _contextService = contextService;
            _licenseeService = licenseeService;

            _validateCustId = validateCustId;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isValid = await IsValid(context.ActionArguments);
            if (!isValid)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status400BadRequest);
                context.HandleInvalidRequest(ErrorMsg);
            }
            else
            {
                await next();
            }
        }

        private async Task<bool> IsValid(IDictionary<string, object> actionArguments)
        {
            var result = !MetaModelHelper.IsMetaModel(actionArguments, out IValidationMetaModel validationMetaModel) || validationMetaModel.IsValid;

            result &= !_validateCustId || (MetaModelHelper.IsMetaModel(actionArguments, out ICustomerIdentityMetaModel customerIdentityMetaModel)
                && await _licenseeService.CheckCustomerBelongsTo(_contextService.AgentList, customerIdentityMetaModel.CustId, customerIdentityMetaModel.TransId));

            return result;
        }
    }
}