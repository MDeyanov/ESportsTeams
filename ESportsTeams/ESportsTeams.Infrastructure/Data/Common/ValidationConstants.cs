using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Infrastructure.Data.Common
{
    public class ValidationConstants
    {
        public class UserConstraints
        {
            public const int PasswordMinLength = 4;
            public const int PasswordMaxLength = 50;

            public const int LastNameMinLength = 2;
            public const int LastNameMaxLength = 40;

            public const int FirstNameMinLength = 2;
            public const int FirstNameMaxLength = 40;

            public const int UserNameMinLength = 4;
            public const int UserNameMaxLength = 15;
        }

        public class TeamConstraints
        {
            public const int TeamNameMinLength = 4;
            public const int TeamNameMaxLength = 50;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 500;
        }
        public class TournamentConstraints
        {
            public const int TitleMinLength = 4;
            public const int TitleMaxLength = 50;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 500;
        }
        public class BlogConstraints
        {
            public const int BlogNameMinLength = 4;
            public const int BlogNameMaxLength = 30;

            public const int BlogContentMinLength = 10;
            public const int BlogContentMaxLength = 500;
        }
        public class EventConstrains
        {
            public const int EventTitleMinLength = 2;
            public const int EventTitleMaxLength = 50;

            public const int EventDescriptionMinLength = 10;
            public const int EventDescriptionMaxLength = 500;
        }
        public class ReviewConstrains
        {
            public const int ContentMessageMaxLength = 500;
        }
    }
}
