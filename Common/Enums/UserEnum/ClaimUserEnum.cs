using System.ComponentModel;

namespace Common.Enums.UserEnum;

public enum ClaimUserEnum
{
    [Description("UserName")] preferred_username = 1,
    [Description("UserName")] userid = 2,

}