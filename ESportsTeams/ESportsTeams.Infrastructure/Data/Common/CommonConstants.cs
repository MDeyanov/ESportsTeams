using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Infrastructure.Data.Common
{
    public class CommonConstants
    {
        public const string ErrorUserId = "NoSuchUser";
        public const int AccountLockOutInMonths = 1;
        public const int AccountUnLockInDays = -1;

        //Messages type
        public const string ErrorMessage = "ErrorMessage";
        public const string WarningMessage = "WarningMessage";
        public const string SuccessMessage = "SuccessMessage";

        //Messages
        public const string NotFoundMessage = "Resouce not found.";

        //Suffix
        public const string AdminSuffix = "Administrator";
        
    }
}
