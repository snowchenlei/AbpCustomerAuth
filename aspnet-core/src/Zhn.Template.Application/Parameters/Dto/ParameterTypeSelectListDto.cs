using System;
using System.Collections.Generic;
using System.Text;

namespace Zhn.Template.Parameters.Dto
{
    public class ParameterTypeSelectListDto : ParameterTypeListDto
    {
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected { get; set; }
    }
}