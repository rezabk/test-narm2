using System.ComponentModel;

namespace Common.Enums;

public enum MessageId
{
    [Description("عملیات با موفقیت انجام شد.")]
    Success = 1,

    [Description("مشکلی در سیستم بوجود آمده است.")]
    Exception = -1,

    [Description("اطلاعات کاربر یافت نشد")]
    NotExistsUser = -2,

    [Description("رکوردی یافت نشد")] EntityDoesNotExist = -3,

    [Description("کاربر به این قسمت دسترسی ندارد")]
    AccessDenied = -4,

    [Description("حساب شما قفل شده است")] AccountLocked = -5,


    [Description("شماره موبایل وجود ندارد")]
    MobileNotFound = -6,

    [Description("کد اعتبار سنجی استفاده شده است")]
    CodeUsed = -7,

    [Description("کد اعتبار سنجی منقضی شده است")]
    CodeExpired = -8,

    [Description("کد اعتبار سنجی اشتباه است")]
    FalseCode = -9,

    [Description("ایمیل اشتباه است")] WrongEmail = -10,

    [Description("ایمیل کاربر در سیستم ثبت شده است")]
    EmailExits = -11,


    [Description("در این مرحله امکان انجام این عملیات نمی باشد")]
    ChangeStatusExceptionError = -12,

    [Description("برای انجام این عملیات باید اطلاعات کاربری خود را تکمیل کنید")]
    CompleteProfile = -13,


    [Description("بعضی از مقادیر اشتباه است")]
    ValidationError = -14,


    [Description("در این مرحله امکان این عملیات وجود ندارد.")]
    ThereIsNoFlow = -15,


    [Description("رمز عبور در سیستم ثبت است، لطفا رمز عبور را تغییر دهید")]
    PasswordSetBefore = -16,

    [Description("لطفا ابتدا رمز عبور خود را در سیستم ثبت کنید")]
    NoPassword = -17,

    [Description("کاربر انتخاب شده استاد است")]
    AlreadyTeacher = -18,

    [Description("بعضی از ورودی ها نا معتبر می باشند")]
    FormValidationExceptionOccured = -19,

    [Description("شماره دانشجویی یا رمز عبور اشتباه است")]
    WrongLoginInfo = -20,

    [Description("کاربر با این شماره دانشجویی وجود دارد")]
    UserExists = -21,

    [Description("این شماره موبایل در سایت ثبت شده است")]
    PhoneNumberExits = -23,

    [Description("شما معلم این کلاس نیستید")]
    AccessToClassDenied = -24,

    [Description("ظرفیت کلاس پر شده است")] ClassCapacityLimit = -25,

    [Description("شما عضو این کلاس هستید")]
    AlreadyInClass = -26,

    [Description("شما به این سوال پاسخ داده اید")]
    AlreadyAnsweredQuestion = -27,

    [Description("مهلت ارسال پاسخ تمرین به تمام رسیده")]
    DeadlineReached = -28, 
    
    [Description("شما درخواست جدید دارید")]
    OpenRequest = -29,
}