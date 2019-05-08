﻿using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Zhn.Template.Authorization.Users.Dto
{
    public class UserListDto : EntityDto<long>, IPassivable, IHasCreationTime
    {

        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }

        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public Guid? ProfilePictureId { get; set; }

        public bool IsActive { get; set; }

        public List<UserListRoleDto> Roles { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
