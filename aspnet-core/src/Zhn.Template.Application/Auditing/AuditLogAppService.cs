using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityHistory;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Zhn.Template.Auditing.Dto;
using Zhn.Template.Authorization;
using Zhn.Template.Authorization.Users;

namespace Zhn.Template.Auditing
{
    [AbpAuthorize(PermissionNames.Pages_Administration_AuditLogs)]
    public class AuditLogAppService : TemplateAppServiceBase, IAuditLogAppService
    {
        private readonly INamespaceStripper _namespaceStripper;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<AuditLog, long> _auditLogRepository;
        private readonly IRepository<EntityPropertyChange, long> _entityPropertyChangeRepository;

        public AuditLogAppService(IRepository<AuditLog, long> auditLogRepository, IRepository<User, long> userRepository, INamespaceStripper namespaceStripper, IRepository<EntityPropertyChange, long> entityPropertyChangeRepository)
        {
            _auditLogRepository = auditLogRepository;
            _userRepository = userRepository;
            _namespaceStripper = namespaceStripper;
            _entityPropertyChangeRepository = entityPropertyChangeRepository;
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_AuditLogs)]
        public async Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input)
        {
            var query = CreateAuditLogAndUsersQuery(input);

            var resultCount = await query.CountAsync();
            var results = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var auditLogListDtos = ConvertToAuditLogListDtos(results);

            return new PagedResultDto<AuditLogListDto>(resultCount, auditLogListDtos);
        }

        private List<AuditLogListDto> ConvertToAuditLogListDtos(List<AuditLogAndUser> results)
        {
            return results.Select(
                result =>
                {
                    var auditLogListDto = ObjectMapper.Map<AuditLogListDto>(result.AuditLog);
                    auditLogListDto.UserName = result.User?.UserName;
                    auditLogListDto.ServiceName = _namespaceStripper.StripNameSpace(auditLogListDto.ServiceName);
                    return auditLogListDto;
                }).ToList();
        }

        private IQueryable<AuditLogAndUser> CreateAuditLogAndUsersQuery(GetAuditLogsInput input)
        {
            DateTime[] times = new DateTime[] { DateTime.Now.AddDays(-1), DateTime.Now, };
            if (!String.IsNullOrEmpty(input.DateRange) && input.DateRange.IndexOf("~", StringComparison.Ordinal) > 0)
            {
                string[] dates = input.DateRange.Split(new[] { '~' }, StringSplitOptions.RemoveEmptyEntries);
                times = Array.ConvertAll(dates, Convert.ToDateTime);
            }

            var query = from auditLog in _auditLogRepository.GetAll()
                        join user in _userRepository.GetAll() on auditLog.UserId equals user.Id into userJoin
                        from joinedUser in userJoin.DefaultIfEmpty()
                        where auditLog.ExecutionTime >= times[0] && auditLog.ExecutionTime <= times[1]
                        select new AuditLogAndUser { AuditLog = auditLog, User = joinedUser };

            query = query
                .WhereIf(!input.UserName.IsNullOrWhiteSpace(), item => item.User.UserName.Contains(input.UserName))
                .WhereIf(!input.ServiceName.IsNullOrWhiteSpace(), item => item.AuditLog.ServiceName.Contains(input.ServiceName))
                .WhereIf(!input.MethodName.IsNullOrWhiteSpace(), item => item.AuditLog.MethodName.Contains(input.MethodName))
                .WhereIf(!input.BrowserInfo.IsNullOrWhiteSpace(), item => item.AuditLog.BrowserInfo.Contains(input.BrowserInfo))
                .WhereIf(input.MinExecutionDuration.HasValue && input.MinExecutionDuration > 0, item => item.AuditLog.ExecutionDuration >= input.MinExecutionDuration.Value)
                .WhereIf(input.MaxExecutionDuration.HasValue && input.MaxExecutionDuration < int.MaxValue, item => item.AuditLog.ExecutionDuration <= input.MaxExecutionDuration.Value)
                .WhereIf(input.HasException == true, item => item.AuditLog.Exception != null && item.AuditLog.Exception != "")
                .WhereIf(input.HasException == false, item => item.AuditLog.Exception == null || item.AuditLog.Exception == "");
            return query;
        }

        public async Task<List<EntityPropertyChangeDto>> GetEntityPropertyChanges(long entityChangeId)
        {
            var entityPropertyChanges = (await _entityPropertyChangeRepository.GetAllListAsync())
                .Where(epc => epc.EntityChangeId == entityChangeId);

            return ObjectMapper.Map<List<EntityPropertyChangeDto>>(entityPropertyChanges);
        }
    }
}