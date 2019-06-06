using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Zhn.Template.Parameters.Dto;

namespace Zhn.Template.Web.Models.Parameters
{
    [AutoMapFrom(typeof(GetParameterForEditOutput))]
    public class CreateOrEditParameterModalViewModel : GetParameterForEditOutput
    {
        public CreateOrEditParameterModalViewModel(GetParameterForEditOutput output)
        {
            output.MapTo(this);
        }

        public bool IsEditMode
        {
            get { return Parameter.Id.HasValue; }
        }
    }
}