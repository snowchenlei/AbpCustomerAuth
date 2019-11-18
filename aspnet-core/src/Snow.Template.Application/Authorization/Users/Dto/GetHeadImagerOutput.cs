using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.Template.Authorization.Users.Dto
{
    public class GetHeadImageOutput
    {
        public string HeadImagePath { get; set; }

        public GetHeadImageOutput(string headImagePath)
        {
            HeadImagePath = headImagePath;
        }
    }
}