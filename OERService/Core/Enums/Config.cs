using System.ComponentModel;
using System.Runtime.Serialization;

namespace Core.Enums
{
    public enum ConfiType
    {  
        [EnumMember(Value = "AWS")]
        [Description("AWS API Configuration")]
        AWS = 1
    }
}
