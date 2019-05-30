using System;
using System.Collections.Generic;
using System.Text;
using Zhn.Template.Authorization.Users.Dto;
using Zhn.Template.Dto;

namespace Zhn.Template.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}
