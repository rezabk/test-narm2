using System.ComponentModel.DataAnnotations;

namespace Common.Enums;

public enum SmsMessagesEnum
{
    [Display(Name = "کد شما : {0}")] Simple = 1,

    [Display(Name = "{0}")] TextBase = 2
}