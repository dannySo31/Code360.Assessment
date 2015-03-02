using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Code360.Assessment.Web.Helpers
{
    public static class ValidationHelper
    {
        public const int UPLOAD_FILE_SIZE_LIMIT = 8000000;

        public const long UPLOAD_SAVED_FILE_SIZE_LIMIT = 4000000;
        public const int MAX_UPLOAD_FILE_WIDTH_PX = 300;
        public const int MAX_UPLOAD_FILE_HEIGHT_PX = 300;
        public const string NO_IMAGE="~/Content/Images/no-photo.jpg";
    }
}