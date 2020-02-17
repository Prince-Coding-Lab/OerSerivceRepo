using System.ComponentModel;
using System.Runtime.Serialization;

namespace Core.Enums
{
    public enum DbReturnValue
    {
        [EnumMember(Value = "Create Success")]
        [Description("Record created successfully")]
        CreateSuccess = 100,

        [EnumMember(Value = "Update Success")]
        [Description("Record updated successfully")]
        UpdateSuccess = 101,

        [EnumMember(Value = "Not Exists")]
        [Description("Record does not exists")]
        NotExists = 102,

        [EnumMember(Value = "Delete Success")]
        [Description("Record deleted successfully")]
        DeleteSuccess = 103,

        [EnumMember(Value = "Active Try Delete")]
        [Description("Active record can not be deleted")]
        ActiveTryDelete = 104,

        [EnumMember(Value = "Record Exists")]
        [Description("Record exists in database")]
        RecordExists = 105,

        [EnumMember(Value = "Updation Failed")]
        [Description("Record updation failed")]
        UpdationFailed = 106,

        [EnumMember(Value = "Creation Failed")]
        [Description("Record creation failed")]
        CreationFailed = 107,

        [EnumMember(Value = "Email Exists")]
        [Description("Email Already Exists")]
        EmailExists = 108,

        [EnumMember(Value = "Email Not Exists")]
        [Description("Email Does not exists")]
        EmailNotExists = 109,

        [EnumMember(Value = "Password Incorrect")]
        [Description("Password Does not match")]
        PasswordIncorrect = 110,

        [EnumMember(Value = "Authentication Success")]
        [Description("Customer Authentication Success")]
        AuthSuccess = 111,

        [EnumMember(Value = "Reason Unknown")]
        [Description("Operation Failed Due to Unknown Reason")]
        ReasonUnknown = 112,

        [EnumMember(Value = "No Records")]
        [Description("No Records Found")]
        NoRecords = 113,

        [EnumMember(Value = "DeleteFailed")]
        [Description("Failed to Delete Record")]
        DeleteFailed = 114,

        [EnumMember(Value = "Approved")]
        [Description("Approved")]
        Approved = 115,

        [EnumMember(Value = "AlreadyApproved")]
        [Description("Already Approved")]
        AlreadyApproved = 116,

        [EnumMember(Value = "ReasouceOnlyHiddenByAuthor")]
        [Description("Reasouce/Course comments can only be made hidden by author")]
        ReasouceOnlyHiddenByAuthor = 117,

    }
}
