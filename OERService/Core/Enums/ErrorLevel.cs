using System.ComponentModel;
using System.Runtime.Serialization;

namespace Core.Enums
{
    public enum ErrorLevel
    {
        [EnumMember(Value = "Critical")]
        [Description("Critical Error")]
        Critical = 1,

        [EnumMember(Value = "Medium")]
        [Description("Medium Error")]
        Medium = 2,

        [EnumMember(Value = "Low")]
        [Description("Low")]
        Low = 3,       
    }

    public enum CommonErrors
    {
        [EnumMember(Value = "Invalid email address")]
        [Description("Invalid email address")]
        InvalidEmail = 1,

        [EnumMember(Value = "NotWhiteListed")]
        [Description("Not White Listed")]
        NotWhiteListed = 2,

        [EnumMember(Value = "WhiteListed")]
        [Description("URL White Listed")]
        WhiteListed = 3,

        [EnumMember(Value = "AbuseReported")]
        [Description("Reported Successfully")]
        AbuseReported = 4,

        [EnumMember(Value = "AbuseReportFailed")]
        [Description("Report Failed")]
        AbuseReportFailed = 5,

        [EnumMember(Value = "CommentHide")]
        [Description("Comment has been made hidden")]
        CommentHide = 6,

        [EnumMember(Value = "CommentHideFailed")]
        [Description("Failed to Hide Comment")]
        CommentHideFailed = 7,

        [EnumMember(Value = "RatingSucess")]
        [Description("Sucessfully Rated Resource")]
        RatingSucess = 8,

        [EnumMember(Value = "RatingFailed")]
        [Description("Failed to Rate Resource")]
        RatingFailed = 9,


        [EnumMember(Value = "AlreadyRatedResource")]
        [Description("Already Rated This Resource")]
        AlreadyRatedResource = 10,

        [EnumMember(Value = "AlreadyReported")]
        [Description("Already Reported this Resource")]
        AlreadyReportedResource = 11,

        [EnumMember(Value = "AlreadyReportedCourse")]
        [Description("Already Reported this Course")]
        AlreadyReportedCourse = 12,

        [EnumMember(Value = "RatingCourseSucess")]
        [Description("Sucessfully Rated Course")]
        RatingCourseSucess = 13,

        [EnumMember(Value = "RatingCourseFailed")]
        [Description("Failed to Rate Course")]
        RatingCourseFailed = 14,

        [EnumMember(Value = "AlreadyRatedCourse")]
        [Description("Already Rated This Course")]
        AlreadyRatedCourse = 15,


    }



}
