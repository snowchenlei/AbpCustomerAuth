using System;
using System.Collections.Generic;
using System.Text;
using Snow.Template.Authorization.Users.Dto;
using Snow.Template.Dto;

namespace Snow.Template.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}

