using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Snow.Template.Auditing.Dto;
using Snow.Template.Dto;

namespace Snow.Template.Auditing
{
  public   interface IAuditLogAppService: IApplicationService
    {
        Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input);
        

        //Task<FileDto> GetAuditLogsToExcel(GetAuditLogsInput input);

        //Task<PagedResultDto<EntityChangeListDto>> GetEntityChanges(GetEntityChangeInput input);

        //Task<PagedResultDto<EntityChangeListDto>> GetEntityTypeChanges(GetEntityTypeChangeInput input);

        //Task<FileDto> GetEntityChangesToExcel(GetEntityChangeInput input);

        Task<List<EntityPropertyChangeDto>> GetEntityPropertyChanges(long entityChangeId);
        //List<NameValueDto> GetEntityHistoryObjectTypes();
    }
}

