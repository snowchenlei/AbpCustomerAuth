using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Snow.Template.Swagger
{
    public class SwaggerEnumParameterFilter : IParameterFilter
    {
        // TODO:Swagger支持枚举升级
        //public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        //{
        //    var type = Nullable.GetUnderlyingType(context.ApiParameterDescription.Type) ?? context.ApiParameterDescription.Type;
        //    if (type.IsEnum)
        //    {
        //        AddRef(parameter.Extensions, type, context.SchemaRegistry);
        //        parameter.Required = type == context.ApiParameterDescription.Type;
        //    }
        //    else if (parameter is NonBodyParameter nonBodyParam && (type.IsArray || (type.IsGenericType && type.GetInterfaces().Contains(typeof(IEnumerable)))))
        //    {
        //        var itemType = type.GetElementType() ?? type.GenericTypeArguments.First();
        //        AddRef(nonBodyParam.Items.Extensions, itemType, context.SchemaRegistry);
        //    }
        //}

        //private static void AddRef(Dictionary<string, object> extensions, Type type, ISchemaRegistry schemaRegistry)
        //{
        //    var registeredType = schemaRegistry.GetOrRegister(type);
        //    if (registeredType.Ref != null)
        //    {
        //        extensions.Add("$ref", registeredType.Ref);
        //    }
        //}
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}