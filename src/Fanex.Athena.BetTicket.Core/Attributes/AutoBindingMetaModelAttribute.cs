using Fanex.Athena.BetTicket.Core.Models;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.BetTicket.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AutoBindingMetaModelAttribute : TypeFilterAttribute
    {
        public AutoBindingMetaModelAttribute(
            bool bindAgent = false,
            bool bindLanguage = false,
            bool bindLicenseeId = false,
            bool bindPlayerComm = false,
            bool bindBetFromIP = false
            )
            : base(typeof(AutoBindingMetaModelImplAttribute))
        {
            Arguments = new object[] { bindAgent, bindLanguage, bindLicenseeId, bindPlayerComm, bindBetFromIP };
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AutoBindingMetaModelImplAttribute : ActionFilterAttribute
    {
        private readonly IMetaModelBindingService _bindingService;
        private readonly IContextService _contextService;

        private readonly bool _bindAgent;
        private readonly bool _bindLanguage;
        private readonly bool _bindLicenseeId;
        private readonly bool _bindPlayerComm;
        private readonly bool _bindBetFromIP;

        public AutoBindingMetaModelImplAttribute(IMetaModelBindingService bindingService, IContextService contextService,
            bool bindAgent,
            bool bindLanguage,
            bool bindLicenseeId,
            bool bindPlayerComm,
            bool bindBetFromIP
         )
        {
            _bindingService = bindingService;
            _contextService = contextService;

            _bindAgent = bindAgent;
            _bindLanguage = bindLanguage;
            _bindLicenseeId = bindLicenseeId;
            _bindPlayerComm = bindPlayerComm;
            _bindBetFromIP = bindBetFromIP;
    }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_bindAgent)
            {
                _bindingService.BindAgentXml(context.ActionArguments, _contextService.AgentList as IDictionary<int, int[]>);
            }

            if (_bindPlayerComm)
            {
                _bindingService.BindPlayerCom(context.ActionArguments, _contextService.IsAllowedCustomerComm);
            }

            if (_bindLicenseeId && MetaModelHelper.IsMetaModel(context.ActionArguments, out ILicenseeIdMetaModel licenseeIdMetaModel))
            {
                licenseeIdMetaModel.LicenseeId = _contextService.LicenseeId;
            }

            if (_bindBetFromIP && MetaModelHelper.IsMetaModel(context.ActionArguments, out IShowBetFromIPMetaModel showBetFromIPMetaModel))
            {
                showBetFromIPMetaModel.IsShowIp = _contextService.IsShowIP;
                showBetFromIPMetaModel.IsShowIpVN = _contextService.IsShowIpVN;
            }

            if (_bindLanguage && MetaModelHelper.IsMetaModel(context.ActionArguments, out ILanguageMetaModel languageMetaModel))
            {
                languageMetaModel.Language = _contextService.Language;
            }
         
            await next();
        }
    }
}