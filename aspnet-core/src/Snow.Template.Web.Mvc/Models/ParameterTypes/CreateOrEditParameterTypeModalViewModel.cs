using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Snow.Template.ParameterManager.ParameterTypes.Dto;

namespace Snow.Template.Web.Models.ParameterTypes
{
    [AutoMapFrom(typeof(GetParameterTypeForEditOutput))]
    public class CreateOrEditParameterTypeModalViewModel : GetParameterTypeForEditOutput
    {
        public CreateOrEditParameterTypeModalViewModel(GetParameterTypeForEditOutput output)
        {
            output.MapTo(this);
        }

        public bool IsEditMode
        {
            get { return ParameterType.Id.HasValue; }
        }
    }
}