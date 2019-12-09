using System;
namespace API.V1.Contracts
{
    public static class ErrorMessages
    {
        public const string InvalidEmail = "Invalid Email Format";

        public const string EmailInuse = "Email is already in use";
       
        public const string InvalidProfileColor = "Invalid color format. Use Hex color format";

        public const string InvalidPasswordFormat = "Invalid Password. Must contain atleast: " +
            "8 characters, 1 upper case letter, 1 lower case letter, 1 number or special character";

        public const string FamilyDoesNotExist = "Family does not exist";

        public const string InvalidLogin = "Invalid Login";

        public const string ServerError = "Error, that's on us, please try again later";

        public const string MemberDoesNotExist = "Member(s) does not exist";

        public const string ParticipantDoesNotexist = "Participant(s) does not exist";

        public const string WeekDaysOnlySetIfFrequencyWeekly = "This should only be set if Frequency is Weekly";

        public const string RequiredWhenAllDayNotSet = "Required when all day is not set";
    }
}
