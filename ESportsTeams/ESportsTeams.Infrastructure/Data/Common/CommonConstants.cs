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
        public const string UsersNotFoundMessage = "Users not found.";
        public const string EnterSearchValue = "Please provide search value.";

        //Suffix
        public const string AdminSuffix = "Administrator";

        //ArgumentExceptions
        public const string EventNotFound = "Event not found!";
        public const string UserNotFound = "User not found!";
        public const string TournamentNotFound = "Tournament not found!";
        public const string TeamNotFound = "Team not found!";
        public const string InvalidUser = "Invalid User!";
        public const string InvalidTournamentId = "Invalid tournament ID!";
        public const string DoNotOwnTeam = "You need to own a team to join this tournament!";
        public const string InvalidTeamCategory = "Your team category is not for this tournament!";
    }
}
