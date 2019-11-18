using Abp.Web.Models;

namespace Snow.Template.Authorization.Users.Dto
{
    public class UpdateHeadImageOutput : ErrorInfo
    {
        public string FileName { get; set; }

        public string FileType { get; set; }

        public string FileToken { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public UpdateHeadImageOutput()
        {
        }

        public UpdateHeadImageOutput(ErrorInfo error)
        {
            Code = error.Code;
            Details = error.Details;
            Message = error.Message;
            ValidationErrors = error.ValidationErrors;
        }
    }
}